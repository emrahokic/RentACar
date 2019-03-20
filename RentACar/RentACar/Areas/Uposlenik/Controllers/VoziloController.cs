using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using RentACar.Areas.Uposlenik.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace RentACar.Areas.Uposlenik.Controllers
{
    [Authorize(Roles = "Administrator,Uposlenik")]
    [Area("Uposlenik")]
    public class VoziloController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _signInManager;

        public VoziloController(ApplicationDbContext context, UserManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            int userID = int.Parse(_signInManager.GetUserId(User));
            int poslovnicaID = _context.UgovorZaposlenja.Where(u => u.UposlenikID == userID).Select(s => s.PoslovnicaID).SingleOrDefault();

            var model = new VoziloVM
            {
                rows = _context.TrenutnaPoslovnica.Where(t => t.PoslovnicaID == poslovnicaID).Select(v => new VoziloVM.Row
                {
                    VoziloID = v.VoziloID,
                    Brend = v.Vozilo.Brend.Naziv,
                    Naziv = v.Vozilo.Naziv,
                    Model = v.Vozilo.Model,
                    BrojVrata = v.Vozilo.BrojVrata,
                    TipVozila = v.Vozilo.TipVozila,
                    Transmisija = v.Vozilo.Transmisija
                }).ToList()
            };

            return View(nameof(Index), model);
        }



       
        //ostavi i ovaj
        public IActionResult Detalji(int id)
        {
            VoziloDetaljnoVM model = _context.Vozilo.Where(z => z.VoziloID == id).Select(x => new VoziloDetaljnoVM
            {
                VoziloID = x.VoziloID,
                BrojSasije = x.BrojSasije,
                RegistarskaOznaka = x.RegistarskaOznaka,
                Boja = x.Boja,
                Model = x.Model,
                GodinaProizvodnje = x.GodinaProizvodnje,
                SnagaMotora = x.SnagaMotora,
                DatumMijenjanjUlja = x.DatumMijenjanjUlja.ToShortDateString(),
                Domet = x.Domet,
                BrojMjesta = x.BrojMjesta,
                BrojVrata = x.BrojVrata,
                ZapreminaPrtljaznika = x.ZapreminaPrtljaznika,
                ZapreminaPrtljaznikaNaprijed = x.ZapreminaPrtljaznikaNaprijed,
                Naziv = x.Naziv,
                Brend = x.Brend.Naziv,
                Klima = x.Klima,
                TipVozila = x.TipVozila,
                DodatniOpis = x.DodatniOpis,
                Gorivo = x.Gorivo,
                Pogon = x.Pogon,
                Transmisija = x.Transmisija,
                GrupniTipVozila = x.GrupniTipVozila,
                Slike = x.slike.Where(y => y.VoziloID == id).Select(u => u.URL).ToList(),
                Kilometraza = x.Kilometraza,
                Cijena = x.Cijena,
                prikolice = _context.KompatibilnostPrikolica.Where(s => s.VoziloID == id).Select(p => new VoziloDetaljnoVM.Row
                {
                    PrikolicaID = p.PrikolicaID,
                    Sirina = p.Prikolica.Sirina,
                    Zapremina = p.Prikolica.Zapremina,
                    Duzina = p.Prikolica.Duzina,
                    TipPrikolice = p.Prikolica.TipPrikolice,
                    TipKuke = p.TipKuke,
                    Tezina = p.Tezina.ToString()
                }).ToList()
            }).FirstOrDefault();

            return View(nameof(Detalji), model);
        }
    }

}
