using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
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

        public ProjectsController(IProjectsRepos courseProjects, ApplicationDbContext context)
        {
            _courseProjects = courseProjects;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_courseProjects.GetAllCourseProjects());
        }

        public IActionResult ProjectInfo(string id)
        {
            var courseProject = _courseProjects.GetCourseProject(id);
            if (courseProject == null)
            {
                ;
                return NotFound();
            }

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
            if (courseProject == null) return RedirectToAction("Index");
            if (!ModelState.IsValid) return View(courseProject);

            courseProject.UserId = "5a73781f-2288-4d04-a36a-99702716cefb";

            courseProject.CreationDate = DateTime.Now;
            courseProject.Id = Guid.NewGuid().ToString();
            _courseProjects.AddCourseProject(courseProject);

            return RedirectToAction("EditProject", courseProject.Id);
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

        //todo this method implementation 
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