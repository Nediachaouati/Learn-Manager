using LearnManager.Models;
using LearnManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnManager.Controllers
{
    public class FinancierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinancierController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult FinancierDashboard()
        {
            return View();
        }
        public IActionResult ManageDepenses()
        {
            if (HttpContext.Session.GetString("UserRole") != "financier")
            {
                return RedirectToAction("Login", "Account");
            }

            var depenses = _context.Depenses
                .Include(d => d.Formateur)
                .Include(d => d.Formation)
                .ToList();

            return View(depenses);
        }

        // GET: Ajouter une dépense
        [HttpGet]
        public IActionResult AddDepense()
        {
            if (HttpContext.Session.GetString("UserRole") != "financier")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Formateurs = _context.Users.Where(u => u.Role == "formateur").ToList();
            ViewBag.Formations = _context.Formations.ToList();
            return View();
        }

        // POST: Ajouter une dépense
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDepense(Depense depense)
        {
            if (HttpContext.Session.GetString("UserRole") != "financier")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                depense.DateDepense = DateTime.Now;
                _context.Depenses.Add(depense);
                _context.SaveChanges();
                return RedirectToAction("ManageDepenses");
            }

            ViewBag.Formateurs = _context.Users.Where(u => u.Role == "formateur").ToList();
            ViewBag.Formations = _context.Formations.ToList();
            return View(depense);
        }

        // GET: Modifier une dépense
        [HttpGet]
        public IActionResult EditDepense(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "financier")
            {
                return RedirectToAction("Login", "Account");
            }

            var depense = _context.Depenses.Find(id);
            if (depense == null)
            {
                return NotFound();
            }

            ViewBag.Formateurs = _context.Users.Where(u => u.Role == "formateur").ToList();
            ViewBag.Formations = _context.Formations.ToList();
            return View(depense);
        }

        // POST: Modifier une dépense
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDepense(Depense depense)
        {
            if (HttpContext.Session.GetString("UserRole") != "financier")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var existingDepense = _context.Depenses.Find(depense.Id);
                if (existingDepense == null)
                {
                    return NotFound();
                }

                existingDepense.Type = depense.Type;
                existingDepense.Montant = depense.Montant;
                existingDepense.FormateurId = depense.FormateurId;
                existingDepense.FormationId = depense.FormationId;
                existingDepense.Description = depense.Description;
                existingDepense.DateDepense = depense.DateDepense;

                _context.SaveChanges();
                return RedirectToAction("ManageDepenses");
            }

            ViewBag.Formateurs = _context.Users.Where(u => u.Role == "formateur").ToList();
            ViewBag.Formations = _context.Formations.ToList();
            return View(depense);
        }

        // POST: Supprimer une dépense
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDepense(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "financier")
            {
                return RedirectToAction("Login", "Account");
            }

            var depense = _context.Depenses.Find(id);
            if (depense != null)
            {
                _context.Depenses.Remove(depense);
                _context.SaveChanges();
            }

            return RedirectToAction("ManageDepenses");
        }
    
}
}
