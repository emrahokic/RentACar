using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Data;

namespace RentACar.Areas.Uposlenik.Controllers
{
    [Authorize(Roles = "Administrator,Uposlenik")]
    [Area("Uposlenik")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController()
        {
        }

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(nameof(Index));
        }
    }
}