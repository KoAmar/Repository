using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Models.DatabaseModels;

namespace Repository.Controllers
{
    public class DisciplineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisciplineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Discipline
        public ActionResult Index()
        {
            return View(_context.Disciplines.Include(d => d.CourseProjects).ToList());
        }

        public ActionResult IndexDiscipline(string id)
        {
            var model = _context
                .Disciplines
                .Include(d => d.CourseProjects)
                .FirstOrDefault(d => d.Id == id);

            if (model != null)
            {
                ViewData["Title"] = $"Проекты с дисциплиной \"{model.Name}\"";

                return View("MyProjects", model.CourseProjects);
            }

            return NotFound();
        }


        // GET: Discipline/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Discipline/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name)
        {
            try
            {
                _context.Disciplines.Add(new Discipline() {Id = Guid.NewGuid().ToString(), Name = name});
                _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Discipline/Edit/5
        public ActionResult Edit(string id)
        {
            var discipline = _context.Disciplines.Find(id);
            return discipline == null ? View("Error") : View(discipline);
        }

        // POST: Discipline/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Discipline discipline)
        {
            if (!ModelState.IsValid) return View(discipline);
            try
            {
                _context.Disciplines.Update(discipline);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View("Error");
            }
        }

        // GET: Discipline/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var discipline = await _context.Disciplines
                .Include(d => d.CourseProjects)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (discipline == null) return View("Error");

            if (discipline.CourseProjects.Any())
            {
                var s = string.Join("<br/>- ", discipline.CourseProjects.Select(c => c.Title));
                return View("Message", $"На дисциплину ссылаются следующие проекты:<br>- {s}");
            }

            _context.Disciplines.Remove(discipline);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}