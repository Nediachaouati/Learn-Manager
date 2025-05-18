using LearnManager.Models;
using System.Diagnostics;
using LearnManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnManager.Controllers
{
    public class ApprenantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApprenantController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ApprenantDashboard()
        {
            var formations = _context.Formations.ToList();
            return View(formations);
        }

        [HttpPost]
        public IActionResult InscriptionFormation(int formationId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue || HttpContext.Session.GetString("UserRole") != "apprenant")
            {
                return RedirectToAction("Login", "Account");
            }

            // Vérifier si l'apprenant déjà inscrit
            var alreadyInscrit = _context.Inscriptions
                .Any(i => i.ApprenantId == userId.Value && i.FormationId == formationId);
            if (alreadyInscrit)
            {
                TempData["Message"] = "Vous êtes déjà inscrit à cette formation.";
                return RedirectToAction("ApprenantDashboard");
            }

            // Ajouter l'inscription
            var inscription = new Inscription
            {
                ApprenantId = userId.Value,
                FormationId = formationId,
                DateInscription = DateTime.Now
            };
            _context.Inscriptions.Add(inscription);
            _context.SaveChanges();

            TempData["Message"] = "Inscription réussie !";
            return RedirectToAction("ApprenantDashboard");
        }

        public IActionResult MesFormations()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue || HttpContext.Session.GetString("UserRole") != "apprenant")
            {
                return RedirectToAction("Login", "Account");
            }

            var formations = _context.Inscriptions
                .Where(i => i.ApprenantId == userId.Value)
                .Select(i => i.Formation)
                .ToList();
            return View(formations);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
