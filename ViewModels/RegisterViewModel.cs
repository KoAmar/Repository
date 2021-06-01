using System.ComponentModel.DataAnnotations;
using Repository.Models.Business;

namespace Repository.ViewModels
{
    public class RegisterViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Требуется ввести почту")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Требуется ввести имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Требуется ввести фамилию")] [Display(Name = "Фамилия")] public string Surname { get; set; }

        [Display(Name = "Отчество")] public string Patronymic { get; set; }

        [Required(ErrorMessage = "Требуется ввести год рождения")]
        [YearValidation(1900)]
        [Display(Name = "Год рождения")]
        public int Year { get; set; }

        // [StringLength(maximumLength: 9, MinimumLength = 7)]
        [Display(Name = "Номер группы")]
        public string GroupNumber { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [Compare("Password",
            ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}