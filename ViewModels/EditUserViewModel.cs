using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required] [Display(Name = "Email")] public string Email { get; set; }

        [Required] [Display(Name = "Имя")] public string FirstName { get; set; }

        [Required] [Display(Name = "Фамилия")] public string Surname { get; set; }

        [Display(Name = "Отчество")] public string Patronymic { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        // [StringLength(maximumLength: 9, MinimumLength = 7)]
        [Display(Name = "Номер группы")]
        public string GroupNumber { get; set; }
    }
}