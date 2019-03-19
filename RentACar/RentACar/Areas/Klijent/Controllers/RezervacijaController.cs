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
using Microsoft.AspNetCore.Http;

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

        public IActionResult Rezervacija(string vozilo)
        {
            RezervacijaDodajVM model = new RezervacijaDodajVM();
            string[] StringVozilo = new string[2];
            try
            {
                StringVozilo = vozilo.Split('-');

                model.Vozilo = vozilo;

            }
            catch (Exception)
            {

            }
            string SifraRezervacije = null;

            try
            {
                SifraRezervacije = Request.Cookies["Sesion"];
            }
            catch (Exception)
            {

                throw;
            }
            if (SifraRezervacije == null)
            {
                //redirect na pocetnu str ili ispisati nesto
            }
            //Preuzeti vozilo iz rezervacije

            Vozilo _Vozilo = null;

            if (SifraRezervacije != null)
            {
                Rezervacija r = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
                int idV = r.VoziloID;
                _Vozilo = db.Vozilo.Where(x => x.VoziloID == idV).FirstOrDefault();

                StringVozilo[0] = db.Vozilo.Where(x => x.VoziloID == idV).Select(y => y.Brend.Naziv).FirstOrDefault();
                StringVozilo[1] = _Vozilo.Naziv;
                model.Vozilo = StringVozilo[0]+"-"+ StringVozilo[1];

                VozilaVM.Row VR = new VozilaVM.Row
                {
                    Brend = db.Brend.FirstOrDefault(z => z.BrendID == _Vozilo.BrendID).Naziv,
                    Naziv = _Vozilo.Naziv,
                    Cijena = _Vozilo.Cijena,
                    PoslovnicaNaziv = db.Poslovnica.FirstOrDefault(x => x.PoslovnicaID == r.PoslovnicaID).Naziv,
                    PoslovnicaLokacija = db.Poslovnica.Where(x => x.PoslovnicaID == r.PoslovnicaID).Select(y => y.Grad.Naziv).FirstOrDefault(),
                    Slika = db.Slika.FirstOrDefault(y => y.VoziloID == _Vozilo.VoziloID && y.Pozicija == 1)
                };
                model.VoziloVM = VR;
                model.DatumPreuzimanja = r.DatumPreuzimanja;
                model.DatumPovrata = r.DatumPovrata;
                return View(nameof(Rezervacija), model);
            }
            else
            {

               _Vozilo = db.Vozilo.Where(x => x.Naziv == StringVozilo[1] && x.Brend.Naziv == StringVozilo[0]).FirstOrDefault();
            }
            //0-Brend 1-VoziloNaziv

            //Provjeriti jel vozilo rezervisano
            //RijesitiException
            var idP = db.TrenutnaPoslovnica.FirstOrDefault(t => t.VoziloID == _Vozilo.VoziloID).PoslovnicaID;
            VozilaVM.Row vr = new VozilaVM.Row
            {
                Brend = db.Brend.FirstOrDefault(z => z.BrendID == _Vozilo.BrendID).Naziv,
                Naziv = _Vozilo.Naziv,
                Cijena = _Vozilo.Cijena,
                PoslovnicaNaziv = db.Poslovnica.FirstOrDefault(x => x.PoslovnicaID == idP).Naziv,
                PoslovnicaLokacija = db.Poslovnica.Where(x => x.PoslovnicaID == idP).Select(y => y.Grad.Naziv).FirstOrDefault(),
                Slika = db.Slika.FirstOrDefault(y => y.VoziloID == _Vozilo.VoziloID && y.Pozicija == 1)
            };
            model.VoziloVM = vr;
            model.DatumPreuzimanja= new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day);
            model.DatumPovrata = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1);
            return View(nameof(Rezervacija), model);
        }

        [ValidateAntiForgeryToken]
        public IActionResult NapraviRezervaciju(RezervacijaDodajVM model)
        {
            if (!ModelState.IsValid)
            {
                if (model.Vozilo == null || model.Vozilo == "")
                {
                    return Redirect("~/Vozilo");
                }
                string[] StringVozilo = model.Vozilo.Split('-');

                Vozilo _Vozilo = db.Vozilo.Where(x => x.Naziv == StringVozilo[1] && x.Brend.Naziv == StringVozilo[0]).FirstOrDefault();

                var idP = db.TrenutnaPoslovnica.FirstOrDefault(t => t.VoziloID == _Vozilo.VoziloID).PoslovnicaID;
                VozilaVM.Row vr = new VozilaVM.Row
                {
                    Brend = db.Brend.FirstOrDefault(z => z.BrendID == _Vozilo.BrendID).Naziv,
                    Naziv = _Vozilo.Naziv,
                    Cijena = _Vozilo.Cijena,
                    PoslovnicaNaziv = db.Poslovnica.FirstOrDefault(x => x.PoslovnicaID == idP).Naziv,
                    PoslovnicaLokacija = db.Poslovnica.Where(x => x.PoslovnicaID == idP).Select(y => y.Grad.Naziv).FirstOrDefault(),
                    Slika = db.Slika.FirstOrDefault(y => y.VoziloID == _Vozilo.VoziloID && y.Pozicija == 1)
                };
                model.VoziloVM = vr;
                return View(nameof(Rezervacija), model);
            }

            string SifraRezervacije = null;
            try
            {
                SifraRezervacije = Request.Cookies["Sesion"];
            }
            catch (Exception)
            {

                throw;
            }


            if (SifraRezervacije == null)
            {

                string[] StringVozilo = model.Vozilo.Split('-');

                int voziloID = db.Vozilo.Where(x => x.Naziv == StringVozilo[1] && x.Brend.Naziv == StringVozilo[0]).Select(y => y.VoziloID).FirstOrDefault();
                int god = DateTime.Now.Year;
                int KlijentID = int.Parse(_signInManager.GetUserId(User));
                int pID = db.TrenutnaPoslovnica.Where(x => x.VoziloID == voziloID).FirstOrDefault().PoslovnicaID;
                double c = db.Vozilo.Where(vo => vo.VoziloID == voziloID).FirstOrDefault().Cijena;



                Rezervacija r = new Rezervacija
                {
                    VoziloID = voziloID,
                    PoslovnicaID = pID,
                    DatumRezervacije = DateTime.Now,
                    DatumPovrata = model.DatumPovrata,
                    DatumPreuzimanja =  model.DatumPreuzimanja,
                    Zakljucen = (int)InfoRezervacija.U_Obradi,
                    Cijena = c * (model.DatumPovrata - model.DatumPreuzimanja).Days,
                    KlijentID = KlijentID,
                    BrojDanaIznajmljivanja = (model.DatumPovrata - model.DatumPreuzimanja).Days,
                    NacinPlacanja = (int)NacinPlacanja.NijeDefinisano,
                    UspjesnoSpremljena = false
                };

                //kreiranje kukija za rezervaciju
                CookieOptions option = new CookieOptions();
                var ticks = DateTime.Now.Ticks;
                var guid = Guid.NewGuid().ToString();
                var uniqueSessionId = ticks.ToString() + '-' + guid;
                option.Expires = DateTime.Now.AddHours(24);
                Response.Cookies.Append("Sesion", uniqueSessionId, option);

                r.SifraRezervacije = uniqueSessionId;

                db.Add(r);
                db.SaveChanges();
            }

            return Redirect(nameof(DodatneUsluge));
            //return View(nameof(DodatneUslugePV));
        }

        public IActionResult DodatneUsluge()
        {
            
            DodatneUslugeDodajVM dodatneUslugeDodajVM = new DodatneUslugeDodajVM();
            List<DodatneUsluge> DodatneUsluge = db.DodatneUsluge.ToList();

            //PreuzimanjeSesijeID
            string SifraRezervacije = null;

            try
            {
                SifraRezervacije = Request.Cookies["Sesion"];
            }
            catch (Exception)
            {

                throw;
            }
            if (SifraRezervacije == null)
            {
                //redirect na pocetnu str ili ispisati nesto
            }
            List<RezervisanaUsluga> RezervisaneUslugeFromDB = db.RezervisanaUsluga.Where(x=>x.Rezervacija.SifraRezervacije == SifraRezervacije).ToList();


            //Preuzeti vozilo iz rezervacije
            int VoziloID = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).Select(y => y.VoziloID).FirstOrDefault();


            bool kuka = db.Vozilo.FirstOrDefault(v => v.VoziloID == VoziloID).Kuka;
            dodatneUslugeDodajVM.Kuka = kuka;
            //provjera da li rezervacija ima sprmeljenu prikolicu
            int? idPrikolice = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).Select(y => y.PrikolicaID).FirstOrDefault();
            dodatneUslugeDodajVM.DodajPrikolicu = idPrikolice == null ? false : true;
            dodatneUslugeDodajVM.rows = new List<DodatneUslugeDodajVM.UslugeVm>();

            for (int i = 0; i < DodatneUsluge.Count; i++)
            {
                bool selektovano = RezervisaneUslugeFromDB.Any(ru => ru.DodatneUslugeID == DodatneUsluge[i].DodatneUslugeID);
                DodatneUslugeDodajVM.UslugeVm x = new DodatneUslugeDodajVM.UslugeVm
                {
                    Selected = selektovano,
                    Cijena = DodatneUsluge[i].Cijena,
                    Naziv = DodatneUsluge[i].Naziv,
                    Opis = DodatneUsluge[i].Opis
                };
                dodatneUslugeDodajVM.rows.Add(x);
            }

            return View(nameof(DodatneUsluge), dodatneUslugeDodajVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DodatneUslugeDodaj(DodatneUslugeDodajVM model)
        {
            string SifraRezervacije = null;

            try
            {
                SifraRezervacije = Request.Cookies["Sesion"];
            }
            catch (Exception)
            {

                throw;
            }
            if (SifraRezervacije == null)
            {
                //redirect na pocetnu str ili ispisati nesto
            }
            

            int RezervacijaID = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).Select(y => y.RezervacijaID).FirstOrDefault();

            List<RezervisanaUsluga> rezervisanaUslugaFromDB = db.RezervisanaUsluga.Where(x => x.RezervacijaID == RezervacijaID).ToList();



            List<RezervisanaUsluga> rezervisanaUslugaZaDodati = new List<RezervisanaUsluga>();
            List<RezervisanaUsluga> rezervisanaUslugaZaObrisati = new List<RezervisanaUsluga>();
            for (int i = 0; i < model.rows.Count; i++)
            {
                
                    try
                    {
                        
                        double cijena = (db.DodatneUsluge.Where(du => du.Naziv == model.rows[i].Naziv).FirstOrDefault().Cijena) * model.rows[i].Kolicina;
                        RezervisanaUsluga r = new RezervisanaUsluga
                        {
                            Kolicina = model.rows[i].Kolicina,
                            DodatneUslugeID = db.DodatneUsluge.Where(x => x.Naziv == model.rows[i].Naziv).Select(y => y.DodatneUslugeID).FirstOrDefault(),
                            UkupnaCijenaUsluge = cijena,
                            RezervacijaID = RezervacijaID
                        };
                        if (!rezervisanaUslugaFromDB.Any(p => p.DodatneUslugeID == r.DodatneUslugeID) && model.rows[i].Selected)
                        {
                        rezervisanaUslugaZaDodati.Add(r);
                        }else if(rezervisanaUslugaFromDB.Any(p => p.DodatneUslugeID == r.DodatneUslugeID) && model.rows[i].Selected == false)
                        {
                            RezervisanaUsluga rv = db.RezervisanaUsluga.Where(c => c.DodatneUslugeID == r.DodatneUslugeID && c.RezervacijaID == r.RezervacijaID).FirstOrDefault();
                            rezervisanaUslugaZaObrisati.Add(rv);
                        }
                       
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                  
               
            }

            db.RemoveRange(rezervisanaUslugaZaObrisati);
            db.AddRange(rezervisanaUslugaZaDodati);

            db.SaveChanges();



            //povlcenje prikolica
            if (model.DodajPrikolicu)
            {
                return RedirectPermanent(nameof(Prikolice));
            }
            else
            {
                int? idPrikolice = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).Select(y => y.PrikolicaID).FirstOrDefault();
                bool p = idPrikolice == null ? false : true;
                if (p)
                {
                    double cijenaPrikolice = db.Prikolica.Where(pr => pr.PrikolicaID == idPrikolice).FirstOrDefault().Cijna;
                    Rezervacija Rezervacija = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
                    Rezervacija.Cijena -= cijenaPrikolice;
                    Rezervacija.PrikolicaID = null;
                    db.SaveChanges();
                }
            }

            return RedirectPermanent(nameof(Placanje));
        }

        public IActionResult Prikolice()
        {

            string SifraRezervacije = null;

            try
            {
                SifraRezervacije = Request.Cookies["Sesion"];
            }
            catch (Exception)
            {

                throw;
            }
            if (SifraRezervacije == null)
            {
                //redirect na pocetnu str ili ispisati nesto
            }
            //Preuzeti vozilo iz rezervacije
            int RezervacijaID = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).Select(y => y.RezervacijaID).FirstOrDefault();


            int voziloID = db.Rezervacija.Where(rez => rez.SifraRezervacije == SifraRezervacije).FirstOrDefault().VoziloID;

            List<KompatibilnostPrikolica> kompatibilnostPrikolicas = db.KompatibilnostPrikolica.Where(kp => kp.VoziloID == voziloID).ToList();
            List<Prikolica> prikolice = new List<Prikolica>();
            for (int i = 0; i < kompatibilnostPrikolicas.Count; i++)
            {
                Prikolica p = db.Prikolica.Where(pr => pr.PrikolicaID == kompatibilnostPrikolicas[i].PrikolicaID).FirstOrDefault();
                prikolice.Add(p);
            }

            PrikoliceDodajVM prikoliceDodajVM = new PrikoliceDodajVM();
            prikoliceDodajVM.rows = new List<PrikoliceDodajVM.PrikoliceVm>();
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

            return View("Prikolice", prikoliceDodajVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PrikoliceSpremi(PrikoliceDodajVM model)
        {
            string SifraRezervacije = null;

            try
            {
                SifraRezervacije = Request.Cookies["Sesion"];
            }
            catch (Exception)
            {

                throw;
            }
            if (SifraRezervacije == null)
            {
                //redirect na pocetnu str ili ispisati nesto
            }

            Rezervacija r = db.Rezervacija.Where(x=> x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
            double cijenaRezervacijeOld = r.Cijena;
            int brojDanaRezervacije= r.BrojDanaIznajmljivanja;
            if (model.Selected != null && model.Selected != "" && int.Parse(model.Selected )>0)
            {
                if (r.PrikolicaID == null)
                {

                double cijenaPrikolice = db.Prikolica.Where(p => p.PrikolicaID == int.Parse(model.Selected)).FirstOrDefault().Cijna;
                r.PrikolicaID = int.Parse(model.Selected);
                r.Cijena = cijenaRezervacijeOld + cijenaPrikolice;
                db.SaveChanges();
                    return RedirectPermanent(nameof(Placanje));


                }
                else if (r.PrikolicaID != int.Parse(model.Selected))
                {
                    //oduzimanje cijene rezervacije sa cijenom spremljene prikolice
                    double cijenaPrikolice = db.Prikolica.Where(p => p.PrikolicaID == r.PrikolicaID).FirstOrDefault().Cijna;
                    r.PrikolicaID = null;
                    r.Cijena = cijenaRezervacijeOld - cijenaPrikolice;

                    //dodavanje cijene rezervacije sa cijenom nove prikolice
                    cijenaPrikolice = db.Prikolica.Where(p => p.PrikolicaID == int.Parse(model.Selected)).FirstOrDefault().Cijna;
                    r.PrikolicaID = int.Parse(model.Selected);
                    r.Cijena = r.Cijena + cijenaPrikolice;
                    db.SaveChanges();
                    return RedirectPermanent(nameof(Placanje));

                }
            }
            else
            {
                if (r.PrikolicaID != null)
                {
                    double cijenaPrikolice = db.Prikolica.Where(p => p.PrikolicaID == r.PrikolicaID).FirstOrDefault().Cijna;
                    r.PrikolicaID = null;
                    r.Cijena = cijenaRezervacijeOld - cijenaPrikolice;
                    db.SaveChanges();

                }

            }

           
            return RedirectPermanent(nameof(Placanje));
        }

        public IActionResult Placanje()
        {

            return View(nameof(Placanje));
        }

        [HttpPost]
        public IActionResult DodajPlacanjeSpremi(DodajPlacanjeSpremiPV model)
        {
            string SifraRezervacije = null;

            try
            {
                SifraRezervacije = Request.Cookies["Sesion"];
            }
            catch (Exception)
            {

                throw;
            }
            if (SifraRezervacije == null)
            {
                //redirect na pocetnu str ili ispisati nesto
            }
            Rezervacija r = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
            if (model.NacinPlacanja != null && model.NacinPlacanja != "")
            {
                var NP = Enum.GetNames(typeof(NacinPlacanja));
           
                r.NacinPlacanja = int.Parse(model.NacinPlacanja);
                db.SaveChanges();
            }
            return Redirect(nameof(RezervacijaDetaljno));
        }

        public IActionResult ZavrsiRezervaciju()
        {
            string SifraRezervacije = null;

            try
            {
                SifraRezervacije = Request.Cookies["Sesion"];
            }
            catch (Exception)
            {

                throw;
            }
            if (SifraRezervacije == null)
            {
                //redirect na pocetnu str ili ispisati nesto
            }
            Rezervacija r = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
            r.UspjesnoSpremljena = true;
            int x1 = r.PrikolicaID ?? 0;
            r.SifraRezervacije = DateTime.Now.Day + "" + DateTime.Now.Month + "-" + r.PoslovnicaID + "" + r.VoziloID + "" + r.RezervacijaID + "" + x1 + "/" + DateTime.Now.Year;
            db.SaveChanges();
            Response.Cookies.Delete("Sesion");
            TrenutnaPoslovnica tp = db.TrenutnaPoslovnica.Where(TP => TP.VoziloID == r.VoziloID).FirstOrDefault();
            tp.VoziloRezervisano = true;
            db.SaveChanges();
            return RedirectPermanent("/Klijent/Rezervacija/Index");
        }

        public IActionResult RezervacijaDetaljno()
        {
            string SifraRezervacije = null;

            try
            {
                SifraRezervacije = Request.Cookies["Sesion"];
            }
            catch (Exception)
            {

                throw;
            }
            if (SifraRezervacije == null)
            {
                //redirect na pocetnu str ili ispisati nesto
            }
            int KlijentID = int.Parse(_signInManager.GetUserId(User));
            int kFromRezervaijca = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault().KlijentID;
            if (KlijentID == kFromRezervaijca)
            {
                RezervacijaDetaljnoVM model = db.Rezervacija.Where(z => z.SifraRezervacije == SifraRezervacije).Select(x => new RezervacijaDetaljnoVM
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

        private int dateDiff(DateTime datumPreuzimanja, DateTime datumPovrata)
        {
            TimeSpan t = datumPovrata - datumPreuzimanja;
            return t.Days;
        }
    }
}