using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Data;
using RentACar.Models;
using RentACar.Models.ViewModels;

namespace RentACar.Controllers
{
    public class RezervacijaController : Controller
    {
        private  UserManager<ApplicationUser> _signInManager;

        private ApplicationDbContext db;
        public RezervacijaController(ApplicationDbContext _db, UserManager<ApplicationUser> signInManager)
        {
            db = _db;
            _signInManager = signInManager;
        }
        public IActionResult Index(int? KlijentID)
        {
            KlijentID = int.Parse(_signInManager.GetUserId(User));
            RezervacijaIndexVM model = new RezervacijaIndexVM() {
                Rows = db.Rezervacija.Where(z => z.KlijentID == KlijentID || KlijentID == null).Select(x => new RezervacijaIndexVM.Row()
                {
                     RezervacijaID = x.RezervacijaID,
                     Vozilo = x.Vozilo.Naziv,
                     DatumPreuzimanja = x.DatumPreuzimanja,
                     DatumPovrata = x.DatumPovrata,
                     Poslovnica = db.UgovorZaposlenja.FirstOrDefault(y=> y.UposlenikID == x.UposlenikID).Poslovnica.Naziv,
                     VrstaRezervacije = x.VrstaRezervacije,
                     Zakljucen = x.Zakljucen,
                     Brend = x.Vozilo.Brend.Naziv
                }).ToList()

            };
            return View(model);
        }

        public IActionResult Dodaj()
        {
            List<SelectListItem> listaVozila = new List<SelectListItem>();
            listaVozila.AddRange(db.Vozilo.Select(x=>new SelectListItem() {
                     Value = x.VoziloID.ToString(),
                     Text = x.Naziv
            }));
            RezervacijaDodajVM model = new RezervacijaDodajVM();

            model.Vozila = listaVozila;
            return View(model);
        }

        [HttpPost]
        public IActionResult Dodaj(RezervacijaDodajVM model)
        {
            Rezervacija rezervacija = new Rezervacija() {
                DatumRezervacije = System.DateTime.Now,
                DatumRentanja = System.DateTime.Now,
                DatumPreuzimanja = model.DatumPreuzimanja,
                DatumPovrata = model.DatumPovrata,
                NacinPlacanja = model.NacinPlacanja,
                KlijentID = int.Parse(_signInManager.GetUserId(User)),
                BrojDanaIznajmljivanja=1,
                Cijena=1,
                VoziloID=model.VoziloID,
                PoslovnicaID=db.TrenutnaPoslovnica.FirstOrDefault(s=>s.VoziloID==model.VoziloID).PoslovnicaID,
                VrijemePovrata=model.DatumPovrata,
                VrijemePreuzimanja=model.DatumPreuzimanja,
                VrstaRezervacije=model.VrstaRezervacije,
                Zakljucen=false,
                UposlenikID= 3
                
                

            };
            db.Rezervacija.Add(rezervacija);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}