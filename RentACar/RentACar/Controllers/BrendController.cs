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

namespace RentACar.Controllers
{
    [Authorize]
    public class BrendController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrendController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Brend
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brend.ToListAsync());
        }

        // GET: Brend/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brend = await _context.Brend
                .FirstOrDefaultAsync(m => m.BrendID == id);
            if (brend == null)
            {
                return NotFound();
            }

            return View(brend);
        }

        // GET: Brend/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brend/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrendID,Naziv")] Brend brend)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brend);
        }

        // GET: Brend/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brend = await _context.Brend.FindAsync(id);
            if (brend == null)
            {
                return NotFound();
            }
            return View(brend);
        }

        // POST: Brend/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrendID,Naziv")] Brend brend)
        {
            if (id != brend.BrendID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrendExists(brend.BrendID))
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
            return View(brend);
        }

        // GET: Brend/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brend = await _context.Brend
                .FirstOrDefaultAsync(m => m.BrendID == id);
            if (brend == null)
            {
                return NotFound();
            }

            return View(brend);
        }

        // POST: Brend/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brend = await _context.Brend.FindAsync(id);
            _context.Brend.Remove(brend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrendExists(int id)
        {
            return _context.Brend.Any(e => e.BrendID == id);
        }
    }
}
