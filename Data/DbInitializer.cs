using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Repository.Models;

namespace Repository.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Roles.Any())
            {
                return; // DB has been seeded
            }
            
            var roles = new IdentityRole[]
            {
                new IdentityRole()
                {
                    Id = "fab4fac1-c546-41de-aebc-a14da6895711",
                    Name = "Admin",
                    ConcurrencyStamp = "1",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole()
                {
                    Id = "c7b013f0-5201-4317-abd8-c211f91b7330",
                    Name = "Student",
                    ConcurrencyStamp = "2",
                    NormalizedName = "STUDENT",
                },
                new IdentityRole()
                {
                    Id = "33",
                    Name = "Teacher",
                    ConcurrencyStamp = "3",
                    NormalizedName = "TEACHER",
                }
            };
            foreach (var role in roles)
            {
                
                    context.Roles.Add(role);
                
            }

            context.SaveChanges();
        }
    }
}