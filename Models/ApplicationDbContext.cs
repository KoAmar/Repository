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
            // Database.EnsureCreated();
        }
        
        public DbSet<CourseProject> CourseProjects { get; set; }
        public DbSet<File> Files { get; set; }
        //
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     // modelBuilder.Ignore<Company>();
        // }
    }
}