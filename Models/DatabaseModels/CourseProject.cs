using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DatabaseModels
{
    public class CourseProject
    {
        public string Id { get; set; }

        [Required] public DateTime CreationDate { get; set; }


        [Required(ErrorMessage = "Требуется название!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Требуется описание!")]
        public string Description { get; set; }

        [MaxLength(450)] public string UserId { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Выберите дисциплину!")]
        public string DisciplineId { get; set; }

        public Discipline Discipline { get; set; }

        [Required(ErrorMessage = "Выберите руководителя!")]
        [MaxLength(450)]
        public string ProjectManagerId { get; set; }

        public User ProjectManager { get; set; }
    }
}