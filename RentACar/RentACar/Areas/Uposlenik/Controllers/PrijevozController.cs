using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Areas.Uposlenik.ViewModels;
using RentACar.Data;
using RentACar.Helper;
using RentACar.Models;

namespace RentACar.Areas.Uposlenik.Controllers
{
    [Authorize(Roles = "Uposlenik")]
    [Area("Uposlenik")]
    public class PrijevozController : Controller
    {
        private UserManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _context;

        public PrijevozController(ApplicationDbContext context, UserManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var model = new PrijevoziVM()
            {
                prijevozi = _context.Prijevoz.Select(s => new PrijevoziVM.Row
                {
                    PrijevozID = s.PrijevozID,
                    DatumPrijevoza = s.DatumPrijevoza.ToShortDateString(),
                    Klijent = s.Klijent.Ime + " " + s.Klijent.Prezime,
                    TipPrijevoza = Enum.GetName(typeof(TipPrijevoza), s.TipPrijevoza),
                    Zavrsna = _context.PrijevozVozilo.Where(z => z.PrijevozID == s.PrijevozID).Select(k => k.ZavrsnaKilometraza).SingleOrDefault()
                }).ToList()
            };
            return View(nameof(Index), model);
        }

        public IActionResult Obrisi(int id)
        {
            var temp = _context.Prijevoz.Where(p => p.PrijevozID == id).Select(s => s).SingleOrDefault();
            if(temp == null)
            {
                return Content("Prijevoz ne postoji");
            }
            _context.Remove(temp);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detalji(int id)
        {
            var model = _context.Prijevoz.Where(p => p.PrijevozID == id).Select(s => new PrijevozDodajVoziloVM
            {
                PrijevozID = id,
                klijent = s.Klijent.Ime + " " + s.Klijent.Prezime,
                DatumPrijevoza = s.DatumPrijevoza.ToShortDateString(),
                jmbg = s.Klijent.JMBG,
                NacinPlacanja = s.NacinPlacanja,
                TipPrijevoza = s.TipPrijevoza,
                CijenaKilometar = s.CijenaPoKilometru,
                CijenaSat = s.CijenaCekanjaPoSatu,
                CijenaVozac = s.CijenaPoVozacu
            }).SingleOrDefault();

            model.vozaci = _context.UposlenikPrijevoz.Where(up => up.PrijevozID == id).Select(vozac => new PrijevozDodajVoziloVM.Row2
            {
                Ime = vozac.Uposlenik.Ime,
                Prezime = vozac.Uposlenik.Prezime,
                Mail = vozac.Uposlenik.Email,
                VozacID = vozac.UposlenikID
            }).ToList();

            model.vozila = _context.PrijevozVozilo.Where(pv => pv.PrijevozID == id).Select(vozilo => new PrijevozDodajVoziloVM.Row1
            {
                Naziv = vozilo.Vozilo.Naziv,
                GodinaProizvodnje = vozilo.Vozilo.GodinaProizvodnje.ToString(),
                slika = _context.Slika.Where(sl => sl.VoziloID == vozilo.VoziloID && sl.Pozicija == 1).Select(s => s.URL).SingleOrDefault(),
                VoziloID = vozilo.VoziloID
            }).ToList();

            return View(nameof(Detalji), model);
        }

        public IActionResult NoviPrijevoz()
        {
            var model = new PrijevozNoviVM();

            model.nacinPlacanja = Enum.GetValues(typeof(NacinPlacanja)).Cast<NacinPlacanja>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            model.tipPrijevoza = Enum.GetValues(typeof(TipPrijevoza)).Cast<TipPrijevoza>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            return View(nameof(NoviPrijevoz), model);
        }
        public async Task<IActionResult> NoviPrijevozSnimiAsync(PrijevozNoviVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(NoviPrijevoz), model);
            }
            //kreiranje klijenta
            string username = model.Email;
            await _signInManager.CreateAsync(new ApplicationUser
            {
                UserName = username,
                Email = username,
                Ime = model.Ime,
                Prezime = model.Prezime,
                Adresa = model.Adresa,
                Grad = _context.Grad.Where(g => g.GradID == 1).SingleOrDefault(),
                JMBG = model.jmbg,
                DatumRodjenja = DateTime.Parse(model.DatumRodjenja),
                DatumRegistracije = System.DateTime.Now,
                Spol = model.Spol,
                Slika = "nema",
            });
            var klijent = await _signInManager.FindByEmailAsync(username);

            //pravljenje novog prijevoza
            Prijevoz novi = new Prijevoz();
            novi.DatumRezervacije = DateTime.Now;
            novi.DatumPrijevoza = DateTime.Parse(model.DatumPrijevoza);
            novi.NacinPlacanja = model.NacinPlacanja;
            novi.TipPrijevoza = model.TipPrijevoza;
            novi.Klijent = klijent;

            //na osnovu tipa prijevoza se generise cijene
            if(model.TipPrijevoza == 1)
            {
                novi.CijenaCekanjaPoSatu = 10;
                novi.CijenaPoKilometru = 2;
                novi.CijenaPoVozacu = 50;
            }
            else if(model.TipPrijevoza == 2)
            {
                novi.CijenaCekanjaPoSatu = 8;
                novi.CijenaPoKilometru = 1;
                novi.CijenaPoVozacu = 30;
            }
            else
            {
                novi.CijenaCekanjaPoSatu = 5;
                novi.CijenaPoKilometru = 0.8;
                novi.CijenaPoVozacu = 20;
            }

