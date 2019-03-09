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
using RentACar.ViewModels;

namespace RentACar.Controllers
{
    [Authorize]
    public class VoziloController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VoziloController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new VoziloVM
            {
                rows = _context.Vozilo.Select(v => new VoziloVM.Row
                {
                    VoziloID = v.VoziloID,
                    Brend = v.Brend.Naziv,
                    Naziv = v.Naziv,
                    Model = v.Model,
                    BrojVrata = v.BrojVrata,
                    TipVozila = v.TipVozila,
                    Transmisija = v.Transmisija
                }).ToList()
            };

            return View(nameof(Index), model);
        }

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
