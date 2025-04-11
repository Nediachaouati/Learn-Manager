using LearnManager.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}
