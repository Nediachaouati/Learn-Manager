using System.Security.Claims;
using LearnManager.Models.ViewModels;
using LearnManager.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnManager.Controllers
{
    
    public class FormateurController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormateurController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ShowFormations()
        {
            // Vérifier l'état de la session
            var userRole = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetInt32("UserId");

            
            Console.WriteLine($"ShowFormations: UserRole='{userRole}', UserId={userId}, SessionId={HttpContext.Session.Id}");

            if (string.IsNullOrEmpty(userRole))
            {
                Console.WriteLine("Redirecting to Login: UserRole is null or empty");
                return RedirectToAction("Login", "Account");
            }

            if (userRole.ToLower() != "formateur")
            {
                Console.WriteLine($"Redirecting to Login: UserRole='{userRole}' is not 'formateur'");
                return RedirectToAction("Login", "Account");
            }

            if (!userId.HasValue)
            {
                Console.WriteLine("Redirecting to Login: UserId is null");
                return RedirectToAction("Login", "Account");
            }

            // Récupérer formations assignées au formateur
            var formations = _context.Formations
                .Where(f => f.FormateurId == userId.Value)
                .ToList();

            Console.WriteLine($"Found {formations.Count} formations for FormateurId={userId.Value}");
            return View(formations);
        }

        public IActionResult ShowApprenant()
        {
          
            
            if (HttpContext.Session.GetString("UserRole") != "formateur")
            {
                return RedirectToAction("Login", "Account");
            }

            // Récupérer id formateur 
            var formateurId = HttpContext.Session.GetInt32("UserId");
            if (!formateurId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // Récupérer inscriptions pour les formations du formateur
            var inscriptions = _context.Inscriptions
                .Include(i => i.Apprenant)
                .Include(i => i.Formation)
                .Where(i => i.Formation.FormateurId == formateurId && i.Apprenant.Role == "apprenant")
                .Select(i => new ApprenantInscriptionViewModel
                {
                    ApprenantId = i.Apprenant.Id,
                    ApprenantNom = i.Apprenant.Nom ?? i.Apprenant.Email,
                    ApprenantPrenom = i.Apprenant.Prenom ?? "-",
                    ApprenantEmail = i.Apprenant.Email,
                    FormationTitre = i.Formation.Titre,
                    FormationDateDebut = i.Formation.DateDebut,
                    FormationDateFin = i.Formation.DateFin,
                    DateInscription = i.DateInscription
                })
                .ToList();

            return View(inscriptions);
        }
        

        public IActionResult FormateurDashboard()
        {
            return View();
        }
    }
}
