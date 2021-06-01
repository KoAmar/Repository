using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DatabaseModels
{
    public class FileModel
    {
        [MaxLength(450)] [Required] public string Id { get; set; }

        public string Name { get; set; }

        [Required] public string FilePath { get; set; }

        [Required] [MaxLength(450)] public string ProjectId { get; set; }

        public CourseProject Project { get; set; }
    }
}