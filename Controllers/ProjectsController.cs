using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Repository.Models.DatabaseInterfaces;

namespace Repository.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectsRepos _courseProjects;

        public ProjectsController(IProjectsRepos courseProjects)
        {
            _courseProjects = courseProjects;
        }

        public IActionResult Index()
        {
            return View(_courseProjects.GetAllCourseProjects());
        }

        public IActionResult Project(string id)
        {
            return View(_courseProjects.GetCourseProject(id));
        }
        
    }
}