using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Repository.Models.DatabaseModels
{
    public class User : IdentityUser
    {
        [Required] public int Year { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string Surname { get; set; }

        public string Patronymic { get; set; }
    }
}