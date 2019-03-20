using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RentACar.Migrations
{
    public partial class initital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brend",
                columns: table => new
                {
                    BrendID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brend", x => x.BrendID);
                });

            migrationBuilder.CreateTable(
                name: "DodatneUsluge",
                columns: table => new
                {
                    DodatneUslugeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Opis = table.Column<string>(nullable: true),
                    Cijena = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DodatneUsluge", x => x.DodatneUslugeID);
                });

            migrationBuilder.CreateTable(
                name: "Drzava",
                columns: table => new
                {
                    DrzavaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Skracenica = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drzava", x => x.DrzavaID);
                });

            migrationBuilder.CreateTable(
                name: "Prikolica",
                columns: table => new
                {
                    PrikolicaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Sirina = table.Column<double>(nullable: false),
                    Zapremina = table.Column<double>(nullable: false),
                    Duzina = table.Column<double>(nullable: false),
                    TipPrikolice = table.Column<int>(nullable: false),
                    Cijna = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prikolica", x => x.PrikolicaID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Vozilo",
                columns: table => new
                {
                    VoziloID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrojSasije = table.Column<int>(nullable: false),
                    RegistarskaOznaka = table.Column<string>(nullable: true),
                    Boja = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    GodinaProizvodnje = table.Column<int>(nullable: false),
                    SnagaMotora = table.Column<int>(nullable: false),
                    DatumMijenjanjUlja = table.Column<DateTime>(nullable: false),
                    Domet = table.Column<int>(nullable: false),
                    BrojMjesta = table.Column<int>(nullable: false),
                    BrojVrata = table.Column<int>(nullable: false),
                    ZapreminaPrtljaznika = table.Column<double>(nullable: false),
                    ZapreminaPrtljaznikaNaprijed = table.Column<double>(nullable: false),
                    Naziv = table.Column<string>(nullable: true),
                    Kilometraza = table.Column<int>(nullable: false),
                    Kuka = table.Column<bool>(nullable: false),
                    BrendID = table.Column<int>(nullable: false),
                    Cijena = table.Column<double>(nullable: false),
                    Klima = table.Column<bool>(nullable: false),
                    TipVozila = table.Column<int>(nullable: false),
                    DodatniOpis = table.Column<string>(nullable: true),
                    Gorivo = table.Column<int>(nullable: false),
                    Pogon = table.Column<string>(nullable: true),
                    Transmisija = table.Column<int>(nullable: false),
                    GrupniTipVozila = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vozilo", x => x.VoziloID);
                    table.ForeignKey(
                        name: "FK_Vozilo_Brend_BrendID",
                        column: x => x.BrendID,
                        principalTable: "Brend",
                        principalColumn: "BrendID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Regija",
                columns: table => new
                {
                    RegijaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    DrzavaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regija", x => x.RegijaID);
                    table.ForeignKey(
                        name: "FK_Regija_Drzava_DrzavaID",
                        column: x => x.DrzavaID,
                        principalTable: "Drzava",
                        principalColumn: "DrzavaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                columns: table => new
                {
                    RoleClaimID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleID = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.RoleClaimID);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KompatibilnostPrikolica",
                columns: table => new
                {
                    KompatibilnostPrikolicaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TipKuke = table.Column<string>(nullable: true),
                    Tezina = table.Column<string>(nullable: true),
                    PrikolicaID = table.Column<int>(nullable: false),
                    VoziloID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KompatibilnostPrikolica", x => x.KompatibilnostPrikolicaID);
                    table.ForeignKey(
                        name: "FK_KompatibilnostPrikolica_Prikolica_PrikolicaID",
                        column: x => x.PrikolicaID,
                        principalTable: "Prikolica",
                        principalColumn: "PrikolicaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KompatibilnostPrikolica_Vozilo_VoziloID",
                        column: x => x.VoziloID,
                        principalTable: "Vozilo",
                        principalColumn: "VoziloID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Slika",
                columns: table => new
                {
                    SlikaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    Pozicija = table.Column<int>(nullable: false),
                    VoziloID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slika", x => x.SlikaID);
                    table.ForeignKey(
                        name: "FK_Slika_Vozilo_VoziloID",
                        column: x => x.VoziloID,
                        principalTable: "Vozilo",
                        principalColumn: "VoziloID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grad",
                columns: table => new
                {
                    GradID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    PostanskiBroj = table.Column<int>(nullable: false),
                    RegijaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grad", x => x.GradID);
                    table.ForeignKey(
                        name: "FK_Grad_Regija_RegijaID",
                        column: x => x.RegijaID,
                        principalTable: "Regija",
                        principalColumn: "RegijaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Poslovnica",
                columns: table => new
                {
                    PoslovnicaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Adresa = table.Column<string>(nullable: true),
                    BrojTelefona = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    GradID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poslovnica", x => x.PoslovnicaID);
                    table.ForeignKey(
                        name: "FK_Poslovnica_Grad_GradID",
                        column: x => x.GradID,
                        principalTable: "Grad",
                        principalColumn: "GradID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Ime = table.Column<string>(maxLength: 50, nullable: false),
                    Slika = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(maxLength: 50, nullable: false),
                    DatumRodjenja = table.Column<DateTime>(nullable: false),
                    GradID = table.Column<int>(nullable: true),
                    Adresa = table.Column<string>(nullable: true),
                    DatumRegistracije = table.Column<DateTime>(nullable: false),
                    JMBG = table.Column<string>(nullable: true),
                    Spol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_User_Grad_GradID",
                        column: x => x.GradID,
                        principalTable: "Grad",
                        principalColumn: "GradID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrenutnaPoslovnica",
                columns: table => new
                {
                    TrenutnaPoslovnicaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumUlaza = table.Column<DateTime>(nullable: false),
                    DatumIzlaza = table.Column<DateTime>(nullable: true),
                    VoziloRezervisano = table.Column<bool>(nullable: false),
                    VoziloID = table.Column<int>(nullable: false),
                    PoslovnicaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrenutnaPoslovnica", x => x.TrenutnaPoslovnicaID);
                    table.ForeignKey(
                        name: "FK_TrenutnaPoslovnica_Poslovnica_PoslovnicaID",
                        column: x => x.PoslovnicaID,
                        principalTable: "Poslovnica",
                        principalColumn: "PoslovnicaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrenutnaPoslovnica_Vozilo_VoziloID",
                        column: x => x.VoziloID,
                        principalTable: "Vozilo",
                        principalColumn: "VoziloID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prijevoz",
                columns: table => new
                {
                    PrijevozID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumRezervacije = table.Column<DateTime>(nullable: false),
                    DatumPrijevoza = table.Column<DateTime>(nullable: false),
                    CijenaPoKilometru = table.Column<double>(nullable: false),
                    CijenaCekanjaPoSatu = table.Column<double>(nullable: false),
                    CijenaPoVozacu = table.Column<double>(nullable: false),
                    NacinPlacanja = table.Column<int>(nullable: false),
                    TipPrijevoza = table.Column<int>(nullable: false),
                    BrojVozila = table.Column<int>(nullable: false),
                    BrojVozaca = table.Column<int>(nullable: false),
                    KlijentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prijevoz", x => x.PrijevozID);
                    table.ForeignKey(
                        name: "FK_Prijevoz_User_KlijentID",
                        column: x => x.KlijentID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacija",
                columns: table => new
                {
                    RezervacijaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumRezervacije = table.Column<DateTime>(nullable: false),
                    Cijena = table.Column<double>(nullable: false),
                    DatumPreuzimanja = table.Column<DateTime>(nullable: false),
                    DatumPovrata = table.Column<DateTime>(nullable: false),
                    NacinPlacanja = table.Column<int>(nullable: false),
                    BrojDanaIznajmljivanja = table.Column<int>(nullable: false),
                    Zakljucen = table.Column<int>(nullable: false),
                    SifraRezervacije = table.Column<string>(nullable: true),
                    UspjesnoSpremljena = table.Column<bool>(nullable: true),
                    VoziloID = table.Column<int>(nullable: false),
                    PoslovnicaID = table.Column<int>(nullable: false),
                    KlijentID = table.Column<int>(nullable: false),
                    PrikolicaID = table.Column<int>(nullable: true),
                    UposlenikID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacija", x => x.RezervacijaID);
                    table.ForeignKey(
                        name: "FK_Rezervacija_User_KlijentID",
                        column: x => x.KlijentID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervacija_Poslovnica_PoslovnicaID",
                        column: x => x.PoslovnicaID,
                        principalTable: "Poslovnica",
                        principalColumn: "PoslovnicaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervacija_Prikolica_PrikolicaID",
                        column: x => x.PrikolicaID,
                        principalTable: "Prikolica",
                        principalColumn: "PrikolicaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervacija_User_UposlenikID",
                        column: x => x.UposlenikID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervacija_Vozilo_VoziloID",
                        column: x => x.VoziloID,
                        principalTable: "Vozilo",
                        principalColumn: "VoziloID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UgovorZaposlenja",
                columns: table => new
                {
                    UgovorZaposlenjaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UposlenikID = table.Column<int>(nullable: false),
                    PoslovnicaID = table.Column<int>(nullable: false),
                    DatumZaposlenja = table.Column<DateTime>(nullable: false),
                    RadnoMjesto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UgovorZaposlenja", x => x.UgovorZaposlenjaID);
                    table.ForeignKey(
                        name: "FK_UgovorZaposlenja_Poslovnica_PoslovnicaID",
                        column: x => x.PoslovnicaID,
                        principalTable: "Poslovnica",
                        principalColumn: "PoslovnicaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UgovorZaposlenja_User_UposlenikID",
                        column: x => x.UposlenikID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    UserClaimID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.UserClaimID);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    RoleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.UserID, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OcjenaPrijevoz",
                columns: table => new
                {
                    OcjenaPrijevozID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OcjenaVrijednost = table.Column<int>(nullable: false),
                    Poruka = table.Column<string>(nullable: true),
                    DatumOcjene = table.Column<DateTime>(nullable: false),
                    PrijevozID = table.Column<int>(nullable: false),
                    KlijentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OcjenaPrijevoz", x => x.OcjenaPrijevozID);
                    table.ForeignKey(
                        name: "FK_OcjenaPrijevoz_User_KlijentID",
                        column: x => x.KlijentID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OcjenaPrijevoz_Prijevoz_PrijevozID",
                        column: x => x.PrijevozID,
                        principalTable: "Prijevoz",
                        principalColumn: "PrijevozID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrijevozVozilo",
                columns: table => new
                {
                    PrijevozVoziloID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PocetnaKilometraza = table.Column<int>(nullable: false),
                    ZavrsnaKilometraza = table.Column<int>(nullable: false),
                    PrijevozID = table.Column<int>(nullable: false),
                    VoziloID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrijevozVozilo", x => x.PrijevozVoziloID);
                    table.ForeignKey(
                        name: "FK_PrijevozVozilo_Prijevoz_PrijevozID",
                        column: x => x.PrijevozID,
                        principalTable: "Prijevoz",
                        principalColumn: "PrijevozID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrijevozVozilo_Vozilo_VoziloID",
                        column: x => x.VoziloID,
                        principalTable: "Vozilo",
                        principalColumn: "VoziloID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UposlenikPrijevoz",
                columns: table => new
                {
                    UposlenikPrijevozID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UposlenikID = table.Column<int>(nullable: false),
                    PrijevozID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UposlenikPrijevoz", x => x.UposlenikPrijevozID);
                    table.ForeignKey(
                        name: "FK_UposlenikPrijevoz_Prijevoz_PrijevozID",
                        column: x => x.PrijevozID,
                        principalTable: "Prijevoz",
                        principalColumn: "PrijevozID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UposlenikPrijevoz_User_UposlenikID",
                        column: x => x.UposlenikID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifikacija",
                columns: table => new
                {
                    NotifikacijaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Vrijeme = table.Column<DateTime>(nullable: false),
                    Otvorena = table.Column<bool>(nullable: false),
                    Poruka = table.Column<string>(nullable: true),
                    PoslovnicaID = table.Column<int>(nullable: true),
                    RezervacijaID = table.Column<int>(nullable: true),
                    UserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifikacija", x => x.NotifikacijaID);
                    table.ForeignKey(
                        name: "FK_Notifikacija_Poslovnica_PoslovnicaID",
                        column: x => x.PoslovnicaID,
                        principalTable: "Poslovnica",
                        principalColumn: "PoslovnicaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifikacija_Rezervacija_RezervacijaID",
                        column: x => x.RezervacijaID,
                        principalTable: "Rezervacija",
                        principalColumn: "RezervacijaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifikacija_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OcjenaRezervacija",
                columns: table => new
                {
                    OcjenaRezervacijaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OcjenaVrijednost = table.Column<int>(nullable: false),
                    Poruka = table.Column<string>(nullable: true),
                    DatumOcjene = table.Column<DateTime>(nullable: false),
                    RezervacijaID = table.Column<int>(nullable: false),
                    KlijentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OcjenaRezervacija", x => x.OcjenaRezervacijaID);
                    table.ForeignKey(
                        name: "FK_OcjenaRezervacija_User_KlijentID",
                        column: x => x.KlijentID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OcjenaRezervacija_Rezervacija_RezervacijaID",
                        column: x => x.RezervacijaID,
                        principalTable: "Rezervacija",
                        principalColumn: "RezervacijaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OstecenjeInfo",
                columns: table => new
                {
                    OstecenjeInfoID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumZapisa = table.Column<DateTime>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    FizickoOstecenje = table.Column<bool>(nullable: false),
                    RezervacijaID = table.Column<int>(nullable: false),
                    MehanicarID = table.Column<int>(nullable: false),
                    VoziloID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OstecenjeInfo", x => x.OstecenjeInfoID);
                    table.ForeignKey(
                        name: "FK_OstecenjeInfo_User_MehanicarID",
                        column: x => x.MehanicarID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OstecenjeInfo_Rezervacija_RezervacijaID",
                        column: x => x.RezervacijaID,
                        principalTable: "Rezervacija",
                        principalColumn: "RezervacijaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OstecenjeInfo_Vozilo_VoziloID",
                        column: x => x.VoziloID,
                        principalTable: "Vozilo",
                        principalColumn: "VoziloID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RezervisanaUsluga",
                columns: table => new
                {
                    RezervisanaUslugaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UkupnaCijenaUsluge = table.Column<double>(nullable: false),
                    Kolicina = table.Column<int>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    RezervacijaID = table.Column<int>(nullable: false),
                    DodatneUslugeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervisanaUsluga", x => x.RezervisanaUslugaID);
                    table.ForeignKey(
                        name: "FK_RezervisanaUsluga_DodatneUsluge_DodatneUslugeID",
                        column: x => x.DodatneUslugeID,
                        principalTable: "DodatneUsluge",
                        principalColumn: "DodatneUslugeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RezervisanaUsluga_Rezervacija_RezervacijaID",
                        column: x => x.RezervacijaID,
                        principalTable: "Rezervacija",
                        principalColumn: "RezervacijaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grad_RegijaID",
                table: "Grad",
                column: "RegijaID");

            migrationBuilder.CreateIndex(
                name: "IX_KompatibilnostPrikolica_PrikolicaID",
                table: "KompatibilnostPrikolica",
                column: "PrikolicaID");

            migrationBuilder.CreateIndex(
                name: "IX_KompatibilnostPrikolica_VoziloID",
                table: "KompatibilnostPrikolica",
                column: "VoziloID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_PoslovnicaID",
                table: "Notifikacija",
                column: "PoslovnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_RezervacijaID",
                table: "Notifikacija",
                column: "RezervacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_UserID",
                table: "Notifikacija",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_OcjenaPrijevoz_KlijentID",
                table: "OcjenaPrijevoz",
                column: "KlijentID");

            migrationBuilder.CreateIndex(
                name: "IX_OcjenaPrijevoz_PrijevozID",
                table: "OcjenaPrijevoz",
                column: "PrijevozID");

            migrationBuilder.CreateIndex(
                name: "IX_OcjenaRezervacija_KlijentID",
                table: "OcjenaRezervacija",
                column: "KlijentID");

            migrationBuilder.CreateIndex(
                name: "IX_OcjenaRezervacija_RezervacijaID",
                table: "OcjenaRezervacija",
                column: "RezervacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_OstecenjeInfo_MehanicarID",
                table: "OstecenjeInfo",
                column: "MehanicarID");

            migrationBuilder.CreateIndex(
                name: "IX_OstecenjeInfo_RezervacijaID",
                table: "OstecenjeInfo",
                column: "RezervacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_OstecenjeInfo_VoziloID",
                table: "OstecenjeInfo",
                column: "VoziloID");

            migrationBuilder.CreateIndex(
                name: "IX_Poslovnica_GradID",
                table: "Poslovnica",
                column: "GradID");

            migrationBuilder.CreateIndex(
                name: "IX_Prijevoz_KlijentID",
                table: "Prijevoz",
                column: "KlijentID");

            migrationBuilder.CreateIndex(
                name: "IX_PrijevozVozilo_PrijevozID",
                table: "PrijevozVozilo",
                column: "PrijevozID");

            migrationBuilder.CreateIndex(
                name: "IX_PrijevozVozilo_VoziloID",
                table: "PrijevozVozilo",
                column: "VoziloID");

            migrationBuilder.CreateIndex(
                name: "IX_Regija_DrzavaID",
                table: "Regija",
                column: "DrzavaID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_KlijentID",
                table: "Rezervacija",
                column: "KlijentID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_PoslovnicaID",
                table: "Rezervacija",
                column: "PoslovnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_PrikolicaID",
                table: "Rezervacija",
                column: "PrikolicaID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_UposlenikID",
                table: "Rezervacija",
                column: "UposlenikID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_VoziloID",
                table: "Rezervacija",
                column: "VoziloID");

            migrationBuilder.CreateIndex(
                name: "IX_RezervisanaUsluga_DodatneUslugeID",
                table: "RezervisanaUsluga",
                column: "DodatneUslugeID");

            migrationBuilder.CreateIndex(
                name: "IX_RezervisanaUsluga_RezervacijaID",
                table: "RezervisanaUsluga",
                column: "RezervacijaID");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleID",
                table: "RoleClaim",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Slika_VoziloID",
                table: "Slika",
                column: "VoziloID");

            migrationBuilder.CreateIndex(
                name: "IX_TrenutnaPoslovnica_PoslovnicaID",
                table: "TrenutnaPoslovnica",
                column: "PoslovnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_TrenutnaPoslovnica_VoziloID",
                table: "TrenutnaPoslovnica",
                column: "VoziloID");

            migrationBuilder.CreateIndex(
                name: "IX_UgovorZaposlenja_PoslovnicaID",
                table: "UgovorZaposlenja",
                column: "PoslovnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_UgovorZaposlenja_UposlenikID",
                table: "UgovorZaposlenja",
                column: "UposlenikID");

            migrationBuilder.CreateIndex(
                name: "IX_UposlenikPrijevoz_PrijevozID",
                table: "UposlenikPrijevoz",
                column: "PrijevozID");

            migrationBuilder.CreateIndex(
                name: "IX_UposlenikPrijevoz_UposlenikID",
                table: "UposlenikPrijevoz",
                column: "UposlenikID");

            migrationBuilder.CreateIndex(
                name: "IX_User_GradID",
                table: "User",
                column: "GradID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserID",
                table: "UserClaim",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserID",
                table: "UserLogin",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleID",
                table: "UserRole",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Vozilo_BrendID",
                table: "Vozilo",
                column: "BrendID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KompatibilnostPrikolica");

            migrationBuilder.DropTable(
                name: "Notifikacija");

            migrationBuilder.DropTable(
                name: "OcjenaPrijevoz");

            migrationBuilder.DropTable(
                name: "OcjenaRezervacija");

            migrationBuilder.DropTable(
                name: "OstecenjeInfo");

            migrationBuilder.DropTable(
                name: "PrijevozVozilo");

            migrationBuilder.DropTable(
                name: "RezervisanaUsluga");

            migrationBuilder.DropTable(
                name: "RoleClaim");

            migrationBuilder.DropTable(
                name: "Slika");

            migrationBuilder.DropTable(
                name: "TrenutnaPoslovnica");

            migrationBuilder.DropTable(
                name: "UgovorZaposlenja");

            migrationBuilder.DropTable(
                name: "UposlenikPrijevoz");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "DodatneUsluge");

            migrationBuilder.DropTable(
                name: "Rezervacija");

            migrationBuilder.DropTable(
                name: "Prijevoz");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Poslovnica");

            migrationBuilder.DropTable(
                name: "Prikolica");

            migrationBuilder.DropTable(
                name: "Vozilo");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Brend");

            migrationBuilder.DropTable(
                name: "Grad");

            migrationBuilder.DropTable(
                name: "Regija");

            migrationBuilder.DropTable(
                name: "Drzava");
        }
    }
}
