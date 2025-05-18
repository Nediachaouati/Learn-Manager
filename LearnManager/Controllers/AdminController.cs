using LearnManager.Models;
using System.Diagnostics;
using LearnManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LearnManager.Models.ViewModels;

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
        // Afficher liste formateurs
        public IActionResult ManageFormateurs()
        {
            var formateurs = _context.Users.Where(u => u.Role == "formateur").ToList();
            return View(formateurs);
        }


        // GET: Ajouter formateur
        [HttpGet]
        public IActionResult AddFormateur()
        {
            return View();
        }

        // POST: Ajouter formateur
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

                
                formateur.Password = BCrypt.Net.BCrypt.HashPassword(formateur.Password);
                formateur.Role = "formateur";
                formateur.CreatedAt = DateTime.Now;

                _context.Users.Add(formateur);
                _context.SaveChanges();

                return RedirectToAction("ManageFormateurs");
            }
            return View(formateur);
        }

        // POST: Supprimer formateur
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
            if (HttpContext.Session.GetString("UserRole") != "admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var inscriptions = _context.Inscriptions
                .Include(i => i.Apprenant)
                .Include(i => i.Formation)
                    .ThenInclude(f => f.Formateur)
                .Where(i => i.Apprenant.Role == "apprenant")
                .Select(i => new ApprenantInscriptionViewModel
                {
                    ApprenantId = i.Apprenant.Id,
                    ApprenantNom = i.Apprenant.Nom ?? i.Apprenant.Email,
                    ApprenantPrenom = i.Apprenant.Prenom ?? "-",
                    ApprenantEmail = i.Apprenant.Email,
                    FormationTitre = i.Formation.Titre,
                    FormationDateDebut = i.Formation.DateDebut,
                    FormationDateFin = i.Formation.DateFin,
                    DateInscription = i.DateInscription,
                    FormateurNom = i.Formation.Formateur != null ? (i.Formation.Formateur.Nom ?? i.Formation.Formateur.Email) : "Non assigné",
                    FormateurPrenom = i.Formation.Formateur != null ? (i.Formation.Formateur.Prenom ?? "-") : "-"
                })
                .ToList();

            return View(inscriptions);
        }

        public IActionResult ManageFormations()
        {
            var formations = _context.Formations
        .Include(f => f.Formateur) 
        .ToList();
            return View(formations);
        }

        public IActionResult AddFormation()
        {
            ViewBag.Formateurs = _context.Users
                .Where(u => u.Role == "formateur")
                .ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFormation(Formation formation, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Formateurs = _context.Users
                    .Where(u => u.Role == "formateur")
                    .ToList();
                return View(formation);
            }

            // image
            if (Image != null && Image.Length > 0)
            {
                
                var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(Image.FileName).ToLower();
                if (!validExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Image", "Veuillez sélectionner une image au format JPG ou PNG.");
                    ViewBag.Formateurs = _context.Users
                        .Where(u => u.Role == "formateur")
                        .ToList();
                    return View(formation);
                }

                
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                // Sauvegarder le fichier
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                
                formation.ImageUrl = $"/images/{fileName}";
            }
            else
            {
                // Image par défaut 
                formation.ImageUrl = "/images/default-formation.jpg";
            }

            
            formation.CreatedAt = DateTime.Now;

            // Ajouter formation 
            _context.Formations.Add(formation);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageFormations");
        }

        // GET: Modifier formation
        [HttpGet]
        public IActionResult EditFormation(int id)
        {
            var formation = _context.Formations.Find(id);
            if (formation == null)
            {
                return NotFound();
            }
            ViewBag.Formateurs = _context.Users.Where(u => u.Role == "Formateur").ToList() ?? new List<User>(); // Ajouté
            return View(formation);
        }

        // POST: Modifier une formation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditFormation(Formation formation)
        {
            
            if (!string.IsNullOrEmpty(formation.Prix) && !decimal.TryParse(formation.Prix, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var prixDecimal))
            {
                ModelState.AddModelError("Prix", "Le prix doit être un montant valide (ex. : 499.99).");
            }

            if (ModelState.IsValid)
            {
                var existingFormation = _context.Formations.Find(formation.Id);
                if (existingFormation == null)
                {
                    return NotFound();
                }
                existingFormation.Titre = formation.Titre;
                existingFormation.Description = formation.Description;
                existingFormation.Categorie = formation.Categorie;
                existingFormation.Niveau = formation.Niveau;
                existingFormation.FormateurId = formation.FormateurId;
                existingFormation.Format = formation.Format;
                existingFormation.DateDebut = formation.DateDebut;
                existingFormation.DateFin = formation.DateFin;
                existingFormation.Duree = formation.Duree;
                existingFormation.Prix = formation.Prix;
                _context.SaveChanges();
                return RedirectToAction("ManageFormations");
            }
            ViewBag.Formateurs = _context.Users.Where(u => u.Role == "Formateur").ToList() ?? new List<User>();
            return View(formation);
        }

        // POST: Supprimer une formation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFormation(int id)
        {
            var formation = _context.Formations.Find(id);
            if (formation != null)
            {
                _context.Formations.Remove(formation);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageFormations");
        }

        // Afficher liste des responsables financiers
        public IActionResult ManageFinanciers()
        {
            var financiers = _context.Users.Where(u => u.Role == "financier").ToList();
            return View(financiers);
        }

        // GET: Ajouter un responsable financier
        [HttpGet]
        public IActionResult AddFinancier()
        {
            return View();
        }

        // POST: Ajouter un responsable financier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFinancier(User financier)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == financier.Email))
                {
                    ModelState.AddModelError("Email", "Cet email est déjà utilisé.");
                    return View(financier);
                }

                financier.Password = BCrypt.Net.BCrypt.HashPassword(financier.Password);
                financier.Role = "financier";
                financier.CreatedAt = DateTime.Now;

                _context.Users.Add(financier);
                _context.SaveChanges();

                return RedirectToAction("ManageFinanciers");
            }
            return View(financier);
        }

        // POST: Supprimer un responsable financier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFinancier(int id)
        {
            var financier = _context.Users.FirstOrDefault(u => u.Id == id && u.Role == "financier");
            if (financier != null)
            {
                _context.Users.Remove(financier);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageFinanciers");
        }


        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
