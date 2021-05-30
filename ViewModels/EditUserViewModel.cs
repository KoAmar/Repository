using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.ViewModels
{
    public class EditUserViewModel : IValidatableObject
    {
        private const int MinValidYear = 1900;
        public string Id { get; set; }

        [Required] [Display(Name = "Email")] public string Email { get; set; }

        [Required] [Display(Name = "Имя")] public string FirstName { get; set; }

        [Required] [Display(Name = "Фамилия")] public string Surname { get; set; }

        [Display(Name = "Отчество")] public string Patronymic { get; set; }

        [Required]
        [Display(Name = "Год рождения")]
        public int Year { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Year < MinValidYear || Year > DateTime.Now.Year)
                yield return new ValidationResult(
                    $"Classic movies must have a release year no later than {MinValidYear}.",
                    new[] {nameof(Year)});
        }
    }
}