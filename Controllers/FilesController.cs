using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Repository.Models.DatabaseModels;
using Repository.ViewModels;

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

        public IActionResult UploadFile(string projectId)
        {
            return View(new ProjectIdViewModel {ProjectId = projectId});
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(string projectId, IFormFile uploadedFile)
        {
            if (uploadedFile == null) return RedirectToAction("Index", "Projects");

            var webRootPath = "files";
            var uniqueFileName = $"{Guid.NewGuid()}_{uploadedFile.FileName}";
            var projectFilePath = Path.Combine(webRootPath, uniqueFileName);
            var fullFilePath = Path.Combine(_appEnvironment.WebRootPath, projectFilePath);

            await uploadedFile.CopyToAsync(new FileStream(fullFilePath, FileMode.Create));

            var file = new FileModel
            {
                Name = uploadedFile.FileName,
                FilePath = projectFilePath,
                ProjectId = projectId,
                Id = Guid.NewGuid().ToString()
            };

            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return RedirectToAction("EditProject", "Projects", new {id = projectId});
        }
    }
}