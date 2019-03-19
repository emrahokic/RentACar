using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Data;
using RentACar.Models;
using RentACar.Areas.Uposlenik.ViewModels;
using RentACar.Helper;

namespace RentACar.Areas.Uposlenik.Controllers
{
    [Authorize(Roles = "Administrator,Uposlenik")]
    [Area("Uposlenik")]
    public class PrikolicaController : Controller
    {
        private ApplicationDbContext _context;
        public PrikolicaController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            var model = new PrikolicaVM
            {
                rows = _context.Prikolica.Select(k => new PrikolicaVM.Row
                {
                    PrikolicaID = k.PrikolicaID,
                    Sirina = k.Sirina,
                    Zapremina = k.Zapremina,
                    Duzina = k.Duzina,
                    TipPrikolice = k.TipPrikolice,
                    Cijena = k.Cijna
                }).ToList()
            };
            return View(nameof(Index), model);
        }

        public IActionResult Uredi(int id)
        {
            PrikolicaUrediVM model = new PrikolicaUrediVM();
            model = _context.Prikolica.Where(p => p.PrikolicaID == id).Select(x => new PrikolicaUrediVM
            {
                PrikolicaID = x.PrikolicaID,
                Duzina = x.Duzina,
                Sirina = x.Sirina,
                Zapremina = x.Zapremina,
                Cijena = x.Cijna,
                TrenutnoTipPrikolice = x.TipPrikolice,
                TipPrikolice = Enum.GetValues(typeof(TipPrikolice)).Cast<TipPrikolice>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList()
            }).SingleOrDefault();

            if (model == null)
            {
                return Content("Prikolica ne postoji");
            }

            return View(nameof(Uredi), model);
        }
        
        public IActionResult UrediSnimi(int PrikolicaID, double Sirina, double Zapremina, double Duzina, double Cijena, int TipPrikolice)
        {
            Prikolica x = _context.Prikolica.Find(PrikolicaID);

            x.PrikolicaID = PrikolicaID;
            x.Sirina = Sirina;
            x.Zapremina = Zapremina;
            x.Duzina = Duzina;
            x.Cijna = Cijena;
            x.TipPrikolice = TipPrikolice;

            _context.SaveChanges();
            _context.Dispose();

            return Redirect(nameof(Index));
        }

        public IActionResult Obrisi(int id)
        {
            Prikolica temp = new Prikolica();
            temp = _context.Prikolica.Find(id);
            if (temp == null)
            {
                return Content("Usluga ne postoji");
            }
            _context.Remove(temp);

            _context.SaveChanges();
            _context.Dispose();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Dodaj()
        {
            PrikolicaDodajVM model = new PrikolicaDodajVM();
            model.tipPrikolice = Enum.GetValues(typeof(TipPrikolice)).Cast<TipPrikolice>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return View(nameof(Dodaj), model);
        }

        public IActionResult DodajSnimi(double Sirina, double Zapremina, double Duzina, double Cijena, int TipPrikolice)
        {
            Prikolica nova = new Prikolica()
            {
                Sirina = Sirina,
                Zapremina = Zapremina,
                Duzina = Duzina,
                Cijna = Cijena,
                TipPrikolice = TipPrikolice
            };
            _context.Add(nova);
            _context.SaveChanges();
            _context.Dispose();

            return Redirect(nameof(Index));
        }

        public IActionResult Detalji(int id)
        {
            Prikolica temp = _context.Prikolica.Find(id);
            PrikolicaDetaljiVM model = new PrikolicaDetaljiVM{

                PrikolicaID = temp.PrikolicaID,
                Sirina = temp.Sirina,
                Zapremina = temp.Zapremina,
                TipPrikolice = temp.TipPrikolice,
                Cijena = temp.Cijna,
                Duzina = temp.Duzina,
                rows = _context.KompatibilnostPrikolica.Where(x=> x.Prikolica.PrikolicaID == id).Select(p=> new PrikolicaDetaljiVM.Row
                {
                    TipKuke = p.TipKuke,
                    Tezina = p.Tezina.ToString(),
                    Auto = p.Vozilo.Brend.Naziv + " " + p.Vozilo.Model,
                    tipAuta = p.Vozilo.GrupniTipVozila
                }).ToList()
            };

            return View(nameof(Detalji), model);
        }

        public IActionResult DodajVozilo(int id)
        {
            var vozPrikolica = _context.KompatibilnostPrikolica.Where(p => p.PrikolicaID == id).Select(v => v.Vozilo);
            var voz = _context.Vozilo.Select(x => x);
            var rez = voz.Where(p1 => vozPrikolica.All(p2 => p2.VoziloID != p1.VoziloID)).Select(p3 => p3);
            var ulazniModel = new DodajVoziloPrikolicaVM();
            ulazniModel.vozila = rez.Select(x => new SelectListItem
            {
                Value = x.VoziloID.ToString(),
                Text = x.Brend.Naziv + " - " + x.Naziv + " - " + x.TipVozila
            }
            ).ToList();

            ulazniModel.PrikolicaID = id;

            return PartialView(nameof(DodajVozilo), ulazniModel);
        }
        public IActionResult DodajVoziloSnimi(int PrikolicaID, string tipKuke, double tezina, int VoziloID)
        {
            Vozilo temp = _context.Vozilo.Find(VoziloID);
            Prikolica temp2 = _context.Prikolica.Find(PrikolicaID);
            KompatibilnostPrikolica model = new KompatibilnostPrikolica();
            model.Vozilo = temp;
            model.TipKuke = tipKuke;
            model.Tezina = tezina.ToString();
            model.Prikolica = temp2;

            _context.Add(model);
            _context.SaveChanges();
            _context.Dispose();
            string route = "/uposlenik/Prikolica/Detalji/" + PrikolicaID.ToString();
            return Redirect(route);
        }
    }
}