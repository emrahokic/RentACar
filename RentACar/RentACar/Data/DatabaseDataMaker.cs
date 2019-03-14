using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RentACar.Models;

namespace RentACar.Data
{
    /*******************************************************************************************************************************
     *
     * 
     * 
     *                                             DatabaseDataMaker Version = 1.0
     *                                          
     * 
     * 
     *******************************************************************************************************************************/
    public class DatabaseDataMaker
    {
        
        public static async Task Napuni(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, ILogger<DatabaseDataMaker> logger)
        {
            //context.Database.EnsureCreated(); 
            if (context.Prikolica.Any())
            {
                return; //baza je vec napunjena
            }


            var gradovi = new List<Grad>();
            var regije = new List<Regija>();
            var drzave = new List<Drzava>();

            var poslovnice = new List<Poslovnica>();
            var trenutnePoslovnice = new List<TrenutnaPoslovnica>();
            var ugovoriZaposlenja = new List<UgovorZaposlenja>();

            var rezervacije = new List<Rezervacija>();
            var ocjeneRezervacije = new List<OcjenaRezervacija>();
            var rezervisaneUsluge = new List<RezervisanaUsluga>();
            var dodatneUsluge = new List<DodatneUsluge>();

            var vozila = new List<Vozilo>();
            var brendovi = new List<Brend>();
            var slike = new List<Slika>();
            var kompatibilnostPrikolice = new List<KompatibilnostPrikolica>();
            var prikolice = new List<Prikolica>();
            var ostecenja = new List<OstecenjeInfo>();

            var prijevozi = new List<Prijevoz>();
            var uposleniciPrijevoz = new List<UposlenikPrijevoz>();
            var vozilaPrijevoz = new List<PrijevozVozilo>();
            var ocjenePrijevoz = new List<OcjenaPrijevoz>();

            var korisnici = new List<ApplicationUser>();

            //dodavanje gradova sa opcinama, regijama i drzavama
            drzave.Add(new Drzava { Naziv = "Bosna i Hercegovina", Skracenica = "BiH" });
            context.Drzava.AddRange(drzave);
            context.SaveChanges();

            var bih = context.Drzava.Select(s => s).SingleOrDefault(p => p.Naziv == "Bosna i Hercegovina");

            regije.Add(new Regija { Naziv = "Federacija BiH", Drzava = bih });
            regije.Add(new Regija { Naziv = "Republika Srpska", Drzava = bih });
            regije.Add(new Regija { Naziv = "Brcko Distrikt", Drzava = bih });
            context.Regija.AddRange(regije);
            context.SaveChanges();

            var fBiH = context.Regija.Select(s => s).SingleOrDefault(p => p.Naziv == "Federacija BiH");
            var rs = context.Regija.Select(s => s).SingleOrDefault(p => p.Naziv == "Republika Srpska");
            var br = context.Regija.Select(s => s).SingleOrDefault(p => p.Naziv == "Brcko Distrikt");
            gradovi.Add(new Grad { Naziv = "Sarajevo", Regija = fBiH });
            gradovi.Add(new Grad { Naziv = "Mostar", Regija = fBiH });
            gradovi.Add(new Grad { Naziv = "Velika Kladusa", Regija = fBiH });
            gradovi.Add(new Grad { Naziv = "Tuzla", Regija = fBiH });
            gradovi.Add(new Grad { Naziv = "Banja Luka", Regija = rs });
            gradovi.Add(new Grad { Naziv = "Brcko", Regija = br });
            context.AddRange(gradovi);
            context.SaveChanges();

            //dodatne usluge 
            dodatneUsluge.Add(new DodatneUsluge { Naziv = "Dodatna usluge 1", Cijena = 50, Opis = "Opis dodatne usluge 1" });
            dodatneUsluge.Add(new DodatneUsluge { Naziv = "Dodatna usluge 2", Cijena = 60, Opis = "Opis dodatne usluge 2" });
            dodatneUsluge.Add(new DodatneUsluge { Naziv = "Dodatna usluge 3", Cijena = 80, Opis = "Opis dodatne usluge 3" });
            dodatneUsluge.Add(new DodatneUsluge { Naziv = "Dodatna usluge 4", Cijena = 40, Opis = "Opis dodatne usluge 4" });
            context.AddRange(dodatneUsluge);
            context.SaveChanges();

            //dodavanje brendova 
            brendovi.Add(new Brend { Naziv = "BMW" });
            brendovi.Add(new Brend { Naziv = "Audi" });
            brendovi.Add(new Brend { Naziv = "Passat" });
            brendovi.Add(new Brend { Naziv = "Opel" });
            brendovi.Add(new Brend { Naziv = "Range Rover" });
            brendovi.Add(new Brend { Naziv = "Skoda" });
            context.AddRange(brendovi);
            context.SaveChanges();

            //dodavanje poslovnica 
            var Sarajevo = context.Grad.Select(s => s).SingleOrDefault(p => p.Naziv == "Sarajevo");
            poslovnice.Add(new Poslovnica { Naziv = "Poslovnica Aeodrom", Adresa = "Butimirska Cesta bb", BrojTelefona = "+387 33 111 111", Email = "sunnycars@sunnygroup.com", Grad = Sarajevo });
            poslovnice.Add(new Poslovnica { Naziv = "Poslovnica Carsija", Adresa = "Bascarsija bb", BrojTelefona = "+387 33 222 222", Email = "sunnycars@sunnygroup.com", Grad = Sarajevo });

            var Mostar = context.Grad.Select(s => s).SingleOrDefault(p => p.Naziv == "Mostar");
            poslovnice.Add(new Poslovnica { Naziv = "Poslovnica Mostar 1", Adresa = "Mostarska bb", BrojTelefona = "+387 35 333 333", Email = "sunnycars@sunnygroup.com", Grad = Mostar });
            context.AddRange(poslovnice);
            context.SaveChanges();

            //dodavanje prikolica 
            prikolice.Add(new Prikolica { Duzina = 250, Sirina = 180, TipPrikolice = "Velika", Zapremina = 200 });
            prikolice.Add(new Prikolica { Duzina = 220, Sirina = 175, TipPrikolice = "Srednja", Zapremina = 170 });
            prikolice.Add(new Prikolica { Duzina = 200, Sirina = 180, TipPrikolice = "Mala", Zapremina = 150 });
            prikolice.Add(new Prikolica { Duzina = 250, Sirina = 185, TipPrikolice = "Velika", Zapremina = 200 });
            prikolice.Add(new Prikolica { Duzina = 250, Sirina = 195, TipPrikolice = "Zatvorena velika", Zapremina = 220 });
            context.AddRange(prikolice);
            context.SaveChanges();

            //kreiranje usera admin, uposlenik, klijent
            string uposlenik = "Uposlenik";
            string klijent = "Klijent";
            string admin = "Administrator";
            string mehanicar = "Mehanicar";
            string vozac = "Vozac";

            await roleManager.CreateAsync(new IdentityRole<int> { Name = uposlenik, NormalizedName = uposlenik.ToUpper() });
            await roleManager.CreateAsync(new IdentityRole<int> { Name = klijent, NormalizedName = klijent.ToUpper() });
            await roleManager.CreateAsync(new IdentityRole<int> { Name = admin, NormalizedName = admin.ToUpper() });
            await roleManager.CreateAsync(new IdentityRole<int> { Name = mehanicar, NormalizedName = mehanicar.ToUpper() });
            await roleManager.CreateAsync(new IdentityRole<int> { Name = vozac, NormalizedName = vozac.ToUpper() });



            //admin
            await KreirajKorisnika(userManager, "johny@sunnycars.admin.com", "Johny", "Bravo", "Dzemala Bijedica bb", Sarajevo, "0133546579", new DateTime(1996, 5, 6), "M", "nema", "P@sword123", admin);

            //uposlenici
            await KreirajKorisnika(userManager, "sonny@sunnycars.uposlenik.com", "Sonny", "Lona", "Dzemala Bijedica bb", Sarajevo, "651951651", new DateTime(1995, 5, 6), "M", "nema", "P@sword123", uposlenik);
            await KreirajKorisnika(userManager, "dona@sunnycars.uposlenik.com", "Dona", "Gomez", "Dzemala Bijedica bb", Sarajevo, "56132184", new DateTime(1994, 5, 6), "Z", "nema", "P@sword123", uposlenik);
            await KreirajKorisnika(userManager, "jack@sunnycars.mehanicar.com", "Jack", "Olivero", "Dzemala Bijedica bb", Sarajevo, "56132184", new DateTime(1994, 5, 6), "Z", "nema", "P@sword123", mehanicar);
            await KreirajKorisnika(userManager, "mile@sunnycars.vozac.com", "Mile", "Dinastija", "Dzemala Bijedica bb", Sarajevo, "56132184", new DateTime(1994, 5, 6), "Z", "nema", "P@sword123", vozac);


            //klijenti
            await KreirajKorisnika(userManager, "adil@gmail.com", "Adil", "Joldic", "Dzemala Bijedica bb", Sarajevo, "654984651", new DateTime(1989, 5, 6), "M", "nema", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "nina@gmail.com", "Nina", "Bijedic", "Dzemala Bijedica bb", Sarajevo, "9498185", new DateTime(1975, 5, 6), "Z", "nema", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "denis@gmail.com", "Denis", "Music", "Dzemala Bijedica bb", Sarajevo, "6519549", new DateTime(1985, 5, 6), "M", "nema", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "emina@gmail.com", "Emina", "Junuz", "Dzemala Bijedica bb", Sarajevo, "959498", new DateTime(1987, 5, 6), "Z", "nema", "P@sword123", klijent);

            //dodavanje uposlenika poslovnicama
            var aeodrom = context.Poslovnica.Select(s => s).SingleOrDefault(x => x.Naziv == "Poslovnica Aeodrom");
            var sonny = userManager.FindByEmailAsync("sonny@sunnycars.uposlenik.com");
            var dona = userManager.FindByEmailAsync("dona@sunnycars.uposlenik.com");
            var johny = userManager.FindByEmailAsync("johny@sunnycars.admin.com");

            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = aeodrom, RadnoMjesto = "salter", Uposlenik = sonny.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = aeodrom, RadnoMjesto = "salter", Uposlenik = dona.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = aeodrom, RadnoMjesto = "kancelarija", Uposlenik = johny.Result });
            context.AddRange(ugovoriZaposlenja);
            context.SaveChanges();

