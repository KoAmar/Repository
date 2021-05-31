using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Models.DatabaseModels;

namespace Repository.Models
{
    public sealed class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // CourseProjects = courseProjects;
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<CourseProject> CourseProjects { get; set; }

        public DbSet<FileModel> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedRoles(modelBuilder);
            SeedUsers(modelBuilder);
            SeedUserRoles(modelBuilder);
        }

        private void SeedUsers(ModelBuilder builder)
        {

            User user = new User()
            {
               Id = "68ccc708-a60f-457a-9562-2b4e3daa8c41",
               Year = 2000,
               FirstName = "Pavel",
               Surname = "Halavanau",
               UserName = "pa1318vel@gmail.com",
               NormalizedUserName = "PA1318VEL@GMAIL.COM",
               Email = "pa1318vel@gmail.com",
               NormalizedEmail = "PA1318VEL@GMAIL.COM",
               EmailConfirmed = true,
               PasswordHash = "AQAAAAEAACcQAAAAEB6KIJYXSh7Sn+mG9MtfhG88yEauqFtQ+XowmR1BpOM7j1mNjcaFe+z//+a0v8w2xA==",
               SecurityStamp = "WPH2JEXSMURUHSJ6U3RACIKZPDE6W2E7",
               ConcurrencyStamp = "c7df8df5 - 773d - 4b24-a757-e6ab4c7ad499",
               PhoneNumberConfirmed = false,
               TwoFactorEnabled = false,
               LockoutEnabled = true,
               AccessFailedCount = 0
            };
            
            builder.Entity<User>()
                .HasData(user);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>()
                .HasData(
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
                        Name = "User",
                        ConcurrencyStamp = "2",
                        NormalizedName = "USER",
                    }
                );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>()
                .HasData(
                    new IdentityUserRole<string>()
                    {
                        RoleId = "fab4fac1-c546-41de-aebc-a14da6895711",
                        UserId = "68ccc708-a60f-457a-9562-2b4e3daa8c41"
                    }
                );
        }
    }
}