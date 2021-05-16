using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Repository.Models.DatabaseModels
{
    public class CourseProject
    {
        // [Key]

        public string Id { get; set; }
        [NotNull]
        public DateTime CreationDate { get; set; }
        [MaxLength(450)]
        [NotNull]
        public string UserId { get; set; }
        [NotNull]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}