            //dodavanje vozila 
            var audi = context.Brend.Select(s => s).SingleOrDefault(x => x.Naziv == "Audi");
            vozila.Add(new Vozilo
            {
                BrojSasije = 1324,
                RegistarskaOznaka = "123-A-654",
                Boja = "Bijela",
                Model = "A6",
                GodinaProizvodnje = 2016,
                SnagaMotora = 210,
                DatumMijenjanjUlja = DateTime.Now,
                Domet = 600,
                BrojMjesta = 5,
                BrojVrata = 4,
                ZapreminaPrtljaznika = 60,
                ZapreminaPrtljaznikaNaprijed = 0,
                Naziv = "Audi A6",
                Brend = audi,
                Klima = true,
                TipVozila = "Normal",
                DodatniOpis = "Bez dodatnog opisa",
                Gorivo = "Dizel",
                Pogon = "4x4",
                Transmisija = "Automatik",
                GrupniTipVozila = "Putnicko"
            });
            vozila.Add(new Vozilo
            {
                BrojSasije = 615654,
                RegistarskaOznaka = "455-A-644",
                Boja = "Crna",
                Model = "A4",
                GodinaProizvodnje = 2015,
                SnagaMotora = 150,
                DatumMijenjanjUlja = DateTime.Now,
                Domet = 450,
                BrojMjesta = 5,
                BrojVrata = 4,
                ZapreminaPrtljaznika = 70,
                ZapreminaPrtljaznikaNaprijed = 0,
                Naziv = "Audi A4",
                Brend = audi,
                Klima = true,
                TipVozila = "Normal",
                DodatniOpis = "Bez dodatnog opisa",
                Gorivo = "Dizel",
                Pogon = "Prednji",
                Transmisija = "Automatik",
                GrupniTipVozila = "Putnicko"
            });
            var bmw = context.Brend.Select(s => s).SingleOrDefault(x => x.Naziv == "BMW");
            vozila.Add(new Vozilo
            {
                BrojSasije = 654984,
                RegistarskaOznaka = "599-S-698",
                Boja = "Bijela",
                Model = "X6",
                GodinaProizvodnje = 2018,
                SnagaMotora = 340,
                DatumMijenjanjUlja = DateTime.Now,
                Domet = 550,
                BrojMjesta = 5,
                BrojVrata = 4,
                ZapreminaPrtljaznika = 120,
                ZapreminaPrtljaznikaNaprijed = 0,
                Naziv = "BMW X6",
                Brend = bmw,
                Klima = true,
                TipVozila = "Normal",
                DodatniOpis = "Bez dodatnog opisa",
                Gorivo = "Benzin",
                Pogon = "4x4",
                Transmisija = "Automatik",
                GrupniTipVozila = "Putnicko"
            });
            context.AddRange(vozila);
            context.SaveChanges();

