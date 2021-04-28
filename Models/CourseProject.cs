using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models
{
    public class CourseProject
    {
        // [Key]
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        // [ForeignKey("UserId")]
        public string UserId { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}