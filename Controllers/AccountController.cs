using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Models.Business;
using Repository.Models.DatabaseModels;
using Repository.ViewModels;

namespace Repository.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var isEmailAlreadyExists = _userManager.Users.Any(x => x.Email == model.Email);
            if (isEmailAlreadyExists)
            {
                ModelState.AddModelError("Email", "Пользователь с такой почтой уже существует");
                return View(model);
            }

            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                Year = model.Year
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new {userId = user.Id, code},
                    HttpContext.Request.Scheme);
                var emailService = new EmailService();

                await emailService.SendEmailAsync(model.Email, "Confirm your account",
                    $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                const string message = "Для завершения регистрации проверьте электронную почту" +
                                       " и перейдите по ссылке, указанной в письме";
                return View("Message", message);
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) return View("Error");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return View("Error");

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded) return View("Error");

            await _userManager.AddToRoleAsync(user, "User");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel {ReturnUrl = returnUrl});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
                // проверяем, подтвержден ли email
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                    return View(model);
                }

            var result =
                await _signInManager.PasswordSignInAsync
                    (model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неправильный логин и (или) пароль");

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                // пользователь с данным email может отсутствовать в бд
                // тем не менее мы выводим стандартное сообщение, чтобы скрыть 
                // наличие или отсутствие пользователя в бд
                return View("ForgotPasswordConfirmation");

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(
                "ResetPassword",
                "Account",
                new {userId = user.Id, code},
                HttpContext.Request.Scheme
            );
            var emailService = new EmailService();
            await emailService.SendEmailAsync(model.Email, "Reset Password",
                $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
            return View("ForgotPasswordConfirmation");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return View("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded) return View("ResetPasswordConfirmation");

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        // [HttpGet]
        // [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}