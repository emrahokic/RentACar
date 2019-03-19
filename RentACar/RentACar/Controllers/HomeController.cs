using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACar.Data;
using RentACar.Models;
using RentACar.ViewModels;

namespace RentACar.Controllers
{
    public class HomeController : Controller
    {

        private UserManager<ApplicationUser> _signInManager;
        private ApplicationDbContext db;
        public HomeController(ApplicationDbContext _db, UserManager<ApplicationUser> signInManager)
        {
            db = _db;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
           

            return View();
        }

        public async Task<IActionResult>  GetCartItems()
        {
            string SifraRezervacije = null;

            try
            {
                SifraRezervacije = Request.Cookies["Sesion"];
            }
            catch (Exception)
            {

                throw;
            }
            string[] array = SifraRezervacije.Split("-");
            long ticks =long.Parse(array[0]);
            DateTime vrijemeRezervacije = new DateTime(ticks);
            DateTime vrijemeIsteka = new DateTime(ticks).AddHours(24);
            int satiDoIsteka = (vrijemeIsteka - DateTime.Now).Hours;
            int minutaDoIsteka = (vrijemeIsteka - DateTime.Now).Minutes;
            
            NotifikacijaKorpa notifikacijaKorpa = null;
           
            if (SifraRezervacije != null)
            {
                notifikacijaKorpa = new NotifikacijaKorpa();
                Rezervacija r = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
                notifikacijaKorpa.URL_Slike = db.Slika.Where(y => y.VoziloID == r.VoziloID).Select(s => s.URL).FirstOrDefault();
                notifikacijaKorpa.VoziloName = db.Vozilo.Where(y => y.VoziloID == r.VoziloID).Select(n => n.Naziv).FirstOrDefault();
                notifikacijaKorpa.VoziloBrand = db.Vozilo.Where(y => y.VoziloID == r.VoziloID).Select(n => n.Brend.Naziv).FirstOrDefault();
                notifikacijaKorpa.VrijemeIstekaH = satiDoIsteka+"";
                notifikacijaKorpa.VrijemeIstekaM = minutaDoIsteka + "";
                return PartialView("Cart",notifikacijaKorpa);
            }
            return PartialView("Cart",notifikacijaKorpa);

        }
        public IActionResult Privacy()
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
