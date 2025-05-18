using LearnManager.Models;
using LearnManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LearnManager.Controllers
{
    public class FormationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string category)
        {
            var formations = _context.Formations
                .Include(f => f.Formateur)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                formations = formations.Where(f => f.Categorie == category);
            }

            return View(formations.ToList());
        }

        public IActionResult Details(int id)
        {
            var formation = _context.Formations
                .Include(f => f.Formateur)
                .FirstOrDefault(f => f.Id == id);

            if (formation == null)
            {
                return NotFound();
            }

            return View(formation);
        }
    }
}