﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RentACar.Data;

namespace RentACar.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190309154437_nulableUposlenik")]
    partial class nulableUposlenik
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RoleID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RoleClaimID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId")
                        .HasColumnName("RoleID");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UserClaimID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId")
                        .HasColumnName("UserID");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId")
                        .HasColumnName("UserID");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnName("UserID");

                    b.Property<int>("RoleId")
                        .HasColumnName("RoleID");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnName("UserID");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserToken");
                });

            modelBuilder.Entity("RentACar.Models.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UserID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Adresa");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DatumRegistracije");

                    b.Property<DateTime>("DatumRodjenja");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("GradID");

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("JMBG");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Spol");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("GradID");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RentACar.Models.Brend", b =>
                {
                    b.Property<int>("BrendID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.HasKey("BrendID");

                    b.ToTable("Brend");
                });

            modelBuilder.Entity("RentACar.Models.DodatneUsluge", b =>
                {
                    b.Property<int>("DodatneUslugeID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Cijena");

                    b.Property<string>("Naziv");

                    b.Property<string>("Opis");

                    b.HasKey("DodatneUslugeID");

                    b.ToTable("DodatneUsluge");
                });

            modelBuilder.Entity("RentACar.Models.Drzava", b =>
                {
                    b.Property<int>("DrzavaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.Property<string>("Skracenica");

                    b.HasKey("DrzavaID");

                    b.ToTable("Drzava");
                });

            modelBuilder.Entity("RentACar.Models.Grad", b =>
                {
                    b.Property<int>("GradID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.Property<int>("OpcinaID");

                    b.HasKey("GradID");

                    b.HasIndex("OpcinaID");

                    b.ToTable("Grad");
                });

            modelBuilder.Entity("RentACar.Models.KompatibilnostPrikolica", b =>
                {
                    b.Property<int>("KompatibilnostPrikolicaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PrikolicaID");

                    b.Property<string>("Tezina");

                    b.Property<string>("TipKuke");

                    b.Property<int>("VoziloID");

                    b.HasKey("KompatibilnostPrikolicaID");

                    b.HasIndex("PrikolicaID");

                    b.HasIndex("VoziloID");

                    b.ToTable("KompatibilnostPrikolica");
                });

            modelBuilder.Entity("RentACar.Models.OcjenaPrijevoz", b =>
                {
                    b.Property<int>("OcjenaPrijevozID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("KlijentID");

                    b.Property<int>("OcjenaVrijednost");

                    b.Property<string>("Poruka");

                    b.Property<int>("PrijevozID");

                    b.Property<int?>("PrijevozVoziloID");

                    b.HasKey("OcjenaPrijevozID");

                    b.HasIndex("KlijentID");

                    b.HasIndex("PrijevozID");

                    b.HasIndex("PrijevozVoziloID");

                    b.ToTable("OcjenaPrijevoz");
                });

            modelBuilder.Entity("RentACar.Models.OcjenaRezervacija", b =>
                {
                    b.Property<int>("OcjenaRezervacijaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("KlijentID");

                    b.Property<int>("OcjenaVrijednost");

                    b.Property<string>("Poruka");

                    b.Property<int>("RezervacijaID");

                    b.HasKey("OcjenaRezervacijaID");

                    b.HasIndex("KlijentID");

                    b.HasIndex("RezervacijaID");

                    b.ToTable("OcjenaRezervacija");
                });

            modelBuilder.Entity("RentACar.Models.Opcina", b =>
                {
                    b.Property<int>("OpcinaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.Property<int>("PostanskiBroj");

                    b.Property<int>("RegijaID");

                    b.HasKey("OpcinaID");

                    b.HasIndex("RegijaID");

                    b.ToTable("Opcina");
                });

            modelBuilder.Entity("RentACar.Models.OstecenjeInfo", b =>
                {
                    b.Property<int>("OstecenjeInfoID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DatumZapisa");

                    b.Property<bool>("FizickoOstecenje");

                    b.Property<int>("MehanicarID");

                    b.Property<string>("Opis");

                    b.Property<int>("RezervacijaID");

                    b.Property<int>("VoziloID");

                    b.HasKey("OstecenjeInfoID");

                    b.HasIndex("MehanicarID");

                    b.HasIndex("RezervacijaID");

                    b.HasIndex("VoziloID");

                    b.ToTable("OstecenjeInfo");
                });

            modelBuilder.Entity("RentACar.Models.Poslovnica", b =>
                {
                    b.Property<int>("PoslovnicaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adresa");

                    b.Property<string>("BrojTelefona");

                    b.Property<string>("Email");

                    b.Property<int>("GradID");

                    b.Property<string>("Naziv");

                    b.HasKey("PoslovnicaID");

                    b.HasIndex("GradID");

                    b.ToTable("Poslovnica");
                });

            modelBuilder.Entity("RentACar.Models.Prijevoz", b =>
                {
                    b.Property<int>("PrijevozID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("CijenaCekanjaPoSatu");

                    b.Property<double>("CijenaPoKilometru");

                    b.Property<DateTime>("DatumPrijevoza");

                    b.Property<DateTime>("DatumRezervacije");

                    b.Property<int>("KlijentID");

                    b.Property<string>("NacinPlacanja");

                    b.Property<string>("TipPrijevoza");

                    b.HasKey("PrijevozID");

                    b.HasIndex("KlijentID");

                    b.ToTable("Prijevoz");
                });

            modelBuilder.Entity("RentACar.Models.PrijevozVozilo", b =>
                {
                    b.Property<int>("PrijevozVoziloID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrojVozila");

                    b.Property<int>("PrijevozID");

                    b.Property<int>("VoziloID");

                    b.HasKey("PrijevozVoziloID");

                    b.HasIndex("PrijevozID");

                    b.HasIndex("VoziloID");

                    b.ToTable("PrijevozVozilo");
                });

            modelBuilder.Entity("RentACar.Models.Prikolica", b =>
                {
                    b.Property<int>("PrikolicaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Duzina");

                    b.Property<double>("Sirina");

                    b.Property<string>("TipPrikolice");

                    b.Property<double>("Zapremina");

                    b.HasKey("PrikolicaID");

                    b.ToTable("Prikolica");
                });

            modelBuilder.Entity("RentACar.Models.Regija", b =>
                {
                    b.Property<int>("RegijaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DrzavaID");

                    b.HasKey("RegijaID");

                    b.HasIndex("DrzavaID");

                    b.ToTable("Regija");
                });

            modelBuilder.Entity("RentACar.Models.Rezervacija", b =>
                {
                    b.Property<int>("RezervacijaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrojDanaIznajmljivanja");

                    b.Property<double>("Cijena");

                    b.Property<DateTime>("DatumPovrata");

                    b.Property<DateTime>("DatumPreuzimanja");

                    b.Property<DateTime>("DatumRentanja");

                    b.Property<DateTime>("DatumRezervacije");

                    b.Property<int>("KlijentID");

                    b.Property<string>("NacinPlacanja");

                    b.Property<int>("PoslovnicaID");

                    b.Property<int?>("UposlenikID");

                    b.Property<int>("VoziloID");

                    b.Property<DateTime>("VrijemePovrata");

                    b.Property<DateTime>("VrijemePreuzimanja");

                    b.Property<string>("VrstaRezervacije");

                    b.Property<bool>("Zakljucen");

                    b.HasKey("RezervacijaID");

                    b.HasIndex("KlijentID");

                    b.HasIndex("PoslovnicaID");

                    b.HasIndex("UposlenikID");

                    b.HasIndex("VoziloID");

                    b.ToTable("Rezervacija");
                });

            modelBuilder.Entity("RentACar.Models.RezervisanaUsluga", b =>
                {
                    b.Property<int>("RezervisanaUslugaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DodatneUslugeID");

                    b.Property<int>("Kolicina");

                    b.Property<string>("Opis");

                    b.Property<int>("RezervacijaID");

                    b.Property<double>("UkupnaCijenaUsluge");

                    b.HasKey("RezervisanaUslugaID");

                    b.HasIndex("DodatneUslugeID");

                    b.HasIndex("RezervacijaID");

                    b.ToTable("RezervisanaUsluga");
                });

            modelBuilder.Entity("RentACar.Models.Slika", b =>
                {
                    b.Property<int>("SlikaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("Pozicija");

                    b.Property<string>("URL");

                    b.Property<int>("VoziloID");

                    b.HasKey("SlikaID");

                    b.HasIndex("VoziloID");

                    b.ToTable("Slika");
                });

            modelBuilder.Entity("RentACar.Models.TrenutnaPoslovnica", b =>
                {
                    b.Property<int>("TrenutnaPoslovnicaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DatumIzlaza");

                    b.Property<DateTime>("DatumUlaza");

                    b.Property<int>("PoslovnicaID");

                    b.Property<int>("VoziloID");

                    b.HasKey("TrenutnaPoslovnicaID");

                    b.HasIndex("PoslovnicaID");

                    b.HasIndex("VoziloID");

                    b.ToTable("TrenutnaPoslovnica");
                });

            modelBuilder.Entity("RentACar.Models.UgovorZaposlenja", b =>
                {
                    b.Property<int>("UgovorZaposlenjaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DatumZaposlenja");

                    b.Property<int>("PoslovnicaID");

                    b.Property<string>("RadnoMjesto");

                    b.Property<int>("UposlenikID");

                    b.HasKey("UgovorZaposlenjaID");

                    b.HasIndex("PoslovnicaID");

                    b.HasIndex("UposlenikID");

                    b.ToTable("UgovorZaposlenja");
                });

            modelBuilder.Entity("RentACar.Models.UposlenikPrijevoz", b =>
                {
                    b.Property<int>("UposlenikPrijevozID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrojVozaca");

                    b.Property<int>("PrijevozID");

                    b.Property<int>("UposlenikID");

                    b.HasKey("UposlenikPrijevozID");

                    b.HasIndex("PrijevozID");

                    b.HasIndex("UposlenikID");

                    b.ToTable("UposlenikPrijevoz");
                });

            modelBuilder.Entity("RentACar.Models.Vozilo", b =>
                {
                    b.Property<int>("VoziloID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Boja");

                    b.Property<int>("BrendID");

                    b.Property<int>("BrojMjesta");

                    b.Property<int>("BrojSasije");

                    b.Property<int>("BrojVrata");

                    b.Property<DateTime>("DatumMijenjanjUlja");

                    b.Property<string>("DodatniOpis");

                    b.Property<int>("Domet");

                    b.Property<int>("GodinaProizvodnje");

                    b.Property<string>("Gorivo");

                    b.Property<string>("GrupniTipVozila");

                    b.Property<bool>("Klima");

                    b.Property<string>("Model");

                    b.Property<string>("Naziv");

                    b.Property<string>("Pogon");

                    b.Property<string>("RegistarskaOznaka");

                    b.Property<int>("SnagaMotora");

                    b.Property<string>("TipVozila");

                    b.Property<string>("Transmisija");

                    b.Property<double>("ZapreminaPrtljaznika");

                    b.Property<double>("ZapreminaPrtljaznikaNaprijed");

                    b.HasKey("VoziloID");

                    b.HasIndex("BrendID");

                    b.ToTable("Vozilo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("RentACar.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("RentACar.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("RentACar.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.ApplicationUser", b =>
                {
                    b.HasOne("RentACar.Models.Grad", "Grad")
                        .WithMany()
                        .HasForeignKey("GradID");
                });

            modelBuilder.Entity("RentACar.Models.Grad", b =>
                {
                    b.HasOne("RentACar.Models.Opcina", "Opcina")
                        .WithMany("gradovi")
                        .HasForeignKey("OpcinaID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.KompatibilnostPrikolica", b =>
                {
                    b.HasOne("RentACar.Models.Prikolica", "Prikolica")
                        .WithMany()
                        .HasForeignKey("PrikolicaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.Vozilo", "Vozilo")
                        .WithMany()
                        .HasForeignKey("VoziloID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.OcjenaPrijevoz", b =>
                {
                    b.HasOne("RentACar.Models.ApplicationUser", "Klijent")
                        .WithMany()
                        .HasForeignKey("KlijentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.Prijevoz")
                        .WithMany("Ocjene")
                        .HasForeignKey("PrijevozID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.PrijevozVozilo", "PrijevozVozilo")
                        .WithMany()
                        .HasForeignKey("PrijevozVoziloID");
                });

            modelBuilder.Entity("RentACar.Models.OcjenaRezervacija", b =>
                {
                    b.HasOne("RentACar.Models.ApplicationUser", "Klijent")
                        .WithMany()
                        .HasForeignKey("KlijentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.Rezervacija", "Rezervacija")
                        .WithMany("Ocjene")
                        .HasForeignKey("RezervacijaID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.Opcina", b =>
                {
                    b.HasOne("RentACar.Models.Regija", "Regija")
                        .WithMany("opcine")
                        .HasForeignKey("RegijaID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.OstecenjeInfo", b =>
                {
                    b.HasOne("RentACar.Models.ApplicationUser", "Mehanicar")
                        .WithMany()
                        .HasForeignKey("MehanicarID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.Rezervacija", "Rezervacija")
                        .WithMany("ostecenjaVozila")
                        .HasForeignKey("RezervacijaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.Vozilo", "Vozilo")
                        .WithMany("ostecenja")
                        .HasForeignKey("VoziloID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.Poslovnica", b =>
                {
                    b.HasOne("RentACar.Models.Grad", "Grad")
                        .WithMany("poslovnice")
                        .HasForeignKey("GradID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.Prijevoz", b =>
                {
                    b.HasOne("RentACar.Models.ApplicationUser", "Klijent")
                        .WithMany()
                        .HasForeignKey("KlijentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.PrijevozVozilo", b =>
                {
                    b.HasOne("RentACar.Models.Prijevoz", "Prijevoz")
                        .WithMany()
                        .HasForeignKey("PrijevozID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.Vozilo", "Vozilo")
                        .WithMany()
                        .HasForeignKey("VoziloID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.Regija", b =>
                {
                    b.HasOne("RentACar.Models.Drzava", "Drzava")
                        .WithMany("regije")
                        .HasForeignKey("DrzavaID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.Rezervacija", b =>
                {
                    b.HasOne("RentACar.Models.ApplicationUser", "Klijent")
                        .WithMany()
                        .HasForeignKey("KlijentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.Poslovnica", "Poslovnica")
                        .WithMany()
                        .HasForeignKey("PoslovnicaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.ApplicationUser", "Uposlenik")
                        .WithMany()
                        .HasForeignKey("UposlenikID");

                    b.HasOne("RentACar.Models.Vozilo", "Vozilo")
                        .WithMany("rezervisano")
                        .HasForeignKey("VoziloID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.RezervisanaUsluga", b =>
                {
                    b.HasOne("RentACar.Models.DodatneUsluge", "DodatneUsluge")
                        .WithMany()
                        .HasForeignKey("DodatneUslugeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.Rezervacija", "Rezervacija")
                        .WithMany()
                        .HasForeignKey("RezervacijaID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.Slika", b =>
                {
                    b.HasOne("RentACar.Models.Vozilo", "Vozilo")
                        .WithMany("slike")
                        .HasForeignKey("VoziloID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.TrenutnaPoslovnica", b =>
                {
                    b.HasOne("RentACar.Models.Poslovnica", "Poslovnica")
                        .WithMany("trenutneposlovnice")
                        .HasForeignKey("PoslovnicaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.Vozilo", "Vozilo")
                        .WithMany("trenutnePoslovnice")
                        .HasForeignKey("VoziloID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.UgovorZaposlenja", b =>
                {
                    b.HasOne("RentACar.Models.Poslovnica", "Poslovnica")
                        .WithMany()
                        .HasForeignKey("PoslovnicaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.ApplicationUser", "Uposlenik")
                        .WithMany()
                        .HasForeignKey("UposlenikID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.UposlenikPrijevoz", b =>
                {
                    b.HasOne("RentACar.Models.Prijevoz", "Prijevoz")
                        .WithMany()
                        .HasForeignKey("PrijevozID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RentACar.Models.ApplicationUser", "Uposlenik")
                        .WithMany()
                        .HasForeignKey("UposlenikID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RentACar.Models.Vozilo", b =>
                {
                    b.HasOne("RentACar.Models.Brend", "Brend")
                        .WithMany()
                        .HasForeignKey("BrendID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
