using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Models.DatabaseModels;

namespace Repository.ViewModels
{
    public class IndexProjectViewModel
    {
        public enum Sort
                {
                    Date,
                    ReverseDate,
                    Name,
                    ReverseName
                }
        public int ProjectsPerPage { get; set; }

        public Sort SortBy { get; set; }
        
        public IEnumerable<CourseProject> Projects { get; set; }  
        
        public int ProjectPage { get; set; }  
        public int CurrentPage { get; set; }  
  
        public int PageCount()  
        {  
            return Convert.ToInt32(Math.Ceiling(Projects.Count() / (double)ProjectsPerPage));  
        }


        public IEnumerable<CourseProject> PaginatedProjects()  
        {  
            var start = (CurrentPage - 1) * ProjectsPerPage;  
            return Projects.Skip(start).Take(ProjectsPerPage);  
        }
    }
}