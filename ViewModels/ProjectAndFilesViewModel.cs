using System.Collections;
using System.Collections.Generic;
using Repository.Models.DatabaseModels;

namespace Repository.ViewModels
{
    public class ProjectAndFilesViewModel
    {
        public CourseProject Project;
        public IEnumerable<FileModel> FileModels;
    }
}