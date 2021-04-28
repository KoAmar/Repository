using Microsoft.AspNetCore.Mvc;
using Repository.Models;

namespace Repository.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectsRepos _courseProjects;

        public ProjectsController(IProjectsRepos courseProjects)
        {
            _courseProjects = courseProjects;
        }

        // GET
        public IActionResult Index()
        {
            
            return View(_courseProjects.GetAllCourseProjects());
        }
    }
}