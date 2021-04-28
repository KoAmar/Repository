using System.Collections;
using System.Collections.Generic;

namespace Repository.Models
{
    public interface IProjectsRepos
    {
        IEnumerable<CourseProject> GetAllCourseProjects();
        CourseProject GetCourseProject(int id);
        CourseProject DeleteCourseProject(int id);
        CourseProject AddCourseProject(CourseProject courseProject);
        CourseProject UpdateCourseProject(CourseProject courseProjectChanges);
    }
}