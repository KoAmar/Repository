using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Repository.Models;
using Repository.Models.DatabaseModels;
using Repository.ViewModels;

namespace Repository.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<User> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var model = new EditUserViewModel
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty,
                    "Пользователь не найден");
                return View(model);
            }

            user.Email = model.Email;
            user.UserName = model.Email;
            user.Surname = model.Surname;
            user.FirstName = model.FirstName;
            user.Patronymic = model.Patronymic;
            user.Year = model.Year;

            if (model.Password != null)
                if (HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) is IPasswordHasher<User>
                    passwordHasher)
                    user.PasswordHash = passwordHasher.HashPassword(user,
                        model.Password);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty,
                    error.Description);
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

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
                return View("Message",
                    errorMessage);
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return View("Message",
                    errorMessage);
            }
        }

        public IActionResult FilterIndex(string roleId)
        {
            
            var userIds= _context.UserRoles
                .Where(a => a.RoleId == roleId)
                .Select(b => b.UserId).Distinct() .ToList();           

           var listUsers=   _context.Users.Where(a => userIds.Any(c => c == a.Id)).ToList();
            
            
            return View("Index", listUsers);
        }
    }
}