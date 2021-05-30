using System.ComponentModel.DataAnnotations;

namespace Repository.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пароль должен содержать как минимум 8 символов", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}