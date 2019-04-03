using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Data;
using RentACar.Models;
using RentACar.Areas.Uposlenik.ViewModels;

namespace RentACar.Areas.Uposlenik.Controllers
{
    [Authorize(Roles = "Administrator,Uposlenik")]
    [Area("Uposlenik")]
    public class UslugeController : Controller

    {
        private ApplicationDbContext _context;

        public UslugeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model1 = new DodatneUslugeIndexVM
            {
                rows = _context.DodatneUsluge.Select(k => new Rows
                {
                    DodatneUslugeID = k.DodatneUslugeID,
                    Naziv = k.Naziv,
                    Opis = k.Opis,
                    Cijena = k.Cijena
                }).ToList()
            };
  
            return View(nameof(Index), model1);
        }

        public IActionResult Dodaj()
        {
            return View(nameof(Dodaj));
        }

        public IActionResult DodajSnimi(string Naziv, string Opis, double Cijena)
        {
            if(Opis == null)
            {
                Opis = "Nema opisa";
            }
            DodatneUsluge nova = new DodatneUsluge
            {
                Naziv = Naziv,
                Opis = Opis,
                Cijena = Cijena
            };

            _context.Add(nova);
            _context.SaveChanges();
            _context.Dispose();

            return Redirect(nameof(Index));
        }


        public IActionResult Obrisi(int id)
        {
            DodatneUsluge temp = new DodatneUsluge();
            temp = _context.DodatneUsluge.Find(id);
            if (temp == null)
            {
                return Content("Usluga ne postoji");
            }
            _context.Remove(temp);

            _context.SaveChanges();
            _context.Dispose();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Uredi(int id)
        {
            DodatneUslugeVM model = _context.DodatneUsluge.Where(d => d.DodatneUslugeID == id).Select(s =>
            new DodatneUslugeVM
            {
                DodatnaUslugaID = s.DodatneUslugeID,
                Naziv = s.Naziv,
                Opis = s.Opis,
                Cijena = s.Cijena
            }).SingleOrDefault();
            if (model == null)
            {
                return Content("Usluga ne postoji");
            }

            return View(nameof(Uredi), model);
        }

        public IActionResult UrediSnimi(int DodatneUslugeID, string Naziv, string Opis, double Cijena)
        {
            DodatneUsluge x = _context.DodatneUsluge.Find(DodatneUslugeID);

            if (x == null)
            {
                return Content("Usluga ne postoji");
            }

            x.DodatneUslugeID = DodatneUslugeID;
            x.Naziv = Naziv;
            x.Opis = Opis;
            x.Cijena = Cijena;

            _context.SaveChanges();
            _context.Dispose();

            return Redirect(nameof(Index));
        }
    }
}