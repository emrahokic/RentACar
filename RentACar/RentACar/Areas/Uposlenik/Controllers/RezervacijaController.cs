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
using RentACar.Areas.Uposlenik.ViewModels;
using RentACar.Helper;
using System.Security.Claims;

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

       
        public async Task<IActionResult> Dodaj()
        {
            var model = new VoziloRezervacijaVM
            {
                vozila = db.Vozilo.Select(v => new VoziloRezervacijaVM.Row
                {
                    Boja =v.Boja,
                    Brend = v.Brend.Naziv,
                    Naziv = v.Naziv,
                    BrojMjesta = v.BrojMjesta,
                    BrojVrata = v.BrojVrata,
                    Domet = v.Domet,
                    VoziloID = v.VoziloID,
                    GodinaProizvodnje = v.GodinaProizvodnje,
                    Gorivo = v.Gorivo,
                    GrupniTipVozila = v.GrupniTipVozila,
                    Klima = v.Klima,
                    Model = v.Model,
                    slike =  db.Slika.Where(s => s.Pozicija == 1 && s.VoziloID == v.VoziloID).Select(p => p.URL).FirstOrDefault(),
                    SnagaMotora = v.SnagaMotora,
                    TipVozila = v.TipVozila,
                    Transmisija = v.Transmisija
                }).ToList()
            };
            return View(model);
        }
        public IActionResult Rezervisi(int id)
        {
            var model = new RezervacijaPocetakVM();
            model.VoziloID = id;
            model.NazivVozila = db.Vozilo.Where(v => v.VoziloID == id).Select(s => s.Naziv).SingleOrDefault();

            //provjeri ima li prikolicu
            var temp = db.KompatibilnostPrikolica.Where(p => p.VoziloID == id).Select(s => s).ToList();
            if(temp == null)
            {
                model.Prikolica = false;
            }
            else
            {
                model.Prikolica = true;
                model.prikolice = new List<SelectListItem>();
                model.prikolice = temp.Select(s=> new SelectListItem
                {
                    Value = s.PrikolicaID.ToString(),
                    Text = "Tip kuke: " + s.TipKuke + " - " + db.Prikolica.Where(pri => pri.PrikolicaID == s.PrikolicaID).Select(x => "Cijena: "+ x.Cijna.ToString() + " KM - Duzina: " + x.Duzina.ToString() + " - Sirina: " + x.Sirina.ToString() ).SingleOrDefault().ToString()
                }).ToList();
            }

            //dobavi id uposlenika
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.UposlenikID = user;
            
            model.nacinPlacanja = Enum.GetValues(typeof(NacinPlacanja)).Cast<NacinPlacanja>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            return View(nameof(Rezervisi), model);
        }

        public async Task<IActionResult> RezervisiSnimi(int VoziloID, string Ime, string Prezime, string datumRodjenja,
            string Adresa, string jmbg, string Spol, int UposlenikID, string DatumPreuzimanja, string DatumPovrata,
            int NacinPlacanja, int Prikolica)
        {
            //klijent napravi i pronadji id
            string username = Ime + "." + Prezime + "@gm." + Prezime;
            await _signInManager.CreateAsync(new ApplicationUser
            {
                UserName = username,
                Email = username,
                Ime = Ime,
                Prezime = Prezime,
                Adresa = Adresa,
                Grad = db.Grad.Where(g => g.GradID == 1).SingleOrDefault(),
                JMBG = jmbg,
                DatumRodjenja = DateTime.Parse(datumRodjenja),
                DatumRegistracije = System.DateTime.Now,
                Spol = Spol,
                Slika = "nema",
            });

            var klijent = await _signInManager.FindByEmailAsync(username);

            //get poslovnicaID
            var poslovnicaID = db.UgovorZaposlenja.Where(u => u.UposlenikID == UposlenikID).Select(s => s.PoslovnicaID).SingleOrDefault();

            //napravi rezervaciju
            DateTime d1 = DateTime.Parse(DatumPreuzimanja);
            DateTime d2 = DateTime.Parse(DatumPovrata);
            int brojDana = Convert.ToInt32((d2 - d1).TotalDays);

            double cijenaAutaDan = db.Vozilo.Where(v => v.VoziloID == VoziloID).Select(s => s.Cijena).SingleOrDefault();
            string sifra = username + datumRodjenja;

            //cijena prikolice
            double cijenaPrikolice = 0;
            if(Prikolica != 0)
            {
                cijenaPrikolice = db.Prikolica.Where(p => p.PrikolicaID == Prikolica).Select(s => s.Cijna).SingleOrDefault();
            }

            Rezervacija nova = new Rezervacija
            {
                DatumRezervacije = DateTime.Now,
                DatumPreuzimanja = DateTime.Parse(DatumPreuzimanja),
                DatumPovrata = DateTime.Parse(DatumPovrata),
                NacinPlacanja = NacinPlacanja,
                SifraRezervacije = username + datumRodjenja,
                Zakljucen = 3,
                BrojDanaIznajmljivanja = brojDana,
                Cijena = brojDana * cijenaAutaDan + cijenaPrikolice,
                UspjesnoSpremljena = true,
                KlijentID = klijent.Id,
                VoziloID = VoziloID,
                PoslovnicaID = poslovnicaID,
                UposlenikID = UposlenikID
            };
            if(Prikolica != 0)
            {
                nova.PrikolicaID = Prikolica;
            }
            db.Add(nova);
            db.SaveChanges();

            int rezervacijaID = db.Rezervacija.Where(r => r.SifraRezervacije == sifra).Select(s => s.RezervacijaID).SingleOrDefault();
            string route = "/Uposlenik/Rezervacija/DodajUslugeRezervaciji?id="+rezervacijaID;

            return Redirect(route);
        }

        public IActionResult DodajUslugeRezervaciji(int id)
        {
            RezervacijaSredinaVM model = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s => 
            new RezervacijaSredinaVM
            {
                Korisnik = s.Klijent.Ime + " " + s.Klijent.Prezime,
                Vozilo = s.Vozilo.Naziv,
                DatumPreuzimanja = s.DatumPreuzimanja.ToShortDateString(),
                DatumPovrata = s.DatumPovrata.ToShortDateString(),
                Cijena = s.Cijena,
                RezervacijaID = id
            }).SingleOrDefault();
            model.dodatneUsluge = db.RezervisanaUsluga.Where(r => r.RezervacijaID == id).Select(s => new RezervacijaSredinaVM.Row
            {
                Cijena = s.DodatneUsluge.Cijena,
                Naziv = s.DodatneUsluge.Naziv,
                Opis = s.DodatneUsluge.Opis,
                Kolicina = s.Kolicina,
                Ukupno = s.UkupnaCijenaUsluge
            }).ToList();

            return View(nameof(DodajUslugeRezervaciji), model);
        }

        public IActionResult RezervacijaUsluge(int id)
        {
            var model = new RezervacijaUslugeVM();
            model.RezervacijaID = id;

            var rezervisane = db.RezervisanaUsluga.Where(p => p.RezervacijaID == id).Select(v => v.DodatneUsluge);
            var usluge = db.DodatneUsluge.Select(x => x);
            var rez = usluge.Where(p1 => rezervisane.All(p2 => p2.DodatneUslugeID != p1.DodatneUslugeID)).Select(p3 => p3);

            model.usluge = rez.Select(s => new SelectListItem
            {
                Value = s.DodatneUslugeID.ToString(),
                Text = s.Naziv + " - " + s.Cijena.ToString()
            }).ToList();

            return PartialView(nameof(RezervacijaUsluge), model);
        }
        public IActionResult RezervacijaUslugeSnimi(int RezervacijaID, int Kolicina, int UslugaID)
        {
            double cijena = db.DodatneUsluge.Where(d => d.DodatneUslugeID == UslugaID).Select(s => s.Cijena).SingleOrDefault();
            double ukupno = Kolicina * cijena;
            RezervisanaUsluga nova = new RezervisanaUsluga
            {
                RezervacijaID = RezervacijaID,
                DodatneUslugeID = UslugaID,
                Kolicina = Kolicina,
                Opis = "Opis",
                UkupnaCijenaUsluge = ukupno
            };
            db.Add(nova);
            db.SaveChanges();
            var temp = db.Rezervacija.Where(r => r.RezervacijaID == RezervacijaID).Select(s => s).SingleOrDefault();
            temp.Cijena += ukupno;
            db.SaveChanges();
            string route = "/Uposlenik/Rezervacija/DodajUslugeRezervaciji?id=" + RezervacijaID.ToString();
            return Redirect(route);
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
                Ime = x.Klijent.Ime,
                Prezime = x.Klijent.Prezime,
                DatumRodjenja = x.Klijent.DatumRodjenja.ToShortDateString(),
                Adresa = x.Klijent.Adresa,
                Spol = x.Klijent.Spol,
                jmbg = x.Klijent.JMBG,
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

            var temp = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s => s.Prikolica).SingleOrDefault();
            if (temp != null)
            {
                model.Prikolica = true;
                model.CijenaPrikolice = temp.Cijna;
            }
            else
            {
                model.Prikolica = false;
                model.CijenaPrikolice = 0;
            }
            return View(nameof(Detalji), model);
        }


    

    }



}