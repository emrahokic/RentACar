using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            //Vozilo x = _context.Vozilo.Where(w => w.VoziloID == id)
            //    .Include(q => q.Brend)
            //    .SingleOrDefault();

            //VoziloDetaljnoVM model = new VoziloDetaljnoVM
            //{
            //    VoziloID = x.VoziloID,
            //    BrojSasije = x.BrojSasije,
            //    RegistarskaOznaka = x.RegistarskaOznaka,
            //    Boja = x.Boja,
            //    Model = x.Model,
            //    GodinaProizvodnje = x.GodinaProizvodnje,
            //    SnagaMotora = x.SnagaMotora,
            //    DatumMijenjanjUlja = x.DatumMijenjanjUlja.ToShortDateString(),
            //    Domet = x.Domet,
            //    BrojMjesta = x.BrojMjesta,
            //    BrojVrata = x.BrojVrata,
            //    ZapreminaPrtljaznika = x.ZapreminaPrtljaznika,
            //    ZapreminaPrtljaznikaNaprijed = x.ZapreminaPrtljaznikaNaprijed,
            //    Naziv = x.Naziv,
            //    Brend = x.Brend.Naziv,
            //    Klima = x.Klima,
            //    TipVozila = x.TipVozila,
            //    DodatniOpis = x.DodatniOpis,
            //    Gorivo = x.Gorivo,
            //    Pogon = x.Pogon,
            //    Transmisija = x.Transmisija,
            //    GrupniTipVozila = x.GrupniTipVozila,
            //    Slike = x.slike.Where(y => y.VoziloID == id).Select(u => u.URL).ToList()
            //};

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
    //    // GET: Vozilo
    //    public async Task<IActionResult> Index()
    //    {
    //        var applicationDbContext = _context.Vozilo.Include(v => v.Brend);
    //        return View(await applicationDbContext.ToListAsync());
    //    }

    //    // GET: Vozilo/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var vozilo = await _context.Vozilo
    //            .Include(v => v.Brend)
    //            .FirstOrDefaultAsync(m => m.VoziloID == id);
    //        if (vozilo == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(vozilo);
    //    }

    //    // GET: Vozilo/Create
    //    public IActionResult Create()
    //    {
    //        ViewData["BrendID"] = new SelectList(_context.Brend, "BrendID", "Naziv");
    //        return View();
    //    }

    //    // POST: Vozilo/Create
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("VoziloID,BrojSasije,RegistarskaOznaka,Boja,Model,GodinaProizvodnje,SnagaMotora,DatumMijenjanjUlja,Domet,BrojMjesta,BrojVrata,ZapreminaPrtljaznika,ZapreminaPrtljaznikaNaprijed,Naziv,BrendID,Klima,TipVozila,DodatniOpis,Gorivo,Pogon,Transmisija,GrupniTipVozila")] Vozilo vozilo)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(vozilo);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["BrendID"] = new SelectList(_context.Brend, "BrendID", "Naziv", vozilo.BrendID);
    //        return View(vozilo);
    //    }

    //    // GET: Vozilo/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var vozilo = await _context.Vozilo.FindAsync(id);
    //        if (vozilo == null)
    //        {
    //            return NotFound();
    //        }
    //        ViewData["BrendID"] = new SelectList(_context.Brend, "BrendID", "Naziv", vozilo.BrendID);
    //        return View(vozilo);
    //    }

    //    // POST: Vozilo/Edit/5
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("VoziloID,BrojSasije,RegistarskaOznaka,Boja,Model,GodinaProizvodnje,SnagaMotora,DatumMijenjanjUlja,Domet,BrojMjesta,BrojVrata,ZapreminaPrtljaznika,ZapreminaPrtljaznikaNaprijed,Naziv,BrendID,Klima,TipVozila,DodatniOpis,Gorivo,Pogon,Transmisija,GrupniTipVozila")] Vozilo vozilo)
    //    {
    //        if (id != vozilo.VoziloID)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(vozilo);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!VoziloExists(vozilo.VoziloID))
    //                {
    //                    return NotFound();
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["BrendID"] = new SelectList(_context.Brend, "BrendID", "BrendID", vozilo.BrendID);
    //        return View(vozilo);
    //    }

    //    // GET: Vozilo/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var vozilo = await _context.Vozilo
    //            .Include(v => v.Brend)
    //            .FirstOrDefaultAsync(m => m.VoziloID == id);
    //        if (vozilo == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(vozilo);
    //    }

    //    // POST: Vozilo/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        var vozilo = await _context.Vozilo.FindAsync(id);
    //        _context.Vozilo.Remove(vozilo);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool VoziloExists(int id)
    //    {
    //        return _context.Vozilo.Any(e => e.VoziloID == id);
    //    }
    //}
}
