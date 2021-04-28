using Microsoft.AspNetCore.Identity;

namespace Repository.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
    }
}