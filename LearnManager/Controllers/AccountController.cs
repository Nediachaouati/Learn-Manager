using LearnManager.Models;
using LearnManager.Services;
using Microsoft.AspNetCore.Mvc;



namespace LearnManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Cet email est déjà utilisé.");
                    return View(user);
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Role = "apprenant"; 
                user.CreatedAt = DateTime.Now;

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                // Stocker l’état de connexion 
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role);

                switch (user.Role)
                {
                    case "admin":
                        return RedirectToAction("AdminDashboard", "Admin"); 
                    case "formateur":
                        return RedirectToAction("FormateurDashboard", "Formateur");
                    case "apprenant":
                        return RedirectToAction("ApprenantDashboard", "Apprenant");
                    case "financier":
                        return RedirectToAction("FinancierDashboard", "Financier");
                    default:
                        return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.ErrorMessage = "Email ou mot de passe incorrect.";
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
         
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

    