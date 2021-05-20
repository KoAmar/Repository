using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Models.DatabaseModels;
using Repository.ViewModels;

namespace Repository.Controllers
{
    //todo uncomment
    // [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var model = new RegisterViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Year = user.Year
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден");
                return View(model);
            }

            user.Email = model.Email;
            user.UserName = model.Email;
            user.Year = model.Year;

            if (HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) is IPasswordHasher<User>
                passwordHasher)
                user.PasswordHash = passwordHasher.HashPassword(user, model.Password);


            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return RedirectToAction("Index");

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            
            const string errorMessage = "Ошибка удалениия, вероятнее всего к этому" +
                                        " пользователю приявязан проект или другая сущность.";
            try
            {
                await _userManager.DeleteAsync(user);
                    return RedirectToAction("Index");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                return View("Message", errorMessage);
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return View("Message", errorMessage);
            }
        }
    }
}