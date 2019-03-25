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
using System.Text.RegularExpressions;

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
        public async Task<IActionResult> Index()
        {
            int ID = int.Parse(_signInManager.GetUserId(User));
            int PoslovnicaID = db.UgovorZaposlenja.FirstOrDefault(g => g.UposlenikID == ID).PoslovnicaID;
            RezervacijaIndexVM model = new RezervacijaIndexVM()
            {
                Rows = db.Rezervacija
                .Where(z => z.PoslovnicaID == PoslovnicaID && (z.UposlenikID == ID || z.UposlenikID == null) && z.UspjesnoSpremljena == true)
                .OrderByDescending(o => o.DatumRezervacije)
                .Select(x => new RezervacijaIndexVM.Row()
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
        /* -------------------------------------------->  Prikaz rezervacije detaljno*/
        public IActionResult Detalji(int id)
        {
            RezervacijaDetaljnoVM model = db.Rezervacija.Where(z => z.RezervacijaID == id).Select(x => new RezervacijaDetaljnoVM
            {
                RezervacijaID = x.RezervacijaID,
                Grad = x.Poslovnica.Grad.Naziv,
                DatumRezervacije = x.DatumRezervacije,
                SlikaVozila = db.Slika.Where(sl => sl.VoziloID == x.VoziloID && sl.Pozicija == 1).Select(c => new Slika
                {
                    Name = c.Name,
                    SlikaID = c.SlikaID,
                    URL = c.URL,
                    Pozicija = c.Pozicija
                }).FirstOrDefault(),
                DatumPreuzimanja = x.DatumPreuzimanja,
                DatumPovrata = x.DatumPovrata,
                Zakljucen = x.Zakljucen,
                Ukupno = x.Cijena,
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
                dodatneUsluge = db.RezervisanaUsluga.Where(u => u.RezervacijaID == x.RezervacijaID).Select(ru => new RezervacijaDetaljnoVM.Row
                {
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

            model.Cijena = model.Ukupno - model.UkupnaCijenaUsluga - model.CijenaPrikolice;
            return View(nameof(Detalji), model);
        }

        /* -------------------------------------------->  Interakcija sa rezervacijama*/
        public IActionResult Prihvati(int id)
        {
            int rezervacijaID = id;
            int uposlenikID = int.Parse(_signInManager.GetUserId(User)); 
            var rezervacija = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s => s).SingleOrDefault();

            rezervacija.UposlenikID = uposlenikID;
            rezervacija.Zakljucen = (int) InfoRezervacija.Odobrena;

            //postavi vozilo kao nedostupno
            var trPoslovnica = db.TrenutnaPoslovnica.Where(v => v.VoziloID == rezervacija.VoziloID).Select(s => s).SingleOrDefault();
            trPoslovnica.VoziloRezervisano = true;
            trPoslovnica.DatumIzlaza = rezervacija.DatumPreuzimanja;

            //napravi notifikaciju klijentu
            string vozilo = db.Vozilo.Where(v => v.VoziloID == rezervacija.VoziloID).Select(n => n.Naziv).SingleOrDefault();
            string poruka = "Vasa rezervacija za vozilo " + vozilo + " je uspjesno prihvacena. Ugodan dan.";
            NapraviNotifikaciju(id, rezervacija.KlijentID, poruka);

            db.SaveChanges();

            return RedirectToAction(nameof(Index), new { id = rezervacijaID });
        }

        public IActionResult ZakljuciRezervaciju(int id)
        {
            int rezervacijaID = id;
            var rezervacija = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s => s).SingleOrDefault();
            rezervacija.Zakljucen = (int)InfoRezervacija.Zavrsena;

            var voziloPoslovnica = db.TrenutnaPoslovnica.Where(p => p.VoziloID == rezervacija.VoziloID).Select(po => po).SingleOrDefault();
            voziloPoslovnica.DatumIzlaza = null;
            voziloPoslovnica.VoziloRezervisano = false;

            var sifra = rezervacija.SifraRezervacije;
            Regex regex = new Regex(@"#pos");
            Match match = regex.Match(sifra);
            if (match.Success)
            {
                //posalji mail
            }
            else
            {
                string vozilo = db.Vozilo.Where(v => v.VoziloID == rezervacija.VoziloID).Select(n => n.Naziv).SingleOrDefault();
                string poruka = "Vasa rezervacija za vozilo " + vozilo + " je uspjesno zavrsena. Hvala na povjerenju :)";
                NapraviNotifikaciju(id, rezervacija.KlijentID, poruka);
            }

            db.SaveChanges();
            db.Dispose();

            return RedirectToAction(nameof(Index), new { id = rezervacijaID });
        }

        public IActionResult Odbij(int id)
        {
            int rezervacijaID = id;
            var rezervacija = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s => s).SingleOrDefault();
            rezervacija.Zakljucen = (int)InfoRezervacija.Odbijena;

            //napravi novu notifikaciju
            string vozilo = db.Vozilo.Where(v => v.VoziloID == rezervacija.VoziloID).Select(n => n.Naziv).SingleOrDefault();
            string poruka = "Vasa rezervacija za vozilo " + vozilo + " je odbijena!";
            NapraviNotifikaciju(id, rezervacija.KlijentID, poruka);
            
            db.SaveChanges();

            return RedirectToAction(nameof(Index), new { id = rezervacijaID });
        }
        /* -------------------------------------------->  Prikaz vozila za rezervaciju u karticama*/
        public async Task<IActionResult> Dodaj()
        {
            int userID = int.Parse(_signInManager.GetUserId(User));
            int poslovnicaID = db.UgovorZaposlenja.Where(u => u.UposlenikID == userID).Select(s => s.PoslovnicaID).SingleOrDefault();

            var model = new VoziloRezervacijaVM
            {
                vozila = db.TrenutnaPoslovnica.Where(t => t.PoslovnicaID == poslovnicaID && t.VoziloRezervisano == false).Select(v => new VoziloRezervacijaVM.Row
                {
                    Boja =v.Vozilo.Boja,
                    Brend = v.Vozilo.Brend.Naziv,
                    Naziv = v.Vozilo.Naziv,
                    BrojMjesta = v.Vozilo.BrojMjesta,
                    BrojVrata = v.Vozilo.BrojVrata,
                    Domet = v.Vozilo.Domet,
                    VoziloID = v.Vozilo.VoziloID,
                    GodinaProizvodnje = v.Vozilo.GodinaProizvodnje,
                    Gorivo = v.Vozilo.Gorivo,
                    GrupniTipVozila = v.Vozilo.GrupniTipVozila,
                    Klima = v.Vozilo.Klima,
                    Model = v.Vozilo.Model,
                    slike =  db.Slika.Where(s => s.Pozicija == 1 && s.VoziloID == v.VoziloID).Select(p => p.URL).FirstOrDefault(),
                    SnagaMotora = v.Vozilo.SnagaMotora,
                    TipVozila = v.Vozilo.TipVozila,
                    Transmisija = v.Vozilo.Transmisija,
                    Cijena = v.Vozilo.Cijena
                }).ToList()
            };
            return View(model);
        }
        /* -------------------------------------------->  Rezervisanje vozila*/
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

        public async Task<IActionResult> RezervisiSnimi(int VoziloID, string Ime, string Prezime, string Mail, string datumRodjenja,
            string Adresa, string jmbg, string Spol, int UposlenikID, string DatumPreuzimanja, string DatumPovrata,
            int NacinPlacanja, int Prikolica)
        {
            //klijent napravi i pronadji id
            string username = Mail;
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
                Klijent = klijent,
                VoziloID = VoziloID,
                Vozilo = db.Vozilo.Where(v => v.VoziloID == VoziloID).Select(s => s).SingleOrDefault(),
                PoslovnicaID = poslovnicaID,
                Poslovnica = db.Poslovnica.Where(p => p.PoslovnicaID == poslovnicaID).Select(s => s).SingleOrDefault(),
                UposlenikID = UposlenikID
            };
            if(Prikolica != 0)
            {
                nova.PrikolicaID = Prikolica;
            }
            db.Add(nova);
            db.SaveChanges();

            var rezervacija = db.Rezervacija.Where(r => r.SifraRezervacije == sifra).Select(s => s).SingleOrDefault();
            int rezervacijaID = rezervacija.RezervacijaID;
            int pr = rezervacija.PrikolicaID ?? 0;
            rezervacija.SifraRezervacije = DateTime.Now.Day + "" + DateTime.Now.Month + "-" + rezervacija.PoslovnicaID + "" + rezervacija.VoziloID + "" + rezervacija.RezervacijaID + "" + pr + "/" + DateTime.Now.Year + "#pos";

            db.SaveChanges();
            db.Dispose();
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
            UslugaSnimi(RezervacijaID, Kolicina, UslugaID);
            string route = "/Uposlenik/Rezervacija/DodajUslugeRezervaciji?id=" + RezervacijaID.ToString();
            return Redirect(route);
        }

        /* -------------------------------------------->  Uredjivanje postojece rezervacije*/
        public IActionResult RezervacijaUslugeUredi(int id)
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


            return PartialView(nameof(RezervacijaUslugeUredi), model);
        }
       
        public IActionResult RezervacijaUslugeUrediSnimi(int RezervacijaID, int Kolicina, int UslugaID)
        {
            UslugaSnimi(RezervacijaID, Kolicina, UslugaID);
            string route = "/Uposlenik/Rezervacija/DodatneUslugeUrediREZ?id=" + RezervacijaID.ToString();
            return Redirect(route);
        }

        
     

        public IActionResult RezervacijaUredi(int id)
        {
            var model = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s => new RezervacijaUrediVM
            {
                RezervacijaID = id,
                DatumPreuzimanja = s.DatumPreuzimanja,
                DatumRezervacije = s.DatumRezervacije,
                DatumPovrata = s.DatumPovrata,
                Zakljucen = s.Zakljucen,
                Cijena = s.Cijena,
                BrojDanaIznajmljivanja = s.BrojDanaIznajmljivanja,
                NacinPlacanja = s.NacinPlacanja,
                SlikaVozila = db.Slika.Where(x => s.VoziloID == x.VoziloID && x.Pozicija == 1).Select(z => z.URL).SingleOrDefault(),
                Vozilo = s.Vozilo.Naziv,
                Ime = s.Klijent.Ime,
                Prezime = s.Klijent.Prezime,
                Email = db.Users.Where(us => us.Id == s.KlijentID).Select(ma => ma.Email).SingleOrDefault(),
                DatumRodjenja = s.Klijent.DatumRodjenja,
                Adresa = s.Klijent.Adresa,
                Spol = s.Klijent.Spol,
                jmbg = s.Klijent.JMBG,
                PrikolicaID = s.PrikolicaID
            }).SingleOrDefault();

            model.zakljuceni = Enum.GetValues(typeof(InfoRezervacija)).Cast<InfoRezervacija>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            model.naciniPlacanja = Enum.GetValues(typeof(NacinPlacanja)).Cast<NacinPlacanja>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            int voziloID = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s => s.VoziloID).SingleOrDefault();
            var temp = db.KompatibilnostPrikolica.Where(p => p.VoziloID == voziloID).Select(s => s).ToList();
            if (temp.Count() != 0)
            {
                model.imaPrikolicu = true;
                model.prikolice = new List<SelectListItem>();
                model.prikolice = temp.Select(s => new SelectListItem
                {
                    Value = s.PrikolicaID.ToString(),
                    Text = "Tip kuke: " + s.TipKuke + " - " + db.Prikolica.Where(pri => pri.PrikolicaID == s.PrikolicaID).Select(x => "Cijena: " + x.Cijna.ToString() + " KM - Duzina: " + x.Duzina.ToString() + " - Sirina: " + x.Sirina.ToString()).SingleOrDefault().ToString()
                }).ToList();
            }
            else
            {
                model.imaPrikolicu = false;
            }

            return View(nameof(RezervacijaUredi), model);
        }

        public IActionResult RezervacijaUrediSnimi(int Prikolica,int RezervacijaID, string DatumPreuzimanja, string DatumPovrata, int NacinPlacanja, int Zakljucen, 
            string Ime, string Prezime, string Mail, string DatumRodjenja, string Adresa, string jmbg, string Spol)
        {
            //pronadji rezervaciju i smjesti je u temp
            var temp = db.Rezervacija.Where(r => r.RezervacijaID == RezervacijaID).Select(s => s).SingleOrDefault();

            double cijenaVozilaDan = db.Vozilo.Where(v => v.VoziloID == temp.VoziloID).Select(s => s.Cijena).SingleOrDefault();
            double ostatak = temp.Cijena - (temp.BrojDanaIznajmljivanja * cijenaVozilaDan);

            temp.DatumPreuzimanja = DateTime.Parse(DatumPreuzimanja);
            temp.DatumPovrata = DateTime.Parse(DatumPovrata);
            temp.NacinPlacanja = NacinPlacanja;
            temp.Zakljucen = Zakljucen;

            var klijent = db.Users.Where(u => u.Id == temp.KlijentID).Select(s => s).SingleOrDefault();
            klijent.Ime = Ime;
            klijent.Prezime = Prezime;
            klijent.Email = Mail;
            klijent.NormalizedEmail = Mail.ToUpper();
            klijent.DatumRodjenja = DateTime.Parse(DatumRodjenja);
            klijent.Adresa = Adresa;
            klijent.JMBG = jmbg;
            klijent.Spol = Spol;

            int brojDana = Convert.ToInt32((DateTime.Parse(DatumPovrata) - DateTime.Parse(DatumPreuzimanja)).TotalDays);
            temp.BrojDanaIznajmljivanja = brojDana;
            temp.Cijena = (cijenaVozilaDan * brojDana) + ostatak;

            //update prikolice
            if (Prikolica != 0 && Prikolica != -1)
            {
                if (temp.PrikolicaID == 0)
                {
                    temp.PrikolicaID = Prikolica;
                }
                else
                {
                    var cijenaPrikoliceOld = db.Prikolica.Where(prik => prik.PrikolicaID == temp.PrikolicaID).Select(pr => pr.Cijna).SingleOrDefault();
                    temp.Cijena -= cijenaPrikoliceOld;
                    var cijenaPrikoliceNew = db.Prikolica.Where(prik => prik.PrikolicaID == Prikolica).Select(pr => pr.Cijna).SingleOrDefault();
                    temp.Cijena += cijenaPrikoliceNew;
                    temp.PrikolicaID = Prikolica;
                }
            }
            else if (Prikolica == -1)
            {
                if (temp.PrikolicaID != null)
                {
                    double c = db.Prikolica.Where(pri => pri.PrikolicaID == temp.PrikolicaID).Select(cij => cij.Cijna).FirstOrDefault();
                    temp.Cijena -= c;
                }
                temp.PrikolicaID = null;
            }
            db.SaveChanges();
            db.Dispose();

            return RedirectToAction(nameof(DodatneUslugeUrediREZ), new { id = RezervacijaID });
        }

        public IActionResult DodatneUslugeUrediREZ(int id)
        {
            RezervacijaUslugeUrediVM model = new RezervacijaUslugeUrediVM
            {
                RezervacijaID = id,
                rows = db.RezervisanaUsluga.Where(u => u.RezervacijaID == id).Select(s =>
                new RezervacijaUslugeUrediVM.Row
                {
                    UslugaID = s.DodatneUslugeID,
                    Kolicina = s.Kolicina,
                    Ukupno = s.UkupnaCijenaUsluge,
                    Naziv = db.DodatneUsluge.Where(d => d.DodatneUslugeID == s.DodatneUslugeID).Select(x => x.Naziv).SingleOrDefault(),
                    Cijena = db.DodatneUsluge.Where(d => d.DodatneUslugeID == s.DodatneUslugeID).Select(x => x.Cijena).SingleOrDefault(),
                    Opis = db.DodatneUsluge.Where(d => d.DodatneUslugeID == s.DodatneUslugeID).Select(x => x.Opis).SingleOrDefault()
                }).ToList()
            };

            return View(nameof(DodatneUslugeUrediREZ), model);
        }
        public IActionResult DodatneUslugeUrediREZobrisi(int uslugaID, int rezervacijaID)
        {
            var temp = db.RezervisanaUsluga.Where(u => u.RezervacijaID == rezervacijaID && u.DodatneUslugeID == uslugaID).Select(s => s).SingleOrDefault();
            
            //umanji cijenu rezervacije za obrisanu uslugu
            var rezervacija = db.Rezervacija.Where(r => r.RezervacijaID == rezervacijaID).Select(s => s).SingleOrDefault();
            double ukupno = temp.UkupnaCijenaUsluge;
            rezervacija.Cijena -= ukupno;

            db.Remove(temp);
            db.SaveChanges();
            db.Dispose();
            
            return RedirectToAction(nameof(DodatneUslugeUrediREZ), new { id = rezervacijaID });
        }

        /* -------------------------------------------->  Brisanje rezervacije*/
        public IActionResult Obrisi(int id)
        {
            var temp = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s => s).SingleOrDefault();
            if(temp == null)
            {
                return Content("Nema te rezervacije!");
            }

            var klijent = db.Users.Where(u => u.Id == temp.KlijentID).Select(us => us).SingleOrDefault();
            if(klijent == null)
            {
                return Content("Klijent ne postoji u bazi!");
            }

            //provjerava ukoliko je rezervacija zavrsena, onda je ne moze obrisati
            if(temp.Zakljucen != (int)InfoRezervacija.Zavrsena)
            {
                db.Remove(temp);
            }

            //brisanje notifikacija
            var notifikacije = db.Notifikacija.Where(n => n.RezervacijaID == id).Select(ntf => ntf).ToList();
            if(notifikacije != null)
            {
                db.RemoveRange(notifikacije);
            }

            //brise informacije o klijentu ukoliko su podaci uneseni od strane uposlenika
            var sifra = temp.SifraRezervacije;
            Regex regex = new Regex(@"#pos");
            Match match = regex.Match(sifra);
            if (match.Success)
            {
                db.Remove(klijent);
            }

            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        /* -------------------------------------------->  Zajednicke funkcije*/
        private void UslugaSnimi(int RezervacijaID, int Kolicina, int UslugaID)
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
        }

        private void NapraviNotifikaciju(int rezervacijaID, int klijentID, string poruka)
        {
            Notifikacija nova = new Notifikacija
            {
                Otvorena = false,
                Poruka = poruka,
                RezervacijaID = rezervacijaID,
                UserID = klijentID,
                Vrijeme = DateTime.Now
            };
            db.Add(nova);
            db.SaveChanges();
            db.Dispose();
        }
    }
}