            //dodavanje kompatiblnosti prikolica
            kompatibilnostPrikolice.Add(new KompatibilnostPrikolica
            {
                Prikolica = context.Prikolica.OrderBy(x => Guid.NewGuid()).FirstOrDefault(),
                Vozilo = context.Vozilo.OrderBy(x => Guid.NewGuid()).FirstOrDefault(),
                Tezina = "50",
                TipKuke = "Otvorena",
            });
            kompatibilnostPrikolice.Add(new KompatibilnostPrikolica
            {
                Prikolica = context.Prikolica.OrderBy(x => Guid.NewGuid()).FirstOrDefault(),
                Vozilo = context.Vozilo.OrderBy(x => Guid.NewGuid()).FirstOrDefault(),
                Tezina = "60",
                TipKuke = "Otvorena",
            });
            context.AddRange(kompatibilnostPrikolice);
            context.SaveChanges();

            //dodavanje vozila u trenutnu poslovnicu 
            var audia6 = context.Vozilo.Select(s => s).SingleOrDefault(x => x.Naziv == "Audi A6");
            var audia4 = context.Vozilo.Select(s => s).SingleOrDefault(x => x.Naziv == "Audi A4");
            var bmwx6 = context.Vozilo.Select(s => s).SingleOrDefault(x => x.Naziv == "BMW X6");
            trenutnePoslovnice.Add(new TrenutnaPoslovnica { DatumUlaza = DateTime.Now, DatumIzlaza = DateTime.Now, Poslovnica = aeodrom, Vozilo = audia4});
            trenutnePoslovnice.Add(new TrenutnaPoslovnica { DatumUlaza = DateTime.Now, DatumIzlaza = DateTime.Now, Poslovnica = aeodrom, Vozilo = audia6 });
            trenutnePoslovnice.Add(new TrenutnaPoslovnica { DatumUlaza = DateTime.Now, DatumIzlaza = DateTime.Now, Poslovnica = aeodrom, Vozilo = bmwx6 });
            context.AddRange(trenutnePoslovnice);
            context.SaveChanges();

