using System;
using System.ComponentModel.DataAnnotations;
using Repository.Models.Business;

namespace Repository.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        
        [Required]
        [YearValidation(1900)]
        [Display(Name = "Год рождения")]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}