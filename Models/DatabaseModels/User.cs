using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Repository.Models.DatabaseModels
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        [NotNull]
        public string FirstName { get; set; }
        [NotNull]
        public string Surname { get; set; }
        public string Patronymic { get; set; }
    }
}