using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Data;
using RentACar.Models;
using RentACar.Areas.Klijent.ViewModels;
using RentACar.Helper;
using RentACar.ViewModels;

namespace RentACar.Areas.Klijent.Controllers
{
    [Area("Klijent")]
    [Authorize(Roles = "Klijent")]
    public class RezervacijaController : Controller
    {
        private  UserManager<ApplicationUser> _signInManager;
        private ApplicationDbContext db;
        public RezervacijaController(ApplicationDbContext _db, UserManager<ApplicationUser> signInManager)
        {
            db = _db;
            _signInManager = signInManager;
        }


        //Rezervacije za Klijenta
        public IActionResult Index()
        {
            int KlijentID = int.Parse(_signInManager.GetUserId(User));
            RezervacijaIndexVM model = new RezervacijaIndexVM() {
                Rows = db.Rezervacija.Where(z => z.KlijentID == KlijentID && z.UspjesnoSpremljena == true).OrderByDescending(c=>c.DatumRezervacije).Select(x => new RezervacijaIndexVM.Row()
                {
                     RezervacijaID = x.RezervacijaID,
                     Vozilo = x.Vozilo.Naziv,
                     DatumPreuzimanja = x.DatumPreuzimanja,
                     DatumPovrata = x.DatumPovrata,
                     Poslovnica = x.Poslovnica.Naziv,
                     Zakljucen = x.Zakljucen,
                     Brend = x.Vozilo.Brend.Naziv
                }).ToList()

            };
            return View(model);
        }

        public IActionResult Dodaj(int vozilo)
        {
            RezervacijaDodajVM model = new RezervacijaDodajVM();

            model.Vozilo = db.Vozilo.FirstOrDefault(x => x.VoziloID == vozilo);
            var idP = db.TrenutnaPoslovnica.FirstOrDefault(t => t.VoziloID == vozilo).PoslovnicaID;
            VozilaVM.Row vr = new VozilaVM.Row
            {
                Brend = db.Brend.FirstOrDefault(z => z.BrendID == model.Vozilo.BrendID).Naziv,
                Naziv = model.Vozilo.Naziv,
                Cijena = model.Vozilo.Cijena,
                PoslovnicaNaziv = db.Poslovnica.FirstOrDefault(x => x.PoslovnicaID == idP).Naziv,
                Slika = db.Slika.FirstOrDefault(y => y.VoziloID == vozilo && y.Pozicija == 1)
            };
            model.VoziloVM = vr;
            return View(nameof(Dodaj), model);
        }

        public string GenerisiSifruRezervacije(int g)
        {
            return "";
        }
        [HttpPost]
        public IActionResult DodatneUslugePV(RezervacijaDodajVM model)
        {
            int god = DateTime.Now.Year;
            int KlijentID = int.Parse(_signInManager.GetUserId(User));
            int pID = db.TrenutnaPoslovnica.Where(x => x.VoziloID == model.Vozilo.VoziloID).FirstOrDefault().PoslovnicaID;
            double c = db.Vozilo.Where(vo => vo.VoziloID == model.Vozilo.VoziloID).FirstOrDefault().Cijena;
            Rezervacija r = new Rezervacija {
                VoziloID = model.Vozilo.VoziloID,
                PoslovnicaID = pID,
                DatumRezervacije = DateTime.Now,
                DatumPovrata = model.DatumPovrata,
                DatumPreuzimanja = model.DatumPreuzimanja,
                Zakljucen = (int)InfoRezervacija.U_Obradi,
                Cijena = c * (model.DatumPovrata - model.DatumPreuzimanja).Days,
                KlijentID = KlijentID,
                BrojDanaIznajmljivanja = (model.DatumPovrata - model.DatumPreuzimanja).Days,
                NacinPlacanja = (int)NacinPlacanja.NijeDefinisano,
                UspjesnoSpremljena = false
            };

            db.Add(r);
            db.SaveChanges();
            int rezervacijaID = r.RezervacijaID;
            DodatneUslugeDodajVM dodatneUslugeDodajVM = new DodatneUslugeDodajVM();
            dodatneUslugeDodajVM.RezervacijaID = rezervacijaID;
            List<DodatneUsluge> DodatneUsluge = db.DodatneUsluge.ToList();

            bool kuka = db.Vozilo.FirstOrDefault(v => v.VoziloID == model.Vozilo.VoziloID).Kuka;
            dodatneUslugeDodajVM.Kuka = kuka;

            dodatneUslugeDodajVM.rows = new List<DodatneUslugeDodajVM.UslugeVm>();

            for (int i = 0; i < DodatneUsluge.Count; i++)
            {
                DodatneUslugeDodajVM.UslugeVm x = new DodatneUslugeDodajVM.UslugeVm
                {
                    Cijena = DodatneUsluge[i].Cijena,
                    DodatnaUslugaID = DodatneUsluge[i].DodatneUslugeID,
                    Naziv = DodatneUsluge[i].Naziv,
                    Opis = DodatneUsluge[i].Opis
                };
                dodatneUslugeDodajVM.rows.Add(x);
            }

            return View(nameof(DodatneUslugePV), dodatneUslugeDodajVM);
        }

