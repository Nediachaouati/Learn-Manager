using System.Diagnostics;
using LearnManager.Models;
using LearnManager.Models.ViewModels;
using LearnManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeIndexViewModel
            {
                Formations = _context.Formations
                    .Include(f => f.Formateur)
                    .OrderByDescending(f => f.CreatedAt)
                    .Take(4)
                    .ToList()
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}