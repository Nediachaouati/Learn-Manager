using LearnManager.Models;
using System.Diagnostics;
using LearnManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult AdminDashboard()
        {
            return View();
        }
        // Afficher la liste des formateurs
        public IActionResult ManageFormateurs()
        {
            var formateurs = _context.Users.Where(u => u.Role == "formateur").ToList();
            return View(formateurs);
        }


        // GET: Ajouter un formateur
        [HttpGet]
        public IActionResult AddFormateur()
        {
            return View();
        }

        // POST: Ajouter un formateur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFormateur(User formateur)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == formateur.Email))
                {
                    ModelState.AddModelError("Email", "Cet email est déjà utilisé.");
                    return View(formateur);
                }

                // Hasher le mot de passe et définir le rôle "Formateur"
                formateur.Password = BCrypt.Net.BCrypt.HashPassword(formateur.Password);
                formateur.Role = "formateur";
                formateur.CreatedAt = DateTime.Now;

                _context.Users.Add(formateur);
                _context.SaveChanges();

                return RedirectToAction("ManageFormateurs");
            }
            return View(formateur);
        }

        // POST: Supprimer un formateur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFormateur(int id)
        {
            var formateur = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "formateur");
            if (formateur != null)
            {
                _context.Users.Remove(formateur);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageFormateurs");
        }

        public IActionResult ManageApprenant()
        {
            return View();
        }

        public IActionResult ManageFinancier()
        {
            return View();
        }

        public IActionResult ManageFormations()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