        [HttpPost]
        public IActionResult DodatneUslugeDodaj(DodatneUslugeDodajVM model)
        {

            List<RezervisanaUsluga> rezervisanaUslugas = new List<RezervisanaUsluga>();
            for (int i = 0; i < model.rows.Count; i++)
            {
                if (model.rows[i].Selected)
                {
                    double cijena = (db.DodatneUsluge.Where( du => du.DodatneUslugeID == model.rows[i].DodatnaUslugaID).FirstOrDefault().Cijena) * model.rows[i].Kolicina;
                    RezervisanaUsluga r = new RezervisanaUsluga
                    {
                        Kolicina = model.rows[i].Kolicina,
                        DodatneUslugeID = model.rows[i].DodatnaUslugaID,
                        UkupnaCijenaUsluge = cijena,
                        RezervacijaID = model.RezervacijaID
                    };
                    rezervisanaUslugas.Add(r);
                }
            }

            db.AddRange(rezervisanaUslugas);
            db.SaveChanges();
            //povlcenje prikolica
            if (model.DodajPrikolicu)
            {
                int voziloID = db.Rezervacija.Where(rez => rez.RezervacijaID == model.RezervacijaID).FirstOrDefault().VoziloID;
                List<KompatibilnostPrikolica> kompatibilnostPrikolicas = db.KompatibilnostPrikolica.Where(kp => kp.VoziloID == voziloID).ToList();
                List<Prikolica> prikolice = new List<Prikolica>();
                for (int i = 0; i < kompatibilnostPrikolicas.Count; i++)
                {
                    Prikolica p = db.Prikolica.Where(pr => pr.PrikolicaID == kompatibilnostPrikolicas[i].PrikolicaID).FirstOrDefault();
                    prikolice.Add(p);
                }

                PrikoliceDodajVM prikoliceDodajVM = new PrikoliceDodajVM();
                prikoliceDodajVM.rows = new List<PrikoliceDodajVM.PrikoliceVm>();
                prikoliceDodajVM.RezervacijaID = model.RezervacijaID;
                for (int i = 0; i < prikolice.Count; i++)
                {
                    PrikoliceDodajVM.PrikoliceVm vm = new PrikoliceDodajVM.PrikoliceVm
                    {
                        Cijena = prikolice[i].Cijna,
                        Sirina = prikolice[i].Sirina,
                        Zapremina = prikolice[i].Zapremina,
                        Duzina = prikolice[i].Duzina,
                        PrikolicaID = prikolice[i].PrikolicaID,
                        TipPrikolice = prikolice[i].TipPrikolice,
                    };
                    prikoliceDodajVM.rows.Add(vm);
                }

                return View("PrikoliceDodajPV", prikoliceDodajVM);
            }
            DodajPlacanjeSpremiPV dodajPlacanjeSpremiPV = new DodajPlacanjeSpremiPV();
            dodajPlacanjeSpremiPV.RezervacijaID = model.RezervacijaID;
            return View("DodajPlacanje", dodajPlacanjeSpremiPV);
        }

        [HttpPost]
        public IActionResult PrikoliceSpremi(PrikoliceDodajVM model)
        {
            Rezervacija r = db.Rezervacija.Find(model.RezervacijaID);
            double cijenaRezervacijeOld = r.Cijena;
            int brojDanaRezervacije= r.BrojDanaIznajmljivanja;
            if (model.Selected != null && model.Selected != "" && int.Parse(model.Selected )>0)
            {
                double cijenaPrikolice = db.Prikolica.Where(p => p.PrikolicaID == int.Parse(model.Selected)).FirstOrDefault().Cijna;
                r.PrikolicaID = int.Parse(model.Selected);
                r.Cijena = cijenaRezervacijeOld + (cijenaPrikolice * brojDanaRezervacije);
                db.SaveChanges();
            }
            DodajPlacanjeSpremiPV dodajPlacanjeSpremiPV = new DodajPlacanjeSpremiPV();
            dodajPlacanjeSpremiPV.RezervacijaID = model.RezervacijaID;
            return View("DodajPlacanje",dodajPlacanjeSpremiPV);
        }