            //dodavanje rezervacija 
            var klijentAdil = userManager.FindByEmailAsync("adil@gmail.com");
            rezervacije.Add(new Rezervacija
            {
                DatumRezervacije = DateTime.Now,
                DatumRentanja = DateTime.Now,
                Cijena = 400,
                VrstaRezervacije = "Normalna",
                DatumPreuzimanja = DateTime.Now,
                VrijemePreuzimanja = DateTime.Now,
                DatumPovrata = DateTime.Now,
                VrijemePovrata = DateTime.Now,
                BrojDanaIznajmljivanja = 4,
                Zakljucen = false,
                Vozilo = audia6,
                Klijent = klijentAdil.Result,
                Uposlenik = dona.Result,
                Poslovnica = aeodrom
            });
            rezervacije.Add(new Rezervacija
            {
                DatumRezervacije = DateTime.Now,
                DatumRentanja = DateTime.Now,
                Cijena = 400,
                VrstaRezervacije = "Normalna",
                DatumPreuzimanja = DateTime.Now,
                VrijemePreuzimanja = DateTime.Now,
                DatumPovrata = DateTime.Now,
                VrijemePovrata = DateTime.Now,
                BrojDanaIznajmljivanja = 4,
                Zakljucen = false,
                Vozilo = audia4,
                Klijent = klijentAdil.Result,
                Uposlenik = dona.Result,
                Poslovnica = aeodrom
            });
            context.AddRange(rezervacije);
            context.SaveChanges();

