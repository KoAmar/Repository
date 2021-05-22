using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Models.DatabaseInterfaces;
using Repository.Models.DatabaseModels;
using Repository.ViewModels;

namespace Repository.Controllers
{
    public class ProjectsController : Controller
    {
        

        private readonly IProjectsRepos _courseProjects;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public ProjectsController(IProjectsRepos courseProjects,
            ApplicationDbContext context,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _courseProjects = courseProjects;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index(IndexProjectViewModel.Sort sort, int pageId = 1)
        {
            var projects = _courseProjects.GetAllCourseProjects().ToList();

            IEnumerable<CourseProject> sortedProjects = sort switch
            {
                IndexProjectViewModel.Sort.Date => projects.OrderByDescending(p => p.CreationDate),
                IndexProjectViewModel.Sort.ReverseDate => projects.OrderBy(p => p.CreationDate),
                IndexProjectViewModel.Sort.Name => projects.OrderBy(p => p.Title),
                IndexProjectViewModel.Sort.ReverseName => projects.OrderByDescending(p => p.Title),
                _ => projects
            };

            var indexProjectViewModel = new IndexProjectViewModel
            {  
                ProjectsPerPage = 5,  
                Projects = sortedProjects,
                SortBy = sort,
                CurrentPage = pageId
            };  
            return View(indexProjectViewModel);  
        }

        public IActionResult ProjectInfo(string id)
        {
            var courseProject = _courseProjects.GetCourseProject(id);
            if (courseProject == null) return NotFound();

            var fileModels = _context.Files.Where(file => file.ProjectId == courseProject.Id).ToList();

            var projectAndFiles = new ProjectAndFilesViewModel()
            {
                Project = courseProject,
                FileModels = fileModels
            };

            return View(projectAndFiles);
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProject(CourseProject courseProject)
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToAction("Login", "Account", new {returnUrl = Request.Path.Value});

            if (courseProject == null) return RedirectToAction("Index");
            courseProject.UserId = _userManager.GetUserId(User);
            if (!ModelState.IsValid) return View(courseProject);


            courseProject.CreationDate = DateTime.Now;
            courseProject.Id = Guid.NewGuid().ToString();

            _courseProjects.AddCourseProject(courseProject);

            return RedirectToAction("EditProject", new {courseProject.Id});
        }

        [HttpGet]
        public IActionResult EditProject(string id)
        {
            var courseProject = _courseProjects.GetCourseProject(id);
            if (courseProject == null) return NotFound();

            var fileModels = _context.Files
                .Where(file => file.ProjectId == courseProject.Id).ToList();

            var projectAndFiles = new ProjectAndFilesViewModel()
            {
                Project = courseProject,
                FileModels = fileModels
            };

            return View(projectAndFiles);
        }

        [HttpPost]
        public IActionResult EditProject(ProjectAndFilesViewModel projectAndFilesViewModel)
        {
            if (projectAndFilesViewModel == null) return NotFound();
            if (!ModelState.IsValid) return View(projectAndFilesViewModel);

            var newProject = projectAndFilesViewModel.Project;
            var project = _context.CourseProjects.FindAsync(newProject.Id).Result;
            if (project == null) return NotFound();

            project.Title = newProject.Title;
            project.Description = newProject.Description;

            _context.CourseProjects.Update(project);
            _context.SaveChangesAsync();

            return RedirectToAction("EditProject", new {id = project.Id});
        }

        public IActionResult DeleteProject(string id)
        {
            var project = _courseProjects.GetCourseProject(id);

            if (project == null) return NotFound();

            const string errorMessage = "Ошибка удалениия, вероятнее всего к этому курсовому проекту" +
                                        " остались привязаны файлы или другая сущности.";

            try
            {
                _courseProjects.DeleteCourseProject(id);
                return RedirectToAction("Index");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                return View("Message", errorMessage);
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return View("Message", errorMessage);
            }
        }
    }
}