using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using RentACar.Areas.Uposlenik.ViewModels;
using RentACar.Data;
using RentACar.Models;

namespace RentACar.Areas.Uposlenik.Controlers
{
    [Authorize(Roles = "Uposlenik")]
    [Area("Uposlenik")]
    public class ProfilController : Controller
    {
        private UserManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _context;
        private IHostingEnvironment _he;

        public ProfilController(UserManager<ApplicationUser> signInManager, ApplicationDbContext context, IHostingEnvironment he)
        {
            _signInManager = signInManager;
            this._context = context;
            _he = he;
        }

        public IActionResult Index(string msg)
        {
            int id = int.Parse(_signInManager.GetUserId(User));
            ProfilDetaljnoVM model = _context.Users.Where(x => x.Id == id).Select(x => new ProfilDetaljnoVM
            {
                Adresa = x.Adresa,
                BrTelefona = x.PhoneNumber,
                DatumRodjenja = x.DatumRodjenja,
                Email = x.Email,
                Grad = x.Grad.Naziv,
                Ime = x.Ime,
                Username = x.UserName,
                JMBG = x.JMBG,
                Prezime = x.Prezime,
                Slika = x.Slika,
                Spol = x.Spol
            }).FirstOrDefault();
            if (msg != null)
            {
            model.Notification = msg;
            }
            if (model.Slika == null)
            {
                model.Slika = Path.Combine("\\images\\Profilne\\65U.svg");
            }
            else
            {
                model.Slika = Path.Combine("\\images\\Profilne", model.Slika);
            }

            List<SelectListItem> listagradova = new List<SelectListItem>();
            listagradova.AddRange(_context.Grad.Select(x => new SelectListItem()
            {
                Value = x.GradID.ToString(),
                Text = x.Naziv
            }));

            model.Gradovi = listagradova;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfilImage(ProfilDetaljnoVM model, IFormFile SlikaURL)
        {
            int id = int.Parse(_signInManager.GetUserId(User));
            ApplicationUser user = _context.Users.Find(id);

            user.Slika = SlikaURL.FileName;
            var filePath = Path.Combine(_he.WebRootPath + "\\images\\Profilne", SlikaURL.FileName);
            SlikaURL.CopyTo(new FileStream(filePath, FileMode.Create));

            _context.SaveChanges();
            _context.Dispose();

            var values = new RouteValueDictionary();
            values.Add("msg", "ok");
            return RedirectToAction(nameof(Index),values);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProfilUrediSnimi(ProfilDetaljnoVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(nameof(Index));
            }

            int id = int.Parse(_signInManager.GetUserId(User));
            var user = _context.Users.Find(id);

            user.Adresa = model.Adresa;
            user.Email = model.Email;
            user.UserName = model.Username;
            user.PhoneNumber = model.BrTelefona;
            user.Ime = model.Ime;
            user.Prezime = model.Prezime;
            user.Spol = model.Spol;
            user.DatumRodjenja = model.DatumRodjenja;
            user.JMBG = model.JMBG;
            user.GradID = model.GradID;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}