            //dodavanje rezervisani usluga
            rezervisaneUsluge.Add(new RezervisanaUsluga
            {
                Kolicina = 1,
                Opis = "Neki opis",
                UkupnaCijenaUsluge = 26,
                DodatneUsluge = context.DodatneUsluge.OrderBy(x => Guid.NewGuid()).FirstOrDefault(),
                Rezervacija = context.Rezervacija.OrderBy(x => Guid.NewGuid()).FirstOrDefault()
            });
            context.AddRange(rezervisaneUsluge);
            context.SaveChanges();

            //dodavanje prijevoza 
            var klijentNina = userManager.FindByEmailAsync("nina@gmail.com");
            prijevozi.Add(new Prijevoz
            {
                DatumRezervacije = DateTime.Now,
                DatumPrijevoza = DateTime.Now,
                CijenaPoKilometru = 1.5,
                CijenaCekanjaPoSatu = 0.3,
                NacinPlacanja = "Karticno",
                TipPrijevoza = "specijalni",
                Klijent = klijentNina.Result
            });
            context.AddRange(prijevozi);
            context.SaveChanges();

            var vozacMile = userManager.FindByEmailAsync("mile@sunnycars.vozac.com");
            var prijevoz1 = context.Prijevoz.Where(x => x.Klijent.Email == "nina@gmail.com").SingleOrDefault();
            uposleniciPrijevoz.Add(new UposlenikPrijevoz { BrojVozaca = 1, Uposlenik = vozacMile.Result, Prijevoz = prijevoz1 });
            context.AddRange(uposleniciPrijevoz);
            context.SaveChanges();

            vozilaPrijevoz.Add(new PrijevozVozilo { BrojVozila = 1, Prijevoz = prijevoz1, Vozilo = bmwx6 });
            context.AddRange(vozilaPrijevoz);
            context.SaveChanges();

            //dodavanje slika vozilima
            slike.Add(new Slika { Name = "BMW X6", Pozicija = 1, URL = "/images/Vozila/BMWX6/BMWX6_bijela_1.jpg", Vozilo = bmwx6 });
            slike.Add(new Slika { Name = "BMW X6", Pozicija = 2, URL = "/images/Vozila/BMWX6/BMWX6_bijela_2.jpg", Vozilo = bmwx6 });
            slike.Add(new Slika { Name = "BMW X6", Pozicija = 3, URL = "/images/Vozila/BMWX6/BMWX6_bijela_3.jpg", Vozilo = bmwx6 });
            slike.Add(new Slika { Name = "BMW X6", Pozicija = 4, URL = "/images/Vozila/BMWX6/BMWX6_bijela_4.jpg", Vozilo = bmwx6 });
            context.AddRange(slike);
            context.SaveChanges();

        }

        private static async Task KreirajKorisnika(UserManager<ApplicationUser> um, string username, string ime, string prezime, string adresa, Grad grad, string jmbg, DateTime datumRodjena, string spol, string slika, string password, string rola)
        {
            await um.CreateAsync(new ApplicationUser
            {
                UserName = username,
                Email = username,
                Ime = ime,
                Prezime = prezime,
                Adresa = adresa,
                Grad = grad,
                JMBG = jmbg,
                DatumRodjenja = datumRodjena,
                DatumRegistracije = System.DateTime.Now,
                Spol = spol,
                Slika = slika,
            });

            var korisnik = await um.FindByEmailAsync(username);
            await um.AddPasswordAsync(korisnik, password);
            await um.AddToRoleAsync(korisnik, rola);

        }


    }
}