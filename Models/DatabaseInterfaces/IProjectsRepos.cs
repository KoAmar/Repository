using System.Collections.Generic;
using Repository.Models.DatabaseModels;

namespace Repository.Models.DatabaseInterfaces
{
    public interface IProjectsRepos
    {
        IEnumerable<CourseProject> GetAllCourseProjects();
        CourseProject GetCourseProject(string id);
        CourseProject DeleteCourseProject(string id);
        CourseProject AddCourseProject(CourseProject courseProject);
        CourseProject UpdateCourseProject(CourseProject courseProjectChanges);
    }
}