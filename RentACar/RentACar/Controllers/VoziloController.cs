using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;

namespace RentACar.Controllers
{
    public class VoziloController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VoziloController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vozilo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vozilo.Include(v => v.Brend);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vozilo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vozilo = await _context.Vozilo
                .Include(v => v.Brend)
                .FirstOrDefaultAsync(m => m.VoziloID == id);
            if (vozilo == null)
            {
                return NotFound();
            }

            return View(vozilo);
        }

        // GET: Vozilo/Create
        public IActionResult Create()
        {
            ViewData["BrendID"] = new SelectList(_context.Brend, "BrendID", "Naziv");
            return View();
        }

        // POST: Vozilo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoziloID,BrojSasije,RegistarskaOznaka,Boja,Model,GodinaProizvodnje,SnagaMotora,DatumMijenjanjUlja,Domet,BrojMjesta,BrojVrata,ZapreminaPrtljaznika,ZapreminaPrtljaznikaNaprijed,Naziv,BrendID,Klima,TipVozila,DodatniOpis,Gorivo,Pogon,Transmisija,GrupniTipVozila")] Vozilo vozilo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vozilo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrendID"] = new SelectList(_context.Brend, "BrendID", "Naziv", vozilo.BrendID);
            return View(vozilo);
        }

        // GET: Vozilo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vozilo = await _context.Vozilo.FindAsync(id);
            if (vozilo == null)
            {
                return NotFound();
            }
            ViewData["BrendID"] = new SelectList(_context.Brend, "BrendID", "Naziv", vozilo.BrendID);
            return View(vozilo);
        }

        // POST: Vozilo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VoziloID,BrojSasije,RegistarskaOznaka,Boja,Model,GodinaProizvodnje,SnagaMotora,DatumMijenjanjUlja,Domet,BrojMjesta,BrojVrata,ZapreminaPrtljaznika,ZapreminaPrtljaznikaNaprijed,Naziv,BrendID,Klima,TipVozila,DodatniOpis,Gorivo,Pogon,Transmisija,GrupniTipVozila")] Vozilo vozilo)
        {
            if (id != vozilo.VoziloID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vozilo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoziloExists(vozilo.VoziloID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrendID"] = new SelectList(_context.Brend, "BrendID", "BrendID", vozilo.BrendID);
            return View(vozilo);
        }

        // GET: Vozilo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vozilo = await _context.Vozilo
                .Include(v => v.Brend)
                .FirstOrDefaultAsync(m => m.VoziloID == id);
            if (vozilo == null)
            {
                return NotFound();
            }

            return View(vozilo);
        }

        // POST: Vozilo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vozilo = await _context.Vozilo.FindAsync(id);
            _context.Vozilo.Remove(vozilo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoziloExists(int id)
        {
            return _context.Vozilo.Any(e => e.VoziloID == id);
        }
    }
}
