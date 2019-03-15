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
    public class VoziloController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VoziloController(ApplicationDbContext context)
        {
            _context = context;
        }

  


        //ovaj ostavi ovdje 
        public IActionResult Index()
        {
            return View(nameof(Index));
        }


        public IActionResult VozilaPartialView(string grad, int? poslovnicaId)
        {
            List<TrenutnaPoslovnica> temp = null;
            VozilaVM vozila = new VozilaVM();
            vozila.rows = new List<VozilaVM.Row>();
            if (grad == null && poslovnicaId == null)
            {
                temp = _context.TrenutnaPoslovnica.Where(x => x.VoziloRezervisano == false).ToList();
            }
            if (grad != null)
            {
                temp = _context.TrenutnaPoslovnica.Where(x => x.VoziloRezervisano == false
            && (x.Poslovnica.Grad.Naziv.Contains(grad))).ToList();
                vozila.GradSearch = grad;
            }
            if (poslovnicaId != null)
            {
                temp = _context.TrenutnaPoslovnica.Where(x => x.VoziloRezervisano == false
            && (x.PoslovnicaID == poslovnicaId)).ToList();

            }
           

            for (int i = 0; i < temp.Count; i++)
            {
                VozilaVM.Row item = _context.Vozilo.Where(v => v.VoziloID == temp[i].VoziloID).Select(y => new VozilaVM.Row
                {
                    Naziv = y.Naziv,
                    VoziloID = y.VoziloID,
                    Gorivo = y.Gorivo,
                    Brend = y.Brend.Naziv,
                    PoslovnicaID = temp[i].PoslovnicaID,
                    PoslovnicaLokacija = _context.Poslovnica.FirstOrDefault(p => p.PoslovnicaID == temp[i].PoslovnicaID).Grad.Naziv,
                    PoslovnicaNaziv = _context.Poslovnica.FirstOrDefault(p => p.PoslovnicaID == temp[i].PoslovnicaID).Naziv,
                    Slika = _context.Slika.Where(sl => sl.VoziloID == y.VoziloID && sl.Pozicija == 1).Select(c => new Slika
                    {
                        Name = c.Name,
                        SlikaID = c.SlikaID,
                        URL = c.URL,
                        Pozicija = c.Pozicija
                    }).FirstOrDefault(),
                    TipVozila = y.TipVozila,
                    Transmisija = y.Transmisija
                }).FirstOrDefault();

                vozila.rows.Add(item);
            }


            return PartialView("_partialViewVozila", vozila);
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
