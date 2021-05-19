using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            if (courseProject == null) return NotFound();

            var fileModels = _context.Files.Where(file => file.ProjectId == courseProject.Id);

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

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult CreateProject(CourseProject courseProject)
        {
            if (courseProject == null) return RedirectToAction("Index");
            if (!ModelState.IsValid) return View(courseProject);

            courseProject.UserId = "5a73781f-2288-4d04-a36a-99702716cefb";

            courseProject.CreationDate = DateTime.Now;
            courseProject.Id = Guid.NewGuid().ToString();
            _courseProjects.AddCourseProject(courseProject);

            return RedirectToAction("EditProject",courseProject.Id);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult EditProject(string id)
        {
            var courseProject = _courseProjects.GetCourseProject(id);
            if (courseProject == null) return NotFound();

            var fileModels = _context.Files
                .Where(file => file.ProjectId == courseProject.Id);

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
            return NotFound();
        }
    }
}