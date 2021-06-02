using System.Collections.Generic;

namespace Repository.Models.DatabaseModels
{
    public class Discipline
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<CourseProject> CourseProjects { get; set; }
    }
}