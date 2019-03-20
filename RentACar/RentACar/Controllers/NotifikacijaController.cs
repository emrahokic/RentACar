using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACar.Data;
using RentACar.Models;
using RentACar.ViewModels;

namespace RentACar.Controllers
{
    public class NotifikacijaController : Controller
    {
        private UserManager<ApplicationUser> _signInManager;
        private ApplicationDbContext db;
        public NotifikacijaController(UserManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _signInManager = signInManager;
            this.db = db;
        }

        public IActionResult Index()
        {
            return Redirect("/Profil");
        }


        public IActionResult GetNotifications()
        {


            int id = int.Parse(_signInManager.GetUserId(User));
            if (User.IsInRole("Klijent"))
            {
                NotifikacijaProfilVM model = new NotifikacijaProfilVM()
                {
                    notifikacije = db.Notifikacija.Where(x => x.UserID == id).Select(n => new NotifikacijaProfilVM.Row()
                    {
                        Poruka = n.Poruka,
                        Otvorena = n.Otvorena,
                        RezervacijaID = (int)n.RezervacijaID,
                        Vrijeme = getVrijeme(n.Vrijeme)
                    }).ToList()
                };
                model.BrojNeprocitanih = model.notifikacije.Count(r => r.Otvorena == false);
                return PartialView("NotifikacijaPV", model);

            }


            if (User.IsInRole("Uposlenik"))
            {

                int poslovnicaId = db.UgovorZaposlenja.Where(x => x.UposlenikID == id).Select(y => y.PoslovnicaID).FirstOrDefault();
                NotifikacijaProfilVM model = new NotifikacijaProfilVM()
                {
                    notifikacije = db.Notifikacija.Where(x => x.PoslovnicaID == poslovnicaId).Select(n => new NotifikacijaProfilVM.Row()
                    {
                        Poruka = n.Poruka,
                        Otvorena = n.Otvorena,
                        RezervacijaID = (int)n.RezervacijaID,
                        Vrijeme = getVrijeme(n.Vrijeme)
                    }).ToList()
                };

                model.BrojNeprocitanih = model.notifikacije.Count(r => r.Otvorena == false);

                return PartialView("NotifikacijaPV", model);
            }
            return PartialView("NotifikacijaPV");
        }

        private string getVrijeme(DateTime vrijeme)
        {


            if ((DateTime.Now - vrijeme).TotalDays > 1)
            {
                return vrijeme.Day + "." + vrijeme.Month + "." + vrijeme.Year;

            }

            if ((DateTime.Now - vrijeme).TotalHours > 24)
            {
                return ((int)((DateTime.Now - vrijeme).TotalDays)) + " days ago";
            }
            if ((DateTime.Now - vrijeme).TotalMinutes > 60)
            {
                return ((int)((DateTime.Now - vrijeme).TotalHours)) + " h ago";
            }


            if ((DateTime.Now - vrijeme).TotalMinutes < 60)
            {
                return ((int)((DateTime.Now - vrijeme).TotalMinutes)) + " min ago";
            }
            return vrijeme.Day + "." + vrijeme.Month + "." + vrijeme.Year;
        }
    }
}