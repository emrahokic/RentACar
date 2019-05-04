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
        private UserManager<ApplicationUser> _signInManager;
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
            RezervacijaIndexVM model = new RezervacijaIndexVM()
            {
                Rows = db.Rezervacija.Where(z => z.KlijentID == KlijentID && z.UspjesnoSpremljena == true).OrderByDescending(c => c.DatumRezervacije).Select(x => new RezervacijaIndexVM.Row()
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
                model.Vozilo = StringVozilo[0] + "-" + StringVozilo[1];

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
            model.DatumPreuzimanja = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            model.DatumPovrata = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1);
            return View(nameof(Rezervacija), model);
        }

        [ValidateAntiForgeryToken]
        public IActionResult NapraviRezervaciju(RezervacijaDodajVM model)
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

            if (!ModelState.IsValid)
            {
                Vozilo _Vozilo = null;
                if (SifraRezervacije != null)
                {
                    string[] stringVozilo = new string[2];
                    Rezervacija r = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
                    int idV = r.VoziloID;
                    _Vozilo = db.Vozilo.Where(x => x.VoziloID == idV).FirstOrDefault();

                    stringVozilo[0] = db.Vozilo.Where(x => x.VoziloID == idV).Select(y => y.Brend.Naziv).FirstOrDefault();
                    stringVozilo[1] = _Vozilo.Naziv;
                    model.Vozilo = stringVozilo[0] + "-" + stringVozilo[1];

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

                if (model.Vozilo == null || model.Vozilo == "")
                {
                    return Redirect("~/Vozilo");
                }
                string[] StringVozilo = model.Vozilo.Split('-');

                _Vozilo = db.Vozilo.Where(x => x.Naziv == StringVozilo[1] && x.Brend.Naziv == StringVozilo[0]).FirstOrDefault();

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




            if (SifraRezervacije == null)
            {

                string[] StringVozilo = model.Vozilo.Split('-');

                int voziloID = db.Vozilo.Where(x => x.Naziv == StringVozilo[1] && x.Brend.Naziv == StringVozilo[0]).Select(y => y.VoziloID).FirstOrDefault();
                int god = DateTime.Now.Year;
                int KlijentID = int.Parse(_signInManager.GetUserId(User));
                int pID = db.TrenutnaPoslovnica.Where(x => x.VoziloID == voziloID).FirstOrDefault().PoslovnicaID;
                double c = db.Vozilo.Where(vo => vo.VoziloID == voziloID).FirstOrDefault().Cijena;


                int brDanaIznajmljivanja = dateDiff(model.DatumPreuzimanja, model.DatumPovrata);
                Rezervacija r = new Rezervacija
                {
                    VoziloID = voziloID,
                    PoslovnicaID = pID,
                    DatumRezervacije = DateTime.Now,
                    DatumPovrata = model.DatumPovrata,
                    DatumPreuzimanja = model.DatumPreuzimanja,
                    Zakljucen = (int)InfoRezervacija.U_Obradi,
                    Cijena = c * brDanaIznajmljivanja,
                    KlijentID = KlijentID,
                    BrojDanaIznajmljivanja = brDanaIznajmljivanja,
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
                return Redirect(nameof(DodatneUsluge));

            }

            Rezervacija rez = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
            double cijenVozilaPoDanu = db.Vozilo.Where(vo => vo.VoziloID == rez.VoziloID).FirstOrDefault().Cijena;

            rez.DatumPreuzimanja = model.DatumPreuzimanja;
            rez.DatumPovrata = model.DatumPovrata;
            int brDana = (model.DatumPovrata - model.DatumPreuzimanja).Days == 0 ? 1 : (model.DatumPovrata - model.DatumPreuzimanja).Days;
            rez.BrojDanaIznajmljivanja = brDana;
            rez.Cijena = cijenVozilaPoDanu * brDana;
            db.SaveChanges();

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
            List<RezervisanaUsluga> RezervisaneUslugeFromDB = db.RezervisanaUsluga.Where(x => x.Rezervacija.SifraRezervacije == SifraRezervacije).ToList();


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
            Rezervacija rezervacija = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
            List<RezervisanaUsluga> rezervisanaUslugaFromDB = db.RezervisanaUsluga.Where(x => x.RezervacijaID == RezervacijaID).ToList();

            if (!ModelState.IsValid)
            {
                return Redirect(nameof(DodatneUsluge));

            }

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
                        rezervacija.Cijena += r.UkupnaCijenaUsluge;
                        db.SaveChanges();

                    }
                    else if (rezervisanaUslugaFromDB.Any(p => p.DodatneUslugeID == r.DodatneUslugeID) && model.rows[i].Selected == false)
                    {
                        RezervisanaUsluga rv = db.RezervisanaUsluga.Where(c => c.DodatneUslugeID == r.DodatneUslugeID && c.RezervacijaID == r.RezervacijaID).FirstOrDefault();
                        rezervisanaUslugaZaObrisati.Add(rv);
                        rezervacija.Cijena -= r.UkupnaCijenaUsluge;
                        db.SaveChanges();

                    }
                    else if (rezervisanaUslugaFromDB.Any(p => p.DodatneUslugeID == r.DodatneUslugeID) && model.rows[i].Selected == true)
                    {
                        RezervisanaUsluga RUforUpdate = db.RezervisanaUsluga.Where(x => x.DodatneUslugeID == r.DodatneUslugeID && x.RezervacijaID == r.RezervacijaID).FirstOrDefault();
                        rezervacija.Cijena -= RUforUpdate.UkupnaCijenaUsluge;

                        RUforUpdate.Kolicina = r.Kolicina;
                        RUforUpdate.UkupnaCijenaUsluge = r.UkupnaCijenaUsluge;
                        rezervacija.Cijena += RUforUpdate.UkupnaCijenaUsluge;

                        db.SaveChanges();
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

            Rezervacija r = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
            double cijenaRezervacijeOld = r.Cijena;
            int brojDanaRezervacije = r.BrojDanaIznajmljivanja;
            if (model.Selected != null && model.Selected != "" && int.Parse(model.Selected) > 0)
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
            if (!ModelState.IsValid)
            {
                return View(nameof(Placanje));

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

        public IActionResult PonistiRezervaciju(int? id)
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
            if (SifraRezervacije == null && id == null)
            {
                return RedirectPermanent("/Klijent/Rezervacija");
            }

            if (id != null)
            {
                //redirect na pocetnu str ili ispisati nesto
                Rezervacija r1 = db.Rezervacija.Where(x => x.RezervacijaID == id).FirstOrDefault();
                List<RezervisanaUsluga> ru1 = db.RezervisanaUsluga.Where(x => x.RezervacijaID == r1.RezervacijaID).ToList();
                OcjenaRezervacija ocjena1 = db.OcjenaRezervacija.Where(x => x.RezervacijaID == r1.RezervacijaID).FirstOrDefault();
                TrenutnaPoslovnica tp1 = db.TrenutnaPoslovnica.Where(x => x.VoziloID == r1.VoziloID && x.PoslovnicaID == r1.PoslovnicaID && x.VoziloRezervisano == true).FirstOrDefault();
                Vozilo v = db.Vozilo.Where(x => x.VoziloID == r1.VoziloID).FirstOrDefault();


                List<Notifikacija> noti = db.Notifikacija.Where(x => x.RezervacijaID == r1.RezervacijaID).ToList();
                if (noti != null)
                {
                    noti.Select(c => { c.RezervacijaID = null; return c; });
                }

                if (tp1 != null)
                {
                    tp1.VoziloRezervisano = false;
                }
                if (ocjena1 != null)
                {
                    db.Remove(ocjena1);
                }
                if (ru1 != null)
                {
                    db.RemoveRange(ru1);
                }
                if (r1 != null)
                {
                    db.Remove(r1);
                }
                Notifikacija notifikacijaZaKlijenta = new Notifikacija
                {
                    Vrijeme = DateTime.Now,
                    Otvorena = false,
                    Poruka = "Vasa rezervacija za vozilo "+v.Naziv+" je uspjesno obrisana!",
                    PoslovnicaID = null,
                    UserID = r1.KlijentID,
                    RezervacijaID = null
                };
                Notifikacija notifikacijaZaUposlenika = new Notifikacija
                {
                    Vrijeme = DateTime.Now,
                    Otvorena = false,
                    Poruka = User.Identity.Name+ " je obrisao svoju rezervaciju za vozilo: "+v.Naziv + " !",
                    PoslovnicaID = r1.PoslovnicaID,
                    UserID = null,
                    RezervacijaID = null
                };
                List<Notifikacija> n = new List<Notifikacija>();
                n.Add(notifikacijaZaKlijenta);
                n.Add(notifikacijaZaUposlenika);
                db.AddRange(n);
                db.SaveChanges();

                return RedirectPermanent("/Klijent/Rezervacija");
            }

            Rezervacija r = db.Rezervacija.Where(x => x.SifraRezervacije == SifraRezervacije).FirstOrDefault();
            List<RezervisanaUsluga> ru = db.RezervisanaUsluga.Where(x => x.RezervacijaID == r.RezervacijaID).ToList();
            OcjenaRezervacija ocjena = db.OcjenaRezervacija.Where(x => x.RezervacijaID == r.RezervacijaID).FirstOrDefault();
            TrenutnaPoslovnica tp = db.TrenutnaPoslovnica.Where(x => x.VoziloID == r.VoziloID && x.PoslovnicaID == r.PoslovnicaID && x.VoziloRezervisano == true).FirstOrDefault();
            if (tp != null)
            {
                tp.VoziloRezervisano = false;
            }
            if (ocjena != null)
            {
                db.Remove(ocjena);
            }
            if (ru != null)
            {
                db.RemoveRange(ru);
            }
            if (r != null)
            {
                db.Remove(r);
            }
            Response.Cookies.Delete("Sesion");
            db.SaveChanges();

            return RedirectPermanent("/Vozilo");
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
            Vozilo v = db.Vozilo.Where(x => x.VoziloID == r.VoziloID).FirstOrDefault();

            int x1 = r.PrikolicaID ?? 0;
            r.SifraRezervacije = DateTime.Now.Day + "" + DateTime.Now.Month + "-" + r.PoslovnicaID + "" + r.VoziloID + "" + r.RezervacijaID + "" + x1 + "/" + DateTime.Now.Year;
            db.SaveChanges();
            Response.Cookies.Delete("Sesion");
            TrenutnaPoslovnica tp = db.TrenutnaPoslovnica.Where(TP => TP.VoziloID == r.VoziloID).FirstOrDefault();
            tp.VoziloRezervisano = true;
            db.SaveChanges();

            Notifikacija notifikacijaZaKlijenta = new Notifikacija
            {
                Vrijeme = DateTime.Now,
                Otvorena = false,
                Poruka = "Vasa rezervacija za vozilo " + v.Naziv + " je zaprimljena u poslovnicu i trenutno je na obradi!",
                PoslovnicaID = null,
                UserID = r.KlijentID,
                RezervacijaID = r.RezervacijaID
            };
            Notifikacija notifikacijaZaUposlenika = new Notifikacija
            {
                Vrijeme = DateTime.Now,
                Otvorena = false,
                Poruka = "Nova rezervacija za vozilo " + v.Naziv + " od " + User.Identity.Name,
                PoslovnicaID = r.PoslovnicaID,
                UserID = null,
                RezervacijaID = r.RezervacijaID
            };
            List<Notifikacija> n = new List<Notifikacija>();
            n.Add(notifikacijaZaKlijenta);
            n.Add(notifikacijaZaUposlenika);
            db.AddRange(n);
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
                    DatumRezervacije = x.DatumRezervacije,
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
            if (Ocjena >= 0 && Poruka != null && RezervacijaID > 0)
            {

                int KlijentID = int.Parse(_signInManager.GetUserId(User));
                Rezervacija tempR = db.Rezervacija.Find(RezervacijaID);
                ApplicationUser tempK = db.Users.Find(KlijentID);
                OcjenaRezervacija rezervacija = new OcjenaRezervacija()
                {
                    Poruka = Poruka,
                    OcjenaVrijednost = (int)Ocjena,
                    Rezervacija = tempR,
                    DatumOcjene = DateTime.Now,
                    Klijent = tempK
                };
                db.OcjenaRezervacija.Add(rezervacija);
                db.SaveChanges();
                return PartialView("OcjenaKomentar", rezervacija);
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
                DatumRezervacije = x.DatumRezervacije,
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

            return View(nameof(Detalji), model);
        }


        //Za izmjene Rezervacije
        public IActionResult RezervacijaUredi(int id)
        {
            var model = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s => new RezervacijaUrediVM
            {
                RezervacijaID = id,
                DatumPreuzimanja = s.DatumPreuzimanja,
                DatumRezervacije = s.DatumRezervacije,
                DatumPovrata = s.DatumPovrata,
                Zakljucen = s.Zakljucen,
                Cijena = s.Cijena,
                BrojDanaIznajmljivanja = s.BrojDanaIznajmljivanja,
                NacinPlacanja = s.NacinPlacanja,
                SlikaVozila = db.Slika.Where(x => s.VoziloID == x.VoziloID && x.Pozicija == 1).Select(z => z.URL).SingleOrDefault(),
                Vozilo = s.Vozilo.Naziv,
                Ime = s.Klijent.Ime,
                Prezime = s.Klijent.Prezime,
                DatumRodjenja = s.Klijent.DatumRodjenja,
                Adresa = s.Klijent.Adresa,
                Spol = s.Klijent.Spol,
                jmbg = s.Klijent.JMBG,
                PrikolicaID = s.PrikolicaID
            }).SingleOrDefault();

            model.zakljuceni = Enum.GetValues(typeof(InfoRezervacija)).Cast<InfoRezervacija>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            model.naciniPlacanja = Enum.GetValues(typeof(NacinPlacanja)).Cast<NacinPlacanja>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            int voziloID = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s => s.VoziloID).SingleOrDefault();
            var temp = db.KompatibilnostPrikolica.Where(p => p.VoziloID == voziloID).Select(s => s).ToList();
            if (temp.Count() != 0)
            {
                model.imaPrikolicu = true;
                model.prikolice = new List<SelectListItem>();
                model.prikolice = temp.Select(s => new SelectListItem
                {
                    Value = s.PrikolicaID.ToString(),
                    Text = "Tip kuke: " + s.TipKuke + " - " + db.Prikolica.Where(pri => pri.PrikolicaID == s.PrikolicaID).Select(x => "Cijena: " + x.Cijna.ToString() + " KM - Duzina: " + x.Duzina.ToString() + " - Sirina: " + x.Sirina.ToString()).SingleOrDefault().ToString()
                }).ToList();
            }
            else
            {
                model.imaPrikolicu = false;
            }

            return View(nameof(RezervacijaUredi), model);
        }

        public IActionResult RezervacijaUrediSnimi(int Prikolica, int RezervacijaID, string DatumPreuzimanja, string DatumPovrata, int NacinPlacanja)
        {
            //pronadji rezervaciju i smjesti je u temp
            var temp = db.Rezervacija.Where(r => r.RezervacijaID == RezervacijaID).Select(s => s).SingleOrDefault();

            double cijenaVozilaDan = db.Vozilo.Where(v => v.VoziloID == temp.VoziloID).Select(s => s.Cijena).SingleOrDefault();
            double ostatak = temp.Cijena - (temp.BrojDanaIznajmljivanja * cijenaVozilaDan);

            temp.DatumPreuzimanja = DateTime.Parse(DatumPreuzimanja);
            temp.DatumPovrata = DateTime.Parse(DatumPovrata);
            temp.NacinPlacanja = NacinPlacanja;

          

            int brojDana = Convert.ToInt32((DateTime.Parse(DatumPovrata) - DateTime.Parse(DatumPreuzimanja)).TotalDays) == 0 ? 1: Convert.ToInt32((DateTime.Parse(DatumPovrata) - DateTime.Parse(DatumPreuzimanja)).TotalDays);
            temp.BrojDanaIznajmljivanja = brojDana;
            temp.Cijena = (cijenaVozilaDan * brojDana) + ostatak;

            //update prikolice
            if (Prikolica != 0 && Prikolica != -1)
            {
                temp.PrikolicaID = Prikolica;
            }
            else if (Prikolica == -1)
            {
                if (temp.PrikolicaID != null)
                {
                    double c = db.Prikolica.Where(pri => pri.PrikolicaID == temp.PrikolicaID).Select(cij => cij.Cijna).FirstOrDefault();
                    temp.Cijena -= c;
                }
                temp.PrikolicaID = null;

            }
            db.SaveChanges();
            db.Dispose();

            return RedirectToAction(nameof(DodatneUslugeUrediREZ), new { id = RezervacijaID });
        }

        public IActionResult DodatneUslugeUrediREZ(int id)
        {
            RezervacijaUslugeUrediVM model = new RezervacijaUslugeUrediVM
            {
                RezervacijaID = id,
                rows = db.RezervisanaUsluga.Where(u => u.RezervacijaID == id).Select(s =>
                new RezervacijaUslugeUrediVM.Row
                {
                    UslugaID = s.DodatneUslugeID,
                    Kolicina = s.Kolicina,
                    Ukupno = s.UkupnaCijenaUsluge,
                    Naziv = db.DodatneUsluge.Where(d => d.DodatneUslugeID == s.DodatneUslugeID).Select(x => x.Naziv).SingleOrDefault(),
                    Cijena = db.DodatneUsluge.Where(d => d.DodatneUslugeID == s.DodatneUslugeID).Select(x => x.Cijena).SingleOrDefault(),
                    Opis = db.DodatneUsluge.Where(d => d.DodatneUslugeID == s.DodatneUslugeID).Select(x => x.Opis).SingleOrDefault()
                }).ToList()
            };

            return View(nameof(DodatneUslugeUrediREZ), model);
        }
        public IActionResult DodatneUslugeUrediREZobrisi(int uslugaID, int rezervacijaID)
        {
            var temp = db.RezervisanaUsluga.Where(u => u.RezervacijaID == rezervacijaID && u.DodatneUslugeID == uslugaID).Select(s => s).SingleOrDefault();

            //umanji cijenu rezervacije za obrisanu uslugu
            var rezervacija = db.Rezervacija.Where(r => r.RezervacijaID == rezervacijaID).Select(s => s).SingleOrDefault();
            double ukupno = temp.UkupnaCijenaUsluge;
            rezervacija.Cijena -= ukupno;

            db.Remove(temp);
            db.SaveChanges();
            db.Dispose();

            return RedirectToAction(nameof(DodatneUslugeUrediREZ), new { id = rezervacijaID });
        }
        public IActionResult RezervacijaUslugeUredi(int id)
        {
            var model = new RezervacijaUslugeVM();
            model.RezervacijaID = id;

            var rezervisane = db.RezervisanaUsluga.Where(p => p.RezervacijaID == id).Select(v => v.DodatneUsluge);
            var usluge = db.DodatneUsluge.Select(x => x);
            var rez = usluge.Where(p1 => rezervisane.All(p2 => p2.DodatneUslugeID != p1.DodatneUslugeID)).Select(p3 => p3);

            model.usluge = rez.Select(s => new SelectListItem
            {
                Value = s.DodatneUslugeID.ToString(),
                Text = s.Naziv + " - " + s.Cijena.ToString()
            }).ToList();


            return PartialView(nameof(RezervacijaUslugeUredi), model);
        }
        public IActionResult RezervacijaUslugeSnimi(int RezervacijaID, int Kolicina, int UslugaID)
        {
            UslugaSnimi(RezervacijaID, Kolicina, UslugaID);
            string route = "/Klijent/Rezervacija/DodajUslugeRezervaciji?id=" + RezervacijaID.ToString();
            return Redirect(route);
        }
        public IActionResult RezervacijaUslugeUrediSnimi(int RezervacijaID, int Kolicina, int UslugaID)
        {
            UslugaSnimi(RezervacijaID, Kolicina, UslugaID);
            string route = "/Klijent/Rezervacija/DodatneUslugeUrediREZ?id=" + RezervacijaID.ToString();
            return Redirect(route);
        }

        public IActionResult DodajUslugeRezervaciji(int id)
        {
            RezervacijaSredinaVM model = db.Rezervacija.Where(r => r.RezervacijaID == id).Select(s =>
            new RezervacijaSredinaVM
            {
                Korisnik = s.Klijent.Ime + " " + s.Klijent.Prezime,
                Vozilo = s.Vozilo.Naziv,
                DatumPreuzimanja = s.DatumPreuzimanja.ToShortDateString(),
                DatumPovrata = s.DatumPovrata.ToShortDateString(),
                Cijena = s.Cijena,
                RezervacijaID = id
            }).SingleOrDefault();
            model.dodatneUsluge = db.RezervisanaUsluga.Where(r => r.RezervacijaID == id).Select(s => new RezervacijaSredinaVM.Row
            {
                Cijena = s.DodatneUsluge.Cijena,
                Naziv = s.DodatneUsluge.Naziv,
                Opis = s.DodatneUsluge.Opis,
                Kolicina = s.Kolicina,
                Ukupno = s.UkupnaCijenaUsluge
            }).ToList();

            return View(nameof(DodajUslugeRezervaciji), model);
        }
        private void UslugaSnimi(int RezervacijaID, int Kolicina, int UslugaID)
        {
            double cijena = db.DodatneUsluge.Where(d => d.DodatneUslugeID == UslugaID).Select(s => s.Cijena).SingleOrDefault();
            double ukupno = Kolicina * cijena;
            RezervisanaUsluga nova = new RezervisanaUsluga
            {
                RezervacijaID = RezervacijaID,
                DodatneUslugeID = UslugaID,
                Kolicina = Kolicina,
                Opis = "Opis",
                UkupnaCijenaUsluge = ukupno
            };
            db.Add(nova);
            db.SaveChanges();
            var temp = db.Rezervacija.Where(r => r.RezervacijaID == RezervacijaID).Select(s => s).SingleOrDefault();
            temp.Cijena += ukupno;
            db.SaveChanges();
        }


        public int dateDiff(DateTime datumPreuzimanja, DateTime datumPovrata)
        {
            TimeSpan t = datumPovrata - datumPreuzimanja;
            return t.Days ==0 ? 1 : t.Days;
        }
    }
}