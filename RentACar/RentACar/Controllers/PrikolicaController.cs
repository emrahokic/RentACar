using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Data;
using RentACar.Models;
using RentACar.ViewModels;

namespace RentACar.Controllers
{
    /****************************************
     *
     * 
     * 
     *       Trebas dodati koje su role za Authorize
     *      [Authorize(Roles = "Administrator,Uposlenik")]
     *       
     * 
     * 
     ***************************************/
    [Authorize(Roles = "Administrator,Uposlenik")]
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
                    TipPrikolice = k.TipPrikolice
                }).ToList()
            };
            return View(nameof(Index), model);
        }

        public IActionResult Uredi(int id)
        {
            Prikolica model = new Prikolica();
            model = _context.Prikolica.Find(id);
            if (model == null)
            {
                return Content("Prikolica ne postoji");
            }

            return View(nameof(Uredi), model);
        }
        
        public IActionResult UrediSnimi(int PrikolicaID, double Sirina, double Zapremina, double Duzina, string TipPrikolice)
        {
            Prikolica x = _context.Prikolica.Find(PrikolicaID);

            x.PrikolicaID = PrikolicaID;
            x.Sirina = Sirina;
            x.Zapremina = Zapremina;
            x.Duzina = Duzina;
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
            return View(nameof(Dodaj));
        }

        public IActionResult DodajSnimi(double Sirina, double Zapremina, double Duzina, string TipPrikolice)
        {
            Prikolica nova = new Prikolica()
            {
                Sirina = Sirina,
                Zapremina = Zapremina,
                Duzina = Duzina,
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
            var ulazniModel = new DodajVoziloPrikolicaVM();
            ulazniModel.vozila = _context.Vozilo.Select(x => new SelectListItem
            {
                Value = x.VoziloID.ToString(),
                Text = x.Brend.Naziv + " " + x.Model + " " + x.TipVozila
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
            string route = "/Prikolica/Detalji/" + PrikolicaID.ToString();
            return Redirect(route);//Redirect(nameof(Index));
        }
    }
}