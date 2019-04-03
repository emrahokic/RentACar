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
using RentACar.Areas.Klijent.ViewModels;
using RentACar.Data;
using RentACar.Models;

namespace RentACar.Areas.Klijent.Controllers
{
    [Authorize(Roles = "Klijent")]
    [Area("Klijent")]
    public class ProfilController : Controller
    {
        private UserManager<ApplicationUser> _signInManager;
        private ApplicationDbContext db;
        private IHostingEnvironment _He;


        public ProfilController(UserManager<ApplicationUser> signInManager, ApplicationDbContext db, IHostingEnvironment _he)
        {
            _signInManager = signInManager;
            this.db = db;
            _He = _he;
        }
        
        public IActionResult Index(string msg)
        {
            
            int id = int.Parse(_signInManager.GetUserId(User));
            ProfilDetaljnoVM model = db.Users.Where(x => x.Id == id).Select(x => new ProfilDetaljnoVM
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
            listagradova.AddRange(db.Grad.Select(x => new SelectListItem()
            {
                Value = x.GradID.ToString(),
                Text = x.Naziv
            }));

            model.Gradovi = listagradova;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfilImage(ProfilDetaljnoVM model, IFormFile SlikaURL)
        {

            int id = int.Parse(_signInManager.GetUserId(User));
            ApplicationUser ap = db.Users.Find(id);

            ap.Slika = SlikaURL.FileName;

            var filePath = Path.Combine(_He.WebRootPath + "\\images\\Profilne", SlikaURL.FileName);
            SlikaURL.CopyTo(new FileStream(filePath, FileMode.Create));

            db.SaveChanges();
            db.Dispose();

            var values = new RouteValueDictionary();
            values.Add("msg", "ok");
            return RedirectToAction(nameof(Index), values);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfil(ProfilDetaljnoVM model)
        {
            int id = int.Parse(_signInManager.GetUserId(User));
            ApplicationUser ap = db.Users.Find(id);

            ap.Adresa = model.Adresa;
            ap.Email = model.Email;
            ap.UserName = model.Username;
            ap.PhoneNumber = model.BrTelefona;
            ap.Ime = model.Ime;
            ap.Prezime = model.Prezime;
            ap.Spol = model.Spol;
            ap.DatumRodjenja = model.DatumRodjenja;
            ap.JMBG = model.JMBG;
            ap.GradID = model.GradID ;
            
            db.SaveChanges();
            db.Dispose();


            return RedirectToAction(nameof(Index));
        }
    }
}