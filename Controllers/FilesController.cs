using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Repository.Models.DatabaseModels;

namespace Repository.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public FilesController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View(_context.Files.ToList());
        }

        //TODO understand why image duplicates in view 
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile == null) return RedirectToAction("Index");

            // путь к папке Files
            var path = "/files/" + uploadedFile.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            await using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            var file = new FileModel {Name = uploadedFile.FileName, FilePath = path, Id = Guid.NewGuid().ToString()};

            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}