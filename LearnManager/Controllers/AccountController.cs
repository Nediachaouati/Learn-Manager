using Microsoft.AspNetCore.Mvc;
using LearnManager.Models;
using LearnManager.Services;

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
            // Journal pour débogage
            Console.WriteLine($"Login attempt: Email={email}");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Veuillez fournir un email et un mot de passe.";
                Console.WriteLine("Login failed: Email or password is empty");
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                // Stocker les informations dans la session
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role?.ToLower() ?? string.Empty);
                HttpContext.Session.SetInt32("UserId", user.Id); // Ajout de UserId

                
                Console.WriteLine($"Login successful: UserEmail={user.Email}, UserRole={user.Role?.ToLower()}, UserId={user.Id}");
                Console.WriteLine($"Session after login: UserEmail={HttpContext.Session.GetString("UserEmail")}, UserRole={HttpContext.Session.GetString("UserRole")}, UserId={HttpContext.Session.GetInt32("UserId")}");

                switch (user.Role?.ToLower())
                {
                   
                    case "admin":
                        return RedirectToAction("ManageFormations", "Admin");
                    case "formateur":
                        return RedirectToAction("ShowFormations", "Formateur");
                    case "financier":
                        return RedirectToAction("ManageDepenses", "Financier");
                    case "apprenant":
                        return RedirectToAction("ApprenantDashboard", "Apprenant");
                    default:
                        return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.ErrorMessage = "Email ou mot de passe incorrect.";
            Console.WriteLine("Login failed: Invalid email or password");
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Console.WriteLine("Logout: Clearing session");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}