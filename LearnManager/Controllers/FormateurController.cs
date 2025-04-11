using LearnManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnManager.Controllers
{
    public class FormateurController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormateurController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult FormateurDashboard()
        {
            return View();
        }
    }
}
