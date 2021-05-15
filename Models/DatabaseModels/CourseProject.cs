using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DatabaseModels
{
    public class CourseProject
    {
        // [Key]
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        [MaxLength(450)]
        public string UserId { get; set; }
        
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}