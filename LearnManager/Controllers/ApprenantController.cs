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
            return View();
        }

        public IActionResult ShowFormations()
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
