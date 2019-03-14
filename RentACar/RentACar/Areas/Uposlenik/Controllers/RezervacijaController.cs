﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Data;
using RentACar.Models;
using RentACar.Areas.Uposlenik.ViewModels;
using RentACar.Helper;

namespace RentACar.Areas.Uposlenik.Controllers
{
    [Authorize(Roles = "Uposlenik")]
    [Area("Uposlenik")]
    public class RezervacijaController : Controller
    {
        private  UserManager<ApplicationUser> _signInManager;

        private ApplicationDbContext db;
        public RezervacijaController(ApplicationDbContext _db, UserManager<ApplicationUser> signInManager)
        {
            db = _db;
            _signInManager = signInManager;
        }

        //Rezervacije za Uposlenika id = userID
        [Authorize(Roles = "Uposlenik")]
        public IActionResult Index(int id)
        {
            int ID = int.Parse(_signInManager.GetUserId(User));
            int PoslovnicaID = db.UgovorZaposlenja.FirstOrDefault(g => g.UposlenikID == ID).PoslovnicaID;
            RezervacijaIndexVM model = new RezervacijaIndexVM()
            {
                Rows = db.Rezervacija.Where(z => z.PoslovnicaID == PoslovnicaID && (z.UposlenikID == ID || z.UposlenikID == null)).Select(x => new RezervacijaIndexVM.Row()
                {
                    RezervacijaID = x.RezervacijaID,
                    Vozilo = x.Vozilo.Naziv,
                    DatumPreuzimanja = x.DatumPreuzimanja,
                    DatumPovrata = x.DatumPovrata,
                    UposlenikID = x.UposlenikID,
                    Poslovnica = x.Poslovnica.Naziv,
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



        //izmjeniti dodavanje

        [HttpPost]
        public IActionResult Dodaj(RezervacijaDodajVM model)
        {
            Rezervacija rezervacija = new Rezervacija() {
                DatumRezervacije = System.DateTime.Now,
                DatumPreuzimanja = model.DatumPreuzimanja,
                DatumPovrata = model.DatumPovrata,
                NacinPlacanja = model.NacinPlacanja,
                KlijentID = int.Parse(_signInManager.GetUserId(User)),
                BrojDanaIznajmljivanja = dateDiff(model.DatumPreuzimanja, model.DatumPovrata),
                Cijena=1,
                VoziloID=model.VoziloID,
                PoslovnicaID=db.TrenutnaPoslovnica.FirstOrDefault(s=>s.VoziloID==model.VoziloID).PoslovnicaID,
                Zakljucen=(int) InfoRezervacija.U_Obradi
                
                

            };
            db.Rezervacija.Add(rezervacija);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        //Ovo je za klijenta mozes obrisati ako ti ne treba
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DodajOcjenuKomentar(string Poruka, int Ocjena, int RezervacijaID)
        {
            int KlijentID = int.Parse(_signInManager.GetUserId(User));
            Rezervacija tempR = db.Rezervacija.Find(RezervacijaID);
            ApplicationUser tempK = db.Users.Find(KlijentID);
            OcjenaRezervacija rezervacija = new OcjenaRezervacija()
            {
                Poruka = Poruka,
                OcjenaVrijednost = Ocjena,
                Rezervacija = tempR,
                Klijent = tempK
            };
            db.OcjenaRezervacija.Add(rezervacija);
            db.SaveChanges();
            return Redirect("Detalji/"+tempR.RezervacijaID);
        }

        private int dateDiff(DateTime datumPreuzimanja, DateTime datumPovrata)
        {
            TimeSpan t = datumPovrata - datumPreuzimanja;
            return t.Days;
        }



        public IActionResult Detalji(int id)
        {
            RezervacijaDetaljnoVM model = db.Rezervacija.Where(z => z.RezervacijaID == id).Select(x => new RezervacijaDetaljnoVM {
                RezervacijaID = x.RezervacijaID,
                Grad = x.Poslovnica.Grad.Naziv,
                DatumRezervacije = x.DatumRezervacije,
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
                ocjenaRezervacija = db.OcjenaRezervacija.Where(y => y.RezervacijaID == x.RezervacijaID).Select(c => new OcjenaRezervacija
                {
                    Poruka = c.Poruka,
                    OcjenaVrijednost = c.OcjenaVrijednost
                }).FirstOrDefault(),
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