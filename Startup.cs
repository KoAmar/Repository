using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository.Models;
using Repository.Models.Business;
using Repository.Models.DatabaseInterfaces;
using Repository.Models.DatabaseInterfaces.Implementations;
using Repository.Models.DatabaseModels;

namespace Repository
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IUserValidator<User>, CustomEmailValidator>();
            services.AddScoped<IProjectsRepos, CourseProjRepos>();
            services.AddIdentity<User, IdentityRole>(opts =>
                {
                    opts.Password.RequiredLength = 8; // минимальная длина
                    opts.Password.RequireNonAlphanumeric = false; // требуются ли не алфавитно-цифровые символы
                    opts.Password.RequireLowercase = true; // требуются ли символы в нижнем регистре
                    opts.Password.RequireUppercase = true; // требуются ли символы в верхнем регистре
                    opts.Password.RequireDigit = true; // требуются ли цифры

                    opts.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            // app.UseStatusCodePagesWithReExecute("/error", "?code={0}");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}