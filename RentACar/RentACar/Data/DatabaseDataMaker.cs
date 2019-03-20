using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RentACar.Models;
using RentACar.Helper;

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

            /****************************************** DODAVANJE DODATNIH USLUGA ***************************************************/
            dodatneUsluge.Add(new DodatneUsluge { Naziv = "Djecije sjedalo", Cijena = 15, Opis = "Djecije sjedalo za zadnja sjedista vozila" });
            dodatneUsluge.Add(new DodatneUsluge { Naziv = "Nosac bicikla krov", Cijena = 10, Opis = "Nosac za bicikla koji se stavlja na krov" });
            dodatneUsluge.Add(new DodatneUsluge { Naziv = "Nosac bicikla nazad", Cijena = 11, Opis = "Nosac bicikla koji se stavlja na zadnji dio vozila" });
            dodatneUsluge.Add(new DodatneUsluge { Naziv = "Krovna kutija", Cijena = 16, Opis = "Kutija namijenjena za prtljagu koja se stavlja na krov" });
            dodatneUsluge.Add(new DodatneUsluge { Naziv = "GPS - navigacija", Cijena = 25, Opis = "GPS uredjaj sa potrebnim kartama za vozilo" });
            dodatneUsluge.Add(new DodatneUsluge { Naziv = "Osiguranje", Cijena = 30, Opis = "Dodatno osiguranje klijenta u slucaju da dodje do nepogodnih situacija" });
            context.AddRange(dodatneUsluge);
            context.SaveChanges();

            /****************************************** DODAVANJE BRENDOVA ***************************************************/
            brendovi.Add(new Brend { Naziv = "BMW" });
            brendovi.Add(new Brend { Naziv = "Audi" });
            brendovi.Add(new Brend { Naziv = "Volkswagen" });
            brendovi.Add(new Brend { Naziv = "Opel" });
            brendovi.Add(new Brend { Naziv = "Range Rover" });
            brendovi.Add(new Brend { Naziv = "Skoda" });
            brendovi.Add(new Brend { Naziv = "Mercedes" });
            brendovi.Add(new Brend { Naziv = "Tesla" });


            context.AddRange(brendovi);
            context.SaveChanges();
            
            /****************************************** DODAVANJE POSLOVNICA ***************************************************/ 

            var Sarajevo = context.Grad.Select(s => s).SingleOrDefault(p => p.Naziv == "Sarajevo");
            poslovnice.Add(new Poslovnica { Naziv = "Sarajevo Aeodrom", Adresa = "Butimirska Cesta bb", BrojTelefona = "+387 33 455 788", Email = "sunnycars@sunnygroup.com", Grad = Sarajevo });
            poslovnice.Add(new Poslovnica { Naziv = "Sarajevo Bascarsija", Adresa = "Bascarsija bb", BrojTelefona = "+387 33 788 789", Email = "sunnycars@sunnygroup.com", Grad = Sarajevo });

            var Mostar = context.Grad.Select(s => s).SingleOrDefault(p => p.Naziv == "Mostar");
            poslovnice.Add(new Poslovnica { Naziv = "Mostar Zalik", Adresa = "Zalik bb", BrojTelefona = "+387 35 448 458", Email = "sunnycars@sunnygroup.com", Grad = Mostar });

            var Kladusa = context.Grad.Select(s => s).SingleOrDefault(p => p.Naziv == "Velika Kladusa");
            poslovnice.Add(new Poslovnica { Naziv = "Velika Kladusa Centar", Adresa = "Milana Pilipovica bb", BrojTelefona = "+387 37 456 658", Email = "sunnycars@sunnygroup.com", Grad = Kladusa });

            var Tuzla = context.Grad.Select(s => s).SingleOrDefault(p => p.Naziv == "Tuzla");
            poslovnice.Add(new Poslovnica { Naziv = "Tuzla Stupine", Adresa = "Mehmedalije Maka Dizdara bb", BrojTelefona = "+387 35 105 202", Email = "sunnycars@sunnygroup.com", Grad = Tuzla });

            var BanjaLuka = context.Grad.Select(s => s).SingleOrDefault(p => p.Naziv == "Banja Luka");
            poslovnice.Add(new Poslovnica { Naziv = "Banja Nova Varos", Adresa = "Grcka bb", BrojTelefona = "+385 37 488 426", Email = "sunnycars@sunnygroup.com", Grad = BanjaLuka });

            var Brcko = context.Grad.Select(s => s).SingleOrDefault(p => p.Naziv == "Brcko");
            poslovnice.Add(new Poslovnica { Naziv = "Brcko Meraje", Adresa = "Mujdanovica bb", BrojTelefona = "+385 36 467 458", Email = "sunnycars@sunnygroup.com", Grad = Brcko });

            context.AddRange(poslovnice);
            context.SaveChanges();

            /****************************************** DODAVANJE PRIKOLICA ***************************************************/
            prikolice.Add(new Prikolica { Duzina = 250, Sirina = 180, TipPrikolice = (int) TipPrikolice.Velika ,Cijna = 75, Zapremina = 200 });
            prikolice.Add(new Prikolica { Duzina = 220, Sirina = 175, TipPrikolice = (int ) TipPrikolice.Srednja, Cijna =70, Zapremina = 170 });
            prikolice.Add(new Prikolica { Duzina = 200, Sirina = 180, TipPrikolice = (int) TipPrikolice.Mala, Cijna = 50, Zapremina = 150 });
            prikolice.Add(new Prikolica { Duzina = 250, Sirina = 185, TipPrikolice = (int) TipPrikolice.Srednja, Cijna = 70, Zapremina = 200 });
            prikolice.Add(new Prikolica { Duzina = 250, Sirina = 195, TipPrikolice = (int ) TipPrikolice.Zatvorena_Velika, Cijna = 80, Zapremina = 220 });
            context.AddRange(prikolice);
            context.SaveChanges();

            //KREIRANJE ULOGA ADMINISTRATOR, UPOSLENIK, KLIJENT ...
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



            //ADMINISTRATOR
            await KreirajKorisnika(userManager, "johny@sunnycars.admin.com", "Johny", "Bravo", "Dzemala Bijedica bb", Sarajevo, "0133546579", new DateTime(1996, 5, 6), "M", "nema", "P@sword123", admin);

            //UPOSLENICI
            //za poslovnicu Sarajevo Aeodrom
            await KreirajKorisnika(userManager, "sonny.lona@sunnycars.com", "Sonny", "Lona", "Dzemala Bijedica bb", Sarajevo, "062354899", new DateTime(1995, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", uposlenik);
            await KreirajKorisnika(userManager, "dona.gomez@sunnycars.com", "Dona", "Gomez", "Dzemala Bijedica bb", Sarajevo, "062231254", new DateTime(1994, 5, 6), "Z", "/images/profile-woman.jpg", "P@sword123", uposlenik);
            await KreirajKorisnika(userManager, "jack.olivero@sunnycars.com", "Jack", "Olivero", "Dzemala Bijedica bb", Sarajevo, "062365458", new DateTime(1994, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", mehanicar);
            await KreirajKorisnika(userManager, "antone.no@sunnycars.com", "Antone", "No", "Dzemala Bijedica bb", Sarajevo, "062325558", new DateTime(1994, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", vozac);

            //za poslovnicu Sarajevo Bascarsija
            await KreirajKorisnika(userManager, "zenaida.aubry@sunnycars.com", "Zenaida", "Aubry", "Dzemala Bijedica bb", Sarajevo, "062456852", new DateTime(1995, 5, 6), "Z", "/images/profile-woman.jpg", "P@sword123", uposlenik);
            await KreirajKorisnika(userManager, "mason.goldman@sunnycars.com", "Mason", "Goldman", "Dzemala Bijedica bb", Sarajevo, "063955489", new DateTime(1994, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", uposlenik);
            await KreirajKorisnika(userManager, "allan.tregre@sunnycars.com", "Allan", "Tregre", "Dzemala Bijedica bb", Sarajevo, "06852549", new DateTime(1994, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", mehanicar);
            await KreirajKorisnika(userManager, "zack.frazer@sunnycars.com", "Zack", "Frazer", "Dzemala Bijedica bb", Sarajevo, "062369855", new DateTime(1994, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", vozac);

            //za poslovnicu Mostar Zalik
            await KreirajKorisnika(userManager, "nina.thiem@sunnycars.com", "Nina", "Thiem", "Mostarska bb", Mostar, "0623321564", new DateTime(1995, 5, 6), "Z", "/images/profile-woman.jpg", "P@sword123", uposlenik);
            await KreirajKorisnika(userManager, "eddie.bachicha@sunnycars.com", "Eddie", "Bachicha", "Mostarska bb", Mostar, "06455256657", new DateTime(1994, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", uposlenik);
            await KreirajKorisnika(userManager, "homer.simpson@sunnycars.com", "Homer", "Simpson", "Mostarska bb", Mostar, "0643215575", new DateTime(1994, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", mehanicar);
            await KreirajKorisnika(userManager, "rodrigo.bui@sunnycars.com", "Rodrigo", "Bui", "Mostarska bb", Mostar, "063123654", new DateTime(1994, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", vozac);

            //KLIJENTI
            await KreirajKorisnika(userManager, "janell.kearns@gmail.com", "Janell", "Kearns", "Dzemala Bijedica bb", Sarajevo, "0623665544", new DateTime(1989, 5, 6), "Z", "/images/profile-woman.jpg", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "ronnie.spidle@gmail.com", "Ronnie", "Spidle", "Dzemala Bijedica bb", Sarajevo, "062544887", new DateTime(1975, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "merle.bowe@gmail.com", "Merle", "Bowe", "Dzemala Bijedica bb", Sarajevo, "062453987", new DateTime(1985, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "ena.schlueter@gmail.com", "Ena", "Schlueter", "Dzemala Bijedica bb", Sarajevo, "062456888", new DateTime(1987, 5, 6), "Z", "/images/profile-woman.jpg", "P@sword123", klijent);

            await KreirajKorisnika(userManager, "brent.pae@gmail.com", "Brent", "Pae", "Dzemala Bijedica bb", Sarajevo, "062654899", new DateTime(1989, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "eleonora.odaniel@gmail.com", "Eleonora", "Odaniel", "Dzemala Bijedica bb", Sarajevo, "063548798", new DateTime(1975, 5, 6), "Z", "/images/profile-woman.jpg", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "rob.petroski@gmail.com", "Rob", "Petroski", "Dzemala Bijedica bb", Sarajevo, "062548832", new DateTime(1985, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "ivana.mayen@gmail.com", "Ivana", "Mayen", "Dzemala Bijedica bb", Sarajevo, "063556321", new DateTime(1987, 5, 6), "Z", "/images/profile-woman.jpg", "P@sword123", klijent);

            await KreirajKorisnika(userManager, "bobbie.wayman@gmail.com", "Bobbie", "Wayman", "Dzemala Bijedica bb", Sarajevo, "0632155587", new DateTime(1989, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "alise.keister@gmail.com", "Alise", "Keister", "Dzemala Bijedica bb", Sarajevo, "06311254658", new DateTime(1975, 5, 6), "Z", "/images/profile-woman.jpg", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "julio.falkner@gmail.com", "Julio", "Falkner", "Dzemala Bijedica bb", Sarajevo, "0624665789", new DateTime(1985, 5, 6), "M", "/images/profile-man.jpg", "P@sword123", klijent);
            await KreirajKorisnika(userManager, "camila.ginn@gmail.com", "Camila", "Ginn", "Dzemala Bijedica bb", Sarajevo, "063123654", new DateTime(1987, 5, 6), "Z", "/images/profile-woman.jpg", "P@sword123", klijent);

            /****************************************** DODAVANJE UPOSLENIKA POSLOVNICAMA ***************************************************/

            //dodavanje uposlenika poslovnici Sarajevo Aeodrom
            var aeodrom = context.Poslovnica.Select(s => s).SingleOrDefault(x => x.Naziv == "Sarajevo Aeodrom");
            var sonny = userManager.FindByEmailAsync("sonny.lona@sunnycars.com");
            var dona = userManager.FindByEmailAsync("dona.gomez@sunnycars.com");
            var jackOlivero = userManager.FindByEmailAsync("jack.olivero@sunnycars.com");
            var antoneNo = userManager.FindByEmailAsync("antone.no@sunnycars.com");
            var johny = userManager.FindByEmailAsync("johny@sunnycars.admin.com");

            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = aeodrom, RadnoMjesto = "salter", Uposlenik = sonny.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = aeodrom, RadnoMjesto = "salter", Uposlenik = dona.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = aeodrom, RadnoMjesto = "kancelarija", Uposlenik = johny.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = aeodrom, RadnoMjesto = "radnja", Uposlenik = jackOlivero.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = aeodrom, RadnoMjesto = "poslovnica", Uposlenik = antoneNo.Result });

            //dodavanje uposlenika poslovnici Sarajevo Bascarsija
            var bascarsija = context.Poslovnica.Select(s => s).SingleOrDefault(x => x.Naziv == "Sarajevo Bascarsija");
            var zenaida = userManager.FindByEmailAsync("zenaida.aubry@sunnycars.com");
            var mason = userManager.FindByEmailAsync("mason.goldman@sunnycars.com");
            var allan = userManager.FindByEmailAsync("allan.tregre@sunnycars.com");
            var zack = userManager.FindByEmailAsync("zack.frazer@sunnycars.com");

            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = bascarsija, RadnoMjesto = "salter", Uposlenik = zenaida.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = bascarsija, RadnoMjesto = "salter", Uposlenik = mason.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = bascarsija, RadnoMjesto = "radnja", Uposlenik = allan.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = bascarsija, RadnoMjesto = "poslovnica", Uposlenik = zack.Result });

            //dodavanje uposlenika poslovnici Mostar Zalik
            //dodavanje uposlenika poslovnici Sarajevo Bascarsija
            var mostarZalik = context.Poslovnica.Select(s => s).SingleOrDefault(x => x.Naziv == "Mostar Zalik");
            var nina = userManager.FindByEmailAsync("nina.thiem@sunnycars.com");
            var eddie = userManager.FindByEmailAsync("eddie.bachicha@sunnycars.com");
            var homer = userManager.FindByEmailAsync("homer.simpson@sunnycars.com");
            var rodrigo = userManager.FindByEmailAsync("rodrigo.bui@sunnycars.com");

            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = mostarZalik, RadnoMjesto = "salter", Uposlenik = nina.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = mostarZalik, RadnoMjesto = "salter", Uposlenik = eddie.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = mostarZalik, RadnoMjesto = "radnja", Uposlenik = homer.Result });
            ugovoriZaposlenja.Add(new UgovorZaposlenja { DatumZaposlenja = DateTime.Now, Poslovnica = mostarZalik, RadnoMjesto = "poslovnica", Uposlenik = rodrigo.Result });


            context.AddRange(ugovoriZaposlenja);
            context.SaveChanges();

            /****************************************** DODAVANJE VOZILA ***************************************************/

            /******************* Audi ********************/
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
                Cijena = 60,
                Kilometraza = 140546,
                ZapreminaPrtljaznika = 60,
                ZapreminaPrtljaznikaNaprijed = 0,
                Naziv = "Audi A6",
                Brend = audi,
                Kuka = true,
                Klima = true,
                TipVozila = (int) TipVozila.Normal,
                DodatniOpis = "Bez dodatnog opisa",
                Gorivo = (int) Gorivo.Dizel,
                Pogon = "4x4",
                Transmisija = (int) Transmisija.Automatik,
                GrupniTipVozila =(int) GrupniTipVozila.Putnicko
            });
            vozila.Add(new Vozilo
            {
                BrojSasije = 615654,
                RegistarskaOznaka = "455-A-644",
                Boja = "Crna",
                Kuka = true,
                Model = "A4",
                GodinaProizvodnje = 2015,
                SnagaMotora = 150,
                Kilometraza = 122302,
                DatumMijenjanjUlja = DateTime.Now,
                Domet = 450,
                BrojMjesta = 5,
                BrojVrata = 4,
                Cijena =50,
                ZapreminaPrtljaznika = 70,
                ZapreminaPrtljaznikaNaprijed = 0,
                Naziv = "Audi A4",
                Brend = audi,
                Klima = true,
                TipVozila = (int)TipVozila.Normal,
                DodatniOpis = "Bez dodatnog opisa",
                Gorivo = (int)Gorivo.Dizel,
                Pogon = "Prednji",
                Transmisija = (int)Transmisija.Automatik,
                GrupniTipVozila = (int)GrupniTipVozila.Putnicko
            });

            /******************* BMW ********************/
            var bmw = context.Brend.Select(s => s).SingleOrDefault(x => x.Naziv == "BMW");
            vozila.Add(new Vozilo
            {
                BrojSasije = 654984,
                RegistarskaOznaka = "599-S-698",
                Boja = "Bijela",
                Kilometraza =92354,
                Kuka = true,
                Model = "X6",
                GodinaProizvodnje = 2018,
                SnagaMotora = 340,
                DatumMijenjanjUlja = DateTime.Now,
                Domet = 550,
                Cijena = 70,
                BrojMjesta = 5,
                BrojVrata = 4,
                ZapreminaPrtljaznika = 120,
                ZapreminaPrtljaznikaNaprijed = 0,
                Naziv = "BMW X6",
                Brend = bmw,
                Klima = true,
                TipVozila = (int)TipVozila.Normal,
                DodatniOpis = "Bez dodatnog opisa",
                Gorivo = (int)Gorivo.Benzin,
                Pogon = "4x4",
                Transmisija = (int)Transmisija.Automatik,
                GrupniTipVozila = (int)GrupniTipVozila.Putnicko
            });

            vozila.Add(new Vozilo
            {
                BrojSasije = 112332211,
                RegistarskaOznaka = "876-G-554",
                Boja = "Bijela",
                Kilometraza = 12354,
                Kuka = false,
                Model = "i3",
                GodinaProizvodnje = 2016,
                SnagaMotora = 120,
                DatumMijenjanjUlja = DateTime.Now,
                Domet = 400,
                Cijena = 85,
                BrojMjesta = 4,
                BrojVrata = 4,
                ZapreminaPrtljaznika = 50,
                ZapreminaPrtljaznikaNaprijed = 0,
                Naziv = "BMW i3",
                Brend = bmw,
                Klima = true,
                TipVozila = (int)TipVozila.Hibrid,
                DodatniOpis = "Bez dodatnog opisa",
                Gorivo = (int)Gorivo.Benzin,
                Pogon = "Zadnja",
                Transmisija = (int)Transmisija.Automatik,
                GrupniTipVozila = (int)GrupniTipVozila.Putnicko
            });

            /******************* Opel ********************/
            var opel = context.Brend.Select(s => s).SingleOrDefault(x => x.Naziv == "Opel");

            vozila.Add(new Vozilo
            {
                BrojSasije = 65488984,
                RegistarskaOznaka = "687-A-126",
                Boja = "Bijela",
                Kuka = true,
                Model = "Insignia",
                GodinaProizvodnje = 2018,
                SnagaMotora = 230,
                Kilometraza = 12255,
                DatumMijenjanjUlja = DateTime.Now,
                Domet = 520,
                BrojMjesta = 5,
                BrojVrata = 4,
                Cijena = 70,
                ZapreminaPrtljaznika = 140,
                ZapreminaPrtljaznikaNaprijed = 0,
                Naziv = "Opel Insignia",
                Brend = opel,
                Klima = true,
                TipVozila = (int)TipVozila.Normal,
                DodatniOpis = "Bez dodatnog opisa",
                Gorivo = (int)Gorivo.Dizel,
                Pogon = "Prednji",
                Transmisija = (int)Transmisija.Automatik,
                GrupniTipVozila = (int)GrupniTipVozila.Putnicko
            });
            /******************* Tesla ********************/
            var tesla = context.Brend.Select(s => s).SingleOrDefault(x => x.Naziv == "Tesla");

            vozila.Add(new Vozilo
            {
                BrojSasije = 789999852,
                RegistarskaOznaka = "123-A-666",
                Boja = "Siva",
                Kuka = false,
                Model = "Model S",
                GodinaProizvodnje = 2017,
                SnagaMotora = 630,
                Kilometraza = 51002,
                DatumMijenjanjUlja = DateTime.Now,
                Domet = 650,
                BrojMjesta = 5,
                BrojVrata = 4,
                Cijena = 130,
                ZapreminaPrtljaznika = 140,
                ZapreminaPrtljaznikaNaprijed = 50,
                Naziv = "Tesla Model S",
                Brend = tesla,
                Klima = true,
                TipVozila = (int)TipVozila.Elektricno,
                DodatniOpis = "Bez dodatnog opisa",
                Gorivo = (int)Gorivo.Struja,
                Pogon = "Dinamicno",
                Transmisija = (int)Transmisija.Automatik,
                GrupniTipVozila = (int)GrupniTipVozila.Putnicko
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
            var opelInsignia = context.Vozilo.Select(s => s).SingleOrDefault(x => x.Naziv == "Opel Insignia");
            var teslaModelS = context.Vozilo.Select(s => s).SingleOrDefault(x => x.Naziv == "Tesla Model S");
            var bmwi3 = context.Vozilo.Select(s => s).SingleOrDefault(x => x.Naziv == "BMW i3");

            //dodavanje vozila u poslovncu Sarajevo Aeodrom
            trenutnePoslovnice.Add(new TrenutnaPoslovnica { DatumUlaza = DateTime.Now, DatumIzlaza = DateTime.Now,VoziloRezervisano = true, Poslovnica = aeodrom, Vozilo = audia4});
            trenutnePoslovnice.Add(new TrenutnaPoslovnica { DatumUlaza = DateTime.Now, DatumIzlaza = DateTime.Now,VoziloRezervisano = true, Poslovnica = aeodrom, Vozilo = audia6 });
            trenutnePoslovnice.Add(new TrenutnaPoslovnica { DatumUlaza = DateTime.Now, DatumIzlaza = null, VoziloRezervisano = false,Poslovnica = aeodrom, Vozilo = bmwx6 });
            trenutnePoslovnice.Add(new TrenutnaPoslovnica { DatumUlaza = DateTime.Now, DatumIzlaza = null, VoziloRezervisano = false,Poslovnica = aeodrom, Vozilo = opelInsignia });

            //dodavanje vozila u poslovnicu Mostar Zalik
            trenutnePoslovnice.Add(new TrenutnaPoslovnica { DatumUlaza = DateTime.Now, DatumIzlaza = null, VoziloRezervisano = false,Poslovnica = mostarZalik, Vozilo = teslaModelS });
            trenutnePoslovnice.Add(new TrenutnaPoslovnica { DatumUlaza = DateTime.Now, DatumIzlaza = null, VoziloRezervisano = false,Poslovnica = mostarZalik, Vozilo = bmwi3 });
            context.AddRange(trenutnePoslovnice);
            context.SaveChanges();

            //dodavanje rezervacija 
            var klijentJanell = userManager.FindByEmailAsync("janell.kearns@gmail.com");
            rezervacije.Add(new Rezervacija
            {
                DatumRezervacije = DateTime.Now,
                Cijena = 400,
                DatumPreuzimanja = DateTime.Now,
                DatumPovrata = DateTime.Now,
                UspjesnoSpremljena = true,
                BrojDanaIznajmljivanja = 4,
                Zakljucen = (int) InfoRezervacija.U_Obradi, 
                Vozilo = audia6,
                SifraRezervacije = "001/19",
                Klijent = klijentJanell.Result,
                Uposlenik = dona.Result,
                Poslovnica = aeodrom
            });
            rezervacije.Add(new Rezervacija
            {
                DatumRezervacije = DateTime.Now,
                Cijena = 400,
                DatumPreuzimanja = DateTime.Now,
                DatumPovrata = DateTime.Now,
                UspjesnoSpremljena = true,
                BrojDanaIznajmljivanja = 4,
                Zakljucen = (int)InfoRezervacija.Odobrena,
                Vozilo = audia4,
                SifraRezervacije = "002/19",
                Klijent = klijentJanell.Result,
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
                UkupnaCijenaUsluge = 80,
                DodatneUsluge = context.DodatneUsluge.OrderBy(x => Guid.NewGuid()).FirstOrDefault(),
                Rezervacija = context.Rezervacija.OrderBy(x => Guid.NewGuid()).FirstOrDefault()
            });
            context.AddRange(rezervisaneUsluge);
            context.SaveChanges();

            //dodavanje prijevoza 
            var klijentRonnie = userManager.FindByEmailAsync("ronnie.spidle@gmail.com");
            prijevozi.Add(new Prijevoz
            {
                DatumRezervacije = DateTime.Now,
                DatumPrijevoza = DateTime.Now,
                CijenaPoKilometru = 1.5,
                CijenaCekanjaPoSatu = 0.3,
                NacinPlacanja = (int) NacinPlacanja.Karticno,
                TipPrijevoza = (int) TipPrijevoza.Specijal,
                Klijent = klijentRonnie.Result
            });
            context.AddRange(prijevozi);
            context.SaveChanges();

            var vozacMile = userManager.FindByEmailAsync("antone.no@sunnycars.com");
            var prijevoz1 = context.Prijevoz.Where(x => x.Klijent.Email == "ronnie.spidle@gmail.com").SingleOrDefault();
            uposleniciPrijevoz.Add(new UposlenikPrijevoz { Uposlenik = vozacMile.Result, Prijevoz = prijevoz1 });
            context.AddRange(uposleniciPrijevoz);
            context.SaveChanges();

            vozilaPrijevoz.Add(new PrijevozVozilo { Prijevoz = prijevoz1, Vozilo = bmwx6 });
            context.AddRange(vozilaPrijevoz);
            context.SaveChanges();

            //dodavanje slika vozilima
            slike.Add(new Slika { Name = "BMW X6", Pozicija = 1, URL = "/images/Vozila/BMWX6/BMWX6_bijela_1.jpg", Vozilo = bmwx6 });
            slike.Add(new Slika { Name = "BMW X6", Pozicija = 2, URL = "/images/Vozila/BMWX6/BMWX6_bijela_2.jpg", Vozilo = bmwx6 });
            slike.Add(new Slika { Name = "BMW X6", Pozicija = 3, URL = "/images/Vozila/BMWX6/BMWX6_bijela_3.jpg", Vozilo = bmwx6 });
            slike.Add(new Slika { Name = "BMW X6", Pozicija = 4, URL = "/images/Vozila/BMWX6/BMWX6_bijela_4.jpg", Vozilo = bmwx6 });

            slike.Add(new Slika { Name = "Audi A6", Pozicija = 1, URL = "/images/Vozila/AudiA6_bijela/AudiA6_bijela_1.jpg", Vozilo = audia6 });
            slike.Add(new Slika { Name = "Audi A6", Pozicija = 2, URL = "/images/Vozila/AudiA6_bijela/AudiA6_bijela_3.jpg", Vozilo = audia6 });
            slike.Add(new Slika { Name = "Audi A6", Pozicija = 3, URL = "/images/Vozila/AudiA6_bijela/AudiA6_bijela_4.jpg", Vozilo = audia6 });
            slike.Add(new Slika { Name = "Audi A6", Pozicija = 4, URL = "/images/Vozila/AudiA6_bijela/AudiA6_bijela_5.jpg", Vozilo = audia6 });

            slike.Add(new Slika { Name = "Audi A4", Pozicija = 1, URL = "/images/Vozila/AudiA4_crna/AudiA4_crna_1.jpg", Vozilo = audia4 });
            slike.Add(new Slika { Name = "Audi A4", Pozicija = 2, URL = "/images/Vozila/AudiA4_crna/AudiA4_crna_2.png", Vozilo = audia4 });
            slike.Add(new Slika { Name = "Audi A4", Pozicija = 3, URL = "/images/Vozila/AudiA4_crna/AudiA4_crna_3.jpg", Vozilo = audia4 });
            slike.Add(new Slika { Name = "Audi A4", Pozicija = 4, URL = "/images/Vozila/AudiA4_crna/AudiA4_crna_4.jpg", Vozilo = audia4 });

            slike.Add(new Slika { Name = "Opel Insignia", Pozicija = 1, URL = "/images/Vozila/Opel_insignia/Opel_Insignia_bijela_1.jpg", Vozilo = opelInsignia });
            slike.Add(new Slika { Name = "Opel Insignia", Pozicija = 2, URL = "/images/Vozila/Opel_insignia/Opel_Insignia_bijela_2.jpg", Vozilo = opelInsignia });
            slike.Add(new Slika { Name = "Opel Insignia", Pozicija = 3, URL = "/images/Vozila/Opel_insignia/Opel_Insignia_bijela_3.jpg", Vozilo = opelInsignia });
            slike.Add(new Slika { Name = "Opel Insignia", Pozicija = 4, URL = "/images/Vozila/Opel_insignia/Opel_Insignia_bijela_4.jpg", Vozilo = opelInsignia });

            slike.Add(new Slika { Name = "Tesla Model S", Pozicija = 1, URL = "/images/Vozila/TeslaModelS/tesla_1.jpg", Vozilo = teslaModelS });
            slike.Add(new Slika { Name = "Tesla Model S", Pozicija = 2, URL = "/images/Vozila/TeslaModelS/tesla_2.jpg", Vozilo = teslaModelS });
            slike.Add(new Slika { Name = "Tesla Model S", Pozicija = 3, URL = "/images/Vozila/TeslaModelS/tesla_3.jpg", Vozilo = teslaModelS });
            slike.Add(new Slika { Name = "Tesla Model S", Pozicija = 4, URL = "/images/Vozila/TeslaModelS/tesla_4.jpg", Vozilo = teslaModelS });

            slike.Add(new Slika { Name = "BMW i3", Pozicija = 1, URL = "/images/Vozila/bmw_i3/i3_1.jpeg", Vozilo = bmwi3 });
            slike.Add(new Slika { Name = "BMW i3", Pozicija = 2, URL = "/images/Vozila/bmw_i3/i3_2.jpg", Vozilo = bmwi3 });
            slike.Add(new Slika { Name = "BMW i3", Pozicija = 3, URL = "/images/Vozila/bmw_i3/i3_3.jpeg", Vozilo = bmwi3 });
            slike.Add(new Slika { Name = "BMW i3", Pozicija = 4, URL = "/images/Vozila/bmw_i3/i3_4.jpg", Vozilo = bmwi3 });

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