        [HttpPost]
        public IActionResult DodajPlacanjeSpremi(DodajPlacanjeSpremiPV model)
        {
            Rezervacija r = db.Rezervacija.Find(model.RezervacijaID);
            if (model.NacinPlacanja != null && model.NacinPlacanja != "")
            {
                var NP = Enum.GetNames(typeof(NacinPlacanja));
           
                r.NacinPlacanja = int.Parse(model.NacinPlacanja);
                db.SaveChanges();
            }
            return RedirectToAction("RezervacijaDetaljno", new { id = model.RezervacijaID});
        }
        //[HttpPost]
        //public IActionResult Dodaj(RezervacijaDodajVM model)
        //{
        //    Rezervacija rezervacija = new Rezervacija() {
        //        DatumRezervacije = System.DateTime.Now,
        //        DatumPreuzimanja = model.DatumPreuzimanja,
        //        DatumPovrata = model.DatumPovrata,
        //        NacinPlacanja = model.NacinPlacanja,
        //        KlijentID = int.Parse(_signInManager.GetUserId(User)),
        //        BrojDanaIznajmljivanja = dateDiff(model.DatumPreuzimanja, model.DatumPovrata),
        //        Cijena=1,
        //        //VoziloID=model.VoziloID,
        //        //PoslovnicaID=db.TrenutnaPoslovnica.FirstOrDefault(s=>s.VoziloID==model.VoziloID).PoslovnicaID,
        //        Zakljucen= (int)InfoRezervacija.U_Obradi



        //    };
        //    db.Rezervacija.Add(rezervacija);
        //    db.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        public IActionResult DodajOcjenuKomentar(string Poruka, int? Ocjena, int RezervacijaID)
        {
            if (Ocjena == null && Poruka != null)
            {
                Ocjena = 0;
            }
            if (Ocjena>=0 && Poruka != null && RezervacijaID>0)
            {

            int KlijentID = int.Parse(_signInManager.GetUserId(User));
            Rezervacija tempR = db.Rezervacija.Find(RezervacijaID);
            ApplicationUser tempK = db.Users.Find(KlijentID);
            OcjenaRezervacija rezervacija = new OcjenaRezervacija()
            {
                Poruka = Poruka,
                OcjenaVrijednost =(int) Ocjena,
                Rezervacija = tempR,
                DatumOcjene = DateTime.Now,
                Klijent = tempK
            };
            db.OcjenaRezervacija.Add(rezervacija);
            db.SaveChanges();
            return PartialView("OcjenaKomentar",rezervacija);
            }
            return PartialView("OcjenaKomentar", null);

        }

        private int dateDiff(DateTime datumPreuzimanja, DateTime datumPovrata)
        {
            TimeSpan t = datumPovrata - datumPreuzimanja;
            return t.Days;
        }

        public IActionResult OcjenaKomentar(int id)
        {
            OcjenaRezervacija model = db.OcjenaRezervacija.Where(y => y.RezervacijaID == id).Select(c => new OcjenaRezervacija
            {
                Poruka = c.Poruka,
                OcjenaVrijednost = c.OcjenaVrijednost
            }).FirstOrDefault();

            return PartialView(nameof(OcjenaKomentar), model);
        }


