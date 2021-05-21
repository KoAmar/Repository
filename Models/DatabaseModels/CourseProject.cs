using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Repository.Models.DatabaseModels
{
    public class CourseProject
    {
        // [Key]

        public string Id { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [MaxLength(450)]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Требуется название!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Требуется описание!")]
        public string Description { get; set; }
    }
}