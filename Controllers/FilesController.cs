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

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile == null) return RedirectToAction("Index");
            
            const string webRootPath = "files";
            var uniqueFileName = $"{Guid.NewGuid()}_{uploadedFile.FileName}";
            var projectFilePath = Path.Combine(webRootPath, uniqueFileName);
            var fullFilePath = Path.Combine(_appEnvironment.WebRootPath,projectFilePath);

            await uploadedFile.CopyToAsync(new FileStream(fullFilePath, FileMode.Create));

            var file = new FileModel
            {
                Name = uploadedFile.FileName,
                FilePath = projectFilePath,
                Id = Guid.NewGuid().ToString()
            };

            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}