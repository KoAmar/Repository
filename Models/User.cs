using Microsoft.AspNetCore.Identity;

namespace Repository.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}