            //postavljanje broj vozaca i vozila na 0
            novi.BrojVozaca = 0;
            novi.BrojVozila = 0;

            _context.Add(novi);
            _context.SaveChanges();


            int pID = _context.Prijevoz.Where(p => p.Klijent.Email == klijent.Email).Select(s => s.PrijevozID).SingleOrDefault();

            return RedirectToAction(nameof(DodajVozilaPrijevoz), new { id = pID });
        }
        public IActionResult DodajVozilaPrijevoz(int id)
        {
            var model = _context.Prijevoz.Where(p => p.PrijevozID == id).Select(s => new PrijevozDodajVoziloVM
            {
                PrijevozID = id,
                klijent = s.Klijent.Ime + " " + s.Klijent.Prezime,
                DatumPrijevoza = s.DatumPrijevoza.ToShortDateString(),
                jmbg = s.Klijent.JMBG,
                NacinPlacanja = s.NacinPlacanja,
                TipPrijevoza = s.TipPrijevoza,
                CijenaKilometar = s.CijenaPoKilometru,
                CijenaSat = s.CijenaCekanjaPoSatu,
                CijenaVozac = s.CijenaPoVozacu
            }).SingleOrDefault();

            model.vozaci = _context.UposlenikPrijevoz.Where(up => up.PrijevozID == id).Select(vozac => new PrijevozDodajVoziloVM.Row2
            {
                Ime = vozac.Uposlenik.Ime,
                Prezime = vozac.Uposlenik.Prezime,
                Mail = vozac.Uposlenik.Email,
                VozacID = vozac.UposlenikID
            }).ToList();

            model.vozila = _context.PrijevozVozilo.Where(pv => pv.PrijevozID == id).Select(vozilo => new PrijevozDodajVoziloVM.Row1
            {
                Naziv = vozilo.Vozilo.Naziv,
                GodinaProizvodnje = vozilo.Vozilo.GodinaProizvodnje.ToString(),
                slika = _context.Slika.Where(sl => sl.VoziloID == vozilo.VoziloID && sl.Pozicija == 1).Select(s => s.URL).SingleOrDefault(),
                VoziloID = vozilo.VoziloID
            }).ToList();

            return View(nameof(DodajVozilaPrijevoz), model);
        }
        
        public IActionResult DodajVozilo(int PrijevozID)
        {
            var model = new DodajVoziloPrijevozVM();

            int ID = int.Parse(_signInManager.GetUserId(User));
            int PoslovnicaID = _context.UgovorZaposlenja.FirstOrDefault(g => g.UposlenikID == ID).PoslovnicaID;

            //vozila koja imaju cijenu 0 su rezervisanja za prijevoz
            model.vozila = _context.TrenutnaPoslovnica.Where(v => v.PoslovnicaID == PoslovnicaID && v.Vozilo.Cijena == 0).Select(s => new SelectListItem
            {
                Value = s.VoziloID.ToString(),
                Text = s.Vozilo.Naziv
            }).ToList();
            model.PrijevozID = PrijevozID;

            return PartialView(nameof(DodajVozilo), model);
        }

        public IActionResult DodajVoziloSnimi(int PrijevozID, int VoziloID)
        {
            //dodavanje vozila prijevozu
            var novi = new PrijevozVozilo();
            novi.PrijevozID = PrijevozID;
            novi.VoziloID = VoziloID;
            novi.PocetnaKilometraza = _context.Vozilo.Where(v => v.VoziloID == VoziloID).Select(s => s.Kilometraza).SingleOrDefault();
            novi.ZavrsnaKilometraza = 0;

            //uvecaj broj vozila u prijevozu
            var temp = _context.Prijevoz.Where(p => p.PrijevozID == PrijevozID).Select(pr => pr).SingleOrDefault();
            temp.BrojVozila += 1;

            _context.Add(novi);
            _context.SaveChanges();

            return RedirectToAction(nameof(DodajVozilaPrijevoz), new { id = PrijevozID });
        }

        public IActionResult DodajVozaca(int PrijevozID)
        {
            //dohvatanje poslovnice
            int ID = int.Parse(_signInManager.GetUserId(User));
            int PoslovnicaID = _context.UgovorZaposlenja.FirstOrDefault(g => g.UposlenikID == ID).PoslovnicaID;

            var model = new PrijevozDodajVozacaVM();
            model.PrijevozID = PrijevozID;

            model.vozaci = _context.UgovorZaposlenja.Where(uz => uz.PoslovnicaID == PoslovnicaID).Select(s => new SelectListItem
            {
                Value = s.UposlenikID.ToString(),
                Text = s.Uposlenik.Ime + " " + s.Uposlenik.Prezime + " " + s.Uposlenik.Email
            }).ToList();

            return PartialView(nameof(DodajVozaca), model);
        }

        public IActionResult DodajVozacSnimi(int PrijevozID, int UposlenikID)
        {
            //novi vozac za prijevoz
            var novi = new UposlenikPrijevoz();
            novi.PrijevozID = PrijevozID;
            novi.UposlenikID = UposlenikID;

            _context.Add(novi);
            _context.SaveChanges();

            //povecavanja broja vozaca u prijevozu
            var temp = _context.Prijevoz.Where(p => p.PrijevozID == PrijevozID).Select(pr => pr).SingleOrDefault();
            temp.BrojVozaca += 1;
            _context.SaveChanges();

            return RedirectToAction(nameof(DodajVozilaPrijevoz), new { id = PrijevozID });
        }
    }
}