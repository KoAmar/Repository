using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.ViewModels;

namespace Repository.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index","Projects");
            // return View();
        }

        
        [Route("Error/{statusCode:int}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            ViewBag.ErrorMessage = statusCode switch
            {
                404 => "Извините, запрашиваемый ресурс не может быть найден",
                _ => $"Ошибка {statusCode}"
            };

            return View("NotFound");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}