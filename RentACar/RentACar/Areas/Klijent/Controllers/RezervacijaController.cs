using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Data;
using RentACar.Models;
using RentACar.Areas.Klijent.ViewModels;
using RentACar.Helper;
using RentACar.ViewModels;

namespace RentACar.Areas.Klijent.Controllers
{
    [Area("Klijent")]
    public class RezervacijaController : Controller
    {
        private  UserManager<ApplicationUser> _signInManager;
        private ApplicationDbContext db;
        private RezervacijaDodajVM MainModel;
        public RezervacijaController(ApplicationDbContext _db, UserManager<ApplicationUser> signInManager)
        {
            db = _db;
            _signInManager = signInManager;
        }


        //Rezervacije za Klijenta
        [Authorize(Roles = "Klijent")]
        public IActionResult Index()
        {
            int KlijentID = int.Parse(_signInManager.GetUserId(User));
            RezervacijaIndexVM model = new RezervacijaIndexVM() {
                Rows = db.Rezervacija.Where(z => z.KlijentID == KlijentID).Select(x => new RezervacijaIndexVM.Row()
                {
                     RezervacijaID = x.RezervacijaID,
                     Vozilo = x.Vozilo.Naziv,
                     DatumPreuzimanja = x.DatumPreuzimanja,
                     DatumPovrata = x.DatumPovrata,
                     Poslovnica = x.Poslovnica.Naziv,
                     Zakljucen = x.Zakljucen,
                     Brend = x.Vozilo.Brend.Naziv
                }).ToList()

            };
            return View(model);
        }

        public IActionResult Dodaj(int vozilo)
        {
            MainModel = new RezervacijaDodajVM();

            MainModel.Vozilo = db.Vozilo.FirstOrDefault(x => x.VoziloID == vozilo);
            var idP = db.TrenutnaPoslovnica.FirstOrDefault(t => t.VoziloID == vozilo).PoslovnicaID;
            VozilaVM.Row vr = new VozilaVM.Row
            {
                Brend = db.Brend.FirstOrDefault(z => z.BrendID == MainModel.Vozilo.BrendID).Naziv,
                Naziv = MainModel.Vozilo.Naziv,
                Cijena = MainModel.Vozilo.Cijena,
                PoslovnicaNaziv = db.Poslovnica.FirstOrDefault(x => x.PoslovnicaID == idP).Naziv,
                Slika = db.Slika.FirstOrDefault(y => y.VoziloID == vozilo && y.Pozicija == 1)
            };
            MainModel.VoziloVM = vr;
            return View(nameof(Dodaj), MainModel);
        }

        [HttpPost]
        public IActionResult DodatneUslugePV()
        {
            
            return PartialView(nameof(DodatneUslugePV));
        }

        //[HttpPost]
        //public IActionResult Dodaj(RezervacijaDodajVM model)
        //{
        //    Rezervacija rezervacija = new Rezervacija() {
        //        DatumRezervacije = System.DateTime.Now,
        //        DatumPreuzimanja = model.DatumPreuzimanja,
        //        DatumPovrata = model.DatumPovrata,
        //        NacinPlacanja = model.NacinPlacanja,
        //        KlijentID = int.Parse(_signInManager.GetUserId(User)),
        //        BrojDanaIznajmljivanja = dateDiff(model.DatumPreuzimanja, model.DatumPovrata),
        //        Cijena=1,
        //        //VoziloID=model.VoziloID,
        //        //PoslovnicaID=db.TrenutnaPoslovnica.FirstOrDefault(s=>s.VoziloID==model.VoziloID).PoslovnicaID,
        //        Zakljucen= (int)InfoRezervacija.U_Obradi



        //    };
        //    db.Rezervacija.Add(rezervacija);
        //    db.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        [Authorize(Roles = "Klijent")]
        public IActionResult DodajOcjenuKomentar(string Poruka, int? Ocjena, int RezervacijaID)
        {
            if (Ocjena == null && Poruka != null)
            {
                Ocjena = 0;
            }
            if (Ocjena>=0 && Poruka != null && RezervacijaID>0)
            {

            int KlijentID = int.Parse(_signInManager.GetUserId(User));
            Rezervacija tempR = db.Rezervacija.Find(RezervacijaID);
            ApplicationUser tempK = db.Users.Find(KlijentID);
            OcjenaRezervacija rezervacija = new OcjenaRezervacija()
            {
                Poruka = Poruka,
                OcjenaVrijednost =(int) Ocjena,
                Rezervacija = tempR,
                DatumOcjene = DateTime.Now,
                Klijent = tempK
            };
            db.OcjenaRezervacija.Add(rezervacija);
            db.SaveChanges();
            return PartialView("OcjenaKomentar",rezervacija);
            }
            return PartialView("OcjenaKomentar", null);

        }

        private int dateDiff(DateTime datumPreuzimanja, DateTime datumPovrata)
        {
            TimeSpan t = datumPovrata - datumPreuzimanja;
            return t.Days;
        }

        public IActionResult OcjenaKomentar(int id)
        {
            OcjenaRezervacija model = db.OcjenaRezervacija.Where(y => y.RezervacijaID == id).Select(c => new OcjenaRezervacija
            {
                Poruka = c.Poruka,
                OcjenaVrijednost = c.OcjenaVrijednost
            }).FirstOrDefault();

            return PartialView(nameof(OcjenaKomentar), model);
        }


        [Authorize(Roles = "Klijent")]
        public IActionResult Detalji(int id)
        {
            RezervacijaDetaljnoVM model = db.Rezervacija.Where(z => z.RezervacijaID == id).Select(x => new RezervacijaDetaljnoVM {
                RezervacijaID = x.RezervacijaID,
                Grad = x.Poslovnica.Grad.Naziv,
                SlikaVozila= db.Slika.Where(sl => sl.VoziloID == x.VoziloID && sl.Pozicija ==1 ).Select(c => new Slika
                {
                    Name = c.Name,
                    SlikaID = c.SlikaID,
                    URL = c.URL,
                    Pozicija = c.Pozicija
                }).FirstOrDefault(),
                DatumPreuzimanja = x.DatumPreuzimanja,
                DatumPovrata = x.DatumPovrata,
                Zakljucen = x.Zakljucen,
                Cijena = x.Cijena,
                BrojDanaIznajmljivanja = x.BrojDanaIznajmljivanja,
                NacinPlacanja = x.NacinPlacanja,
                Vozilo = x.Vozilo.Naziv,
                Brend = x.Vozilo.Brend.Naziv,
                Poslovnica = x.Poslovnica.Naziv,
                dodatneUsluge = db.RezervisanaUsluga.Where(u => u.RezervacijaID == x.RezervacijaID).Select(ru => new RezervacijaDetaljnoVM.Row{
                    Naziv = ru.DodatneUsluge.Naziv,
                    RezervisanaUslugaID = ru.RezervisanaUslugaID,
                    Cijena = db.DodatneUsluge.FirstOrDefault(d => d.DodatneUslugeID == ru.DodatneUslugeID).Cijena,
                    Kolicina = ru.Kolicina,
                    Opis = db.DodatneUsluge.FirstOrDefault(d => d.DodatneUslugeID == ru.DodatneUslugeID).Opis,
                    UkupnaCijenaUsluge = ru.UkupnaCijenaUsluge
                }).ToList()
            }).FirstOrDefault();

            return View(nameof(Detalji), model);
        }

    }
}