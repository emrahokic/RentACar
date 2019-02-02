using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RentACar.Data;
using RentACar.Models;
using RentACar.ViewModels;

public class UslugeController: Controller

{
    private ApplicationDbContext _context;

    public UslugeController (ApplicationDbContext context)
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

        //_context.Dispose();        
        return View("Index", model1);
    }

    public IActionResult DodajForm()
    {
        return View("DodajForm");
    }

    public IActionResult Dodaj(string Naziv, string Opis, double Cijena)
    {
        DodatneUsluge nova = new DodatneUsluge
        {
            Naziv = Naziv,
            Opis = Opis,
            Cijena = Cijena
        };

        _context.Add(nova);
        _context.SaveChanges();
        _context.Dispose();

        return Redirect("/Usluge/Index");
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

        return RedirectToAction("Index");
    }

    public IActionResult UrediForm(int id)
    {
        DodatneUsluge temp = new DodatneUsluge();
        temp = _context.DodatneUsluge.Find(id);
        if (temp == null)
        {
            return Content("Usluga ne postoji");
        }

        return View("UrediForm", temp);
    }

    public IActionResult UrediSnimi(int DodatneUslugeID, string Naziv, string Opis, double Cijena)
    {
        DodatneUsluge x = _context.DodatneUsluge.Find(DodatneUslugeID);

        x.DodatneUslugeID = DodatneUslugeID;
        x.Naziv = Naziv;
        x.Opis = Opis;
        x.Cijena = Cijena;

        _context.SaveChanges();
        _context.Dispose();

        return Redirect("/Usluge/Index");
    }
}
