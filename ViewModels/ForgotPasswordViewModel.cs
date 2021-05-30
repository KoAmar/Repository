using System.ComponentModel.DataAnnotations;

namespace Repository.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}