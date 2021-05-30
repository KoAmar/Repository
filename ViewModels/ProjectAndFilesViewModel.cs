using System.Collections.Generic;
using Repository.Models.DatabaseModels;

namespace Repository.ViewModels
{
    public class ProjectAndFilesViewModel
    {
        public CourseProject Project { get; set; }
        public List<FileModel> FileModels { get; set; }
    }
}