        public IActionResult Detalji(int id)
        {
            RezervacijaDetaljnoVM model = db.Rezervacija.Where(z => z.RezervacijaID == id).Select(x => new RezervacijaDetaljnoVM {
                RezervacijaID = x.RezervacijaID,
                Grad = x.Poslovnica.Grad.Naziv,
                SlikaVozila= db.Slika.Where(sl => sl.VoziloID == x.VoziloID && sl.Pozicija ==1 ).Select(c => new Slika
                {
                    Name = c.Name,
                    SlikaID = c.SlikaID,
                    URL = c.URL,
                    Pozicija = c.Pozicija
                }).FirstOrDefault(),
                DatumPreuzimanja = x.DatumPreuzimanja,
                DatumRezervacije = x.DatumRezervacije,
                DatumPovrata = x.DatumPovrata,
                Zakljucen = x.Zakljucen,
                Cijena = x.Cijena,
                BrojDanaIznajmljivanja = x.BrojDanaIznajmljivanja,
                NacinPlacanja = x.NacinPlacanja,
                Vozilo = x.Vozilo.Naziv,
                Brend = x.Vozilo.Brend.Naziv,
                Poslovnica = x.Poslovnica.Naziv,
                dodatneUsluge = db.RezervisanaUsluga.Where(u => u.RezervacijaID == x.RezervacijaID).Select(ru => new RezervacijaDetaljnoVM.Row{
                    Naziv = ru.DodatneUsluge.Naziv,
                    RezervisanaUslugaID = ru.RezervisanaUslugaID,
                    Cijena = db.DodatneUsluge.FirstOrDefault(d => d.DodatneUslugeID == ru.DodatneUslugeID).Cijena,
                    Kolicina = ru.Kolicina,
                    Opis = db.DodatneUsluge.FirstOrDefault(d => d.DodatneUslugeID == ru.DodatneUslugeID).Opis,
                    UkupnaCijenaUsluge = ru.UkupnaCijenaUsluge
                }).ToList()
            }).FirstOrDefault();

            return View(nameof(Detalji), model);
        }

        public IActionResult ZavrsiRezervaciju(int id)
        {
            Rezervacija r = db.Rezervacija.Find(id);
            int sifra = (DateTime.Now.Year % 100) * 10000 + r.RezervacijaID;
            r.SifraRezervacije = sifra + "/" + DateTime.Now.Year;
            r.UspjesnoSpremljena = true;
            db.SaveChanges();
            TrenutnaPoslovnica tp = db.TrenutnaPoslovnica.Where(TP => TP.VoziloID == r.VoziloID).FirstOrDefault();
            tp.VoziloRezervisano = true;
            db.SaveChanges();
            return Redirect("/Klijent/Rezervacija/Index");
        }

        public IActionResult RezervacijaDetaljno(int id)
        {
            int KlijentID = int.Parse(_signInManager.GetUserId(User));
            int kFromRezervaijca = db.Rezervacija.Where(x => x.RezervacijaID == id).FirstOrDefault().KlijentID;
            if (KlijentID == kFromRezervaijca)
            {
                RezervacijaDetaljnoVM model = db.Rezervacija.Where(z => z.RezervacijaID == id).Select(x => new RezervacijaDetaljnoVM
                {
                    RezervacijaID = x.RezervacijaID,
                    Grad = x.Poslovnica.Grad.Naziv,
                    SlikaVozila = db.Slika.Where(sl => sl.VoziloID == x.VoziloID && sl.Pozicija == 1).Select(c => new Slika
                    {
                        Name = c.Name,
                        SlikaID = c.SlikaID,
                        URL = c.URL,
                        Pozicija = c.Pozicija
                    }).FirstOrDefault(),
                    DatumPreuzimanja = x.DatumPreuzimanja,
                    DatumPovrata = x.DatumPovrata,
                    Zakljucen = x.Zakljucen,
                    Cijena = x.Cijena,
                    BrojDanaIznajmljivanja = x.BrojDanaIznajmljivanja,
                    NacinPlacanja = x.NacinPlacanja,
                    Vozilo = x.Vozilo.Naziv,
                    Brend = x.Vozilo.Brend.Naziv,
                    Poslovnica = x.Poslovnica.Naziv,
                    dodatneUsluge = db.RezervisanaUsluga.Where(u => u.RezervacijaID == x.RezervacijaID).Select(ru => new RezervacijaDetaljnoVM.Row
                    {
                        Naziv = ru.DodatneUsluge.Naziv,
                        RezervisanaUslugaID = ru.RezervisanaUslugaID,
                        Cijena = db.DodatneUsluge.FirstOrDefault(d => d.DodatneUslugeID == ru.DodatneUslugeID).Cijena,
                        Kolicina = ru.Kolicina,
                        Opis = db.DodatneUsluge.FirstOrDefault(d => d.DodatneUslugeID == ru.DodatneUslugeID).Opis,
                        UkupnaCijenaUsluge = ru.UkupnaCijenaUsluge
                    }).ToList()
                }).FirstOrDefault();

                return View("RezervacijaPregled", model);
            }
                return View("RezervacijaPregled");

        }

    }
}