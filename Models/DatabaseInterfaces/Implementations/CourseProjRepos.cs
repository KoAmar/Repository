using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Repository.Models.DatabaseModels;

namespace Repository.Models.DatabaseInterfaces.Implementations
{
    public class CourseProjRepos : IProjectsRepos
    {
        private readonly ApplicationDbContext _context;

        public CourseProjRepos(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CourseProject> GetAllCourseProjects()
        {
            return _context.CourseProjects;
        }

        public CourseProject GetCourseProject(string id)
        {
            return _context.CourseProjects.Find(id);
        }

        public CourseProject DeleteCourseProject(string id)
        {
            var project = _context.CourseProjects.Find(id);

            if (project == null)
            {
                return null;
            }

            _context.Remove(project);
            _context.SaveChanges();

            return project;
        }


        public CourseProject AddCourseProject(CourseProject courseProject)
        {
            _context.CourseProjects.Add(courseProject);
            _context.SaveChanges();
            return courseProject;
        }

        public CourseProject UpdateCourseProject(CourseProject courseProjectChanges)
        {
            var project = _context.Attach(courseProjectChanges);

            project.State = EntityState.Modified;
            _context.SaveChanges();

            return courseProjectChanges;
        }
    }
}