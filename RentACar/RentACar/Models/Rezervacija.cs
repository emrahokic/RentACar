using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RentACar.Models
{
    public class Rezervacija
    {
        public int RezervacijaID { get; set; }

        public DateTime DatumRezervacije { get; set; }
        public DateTime DatumRentanja { get; set; }
        public double Cijena { get; set; }
        public string VrstaRezervacije { get; set; }
        public DateTime DatumPreuzimanja { get; set; }
        public DateTime VrijemePreuzimanja { get; set; }
        public DateTime DatumPovrata { get; set; }
        public DateTime VrijemePovrata { get; set; }
        public string NacinPlacanja { get; set; }
        public int BrojDanaIznajmljivanja { get; set; }
        public bool Zakljucen { get; set; }

        public int VoziloID { get; set; }
        public Vozilo Vozilo { get; set; }

        public int PoslovnicaID { get; set; }
        public Poslovnica Poslovnica { get; set; }

        public int KlijentID { get; set; }
        public ApplicationUser Klijent { get; set; }

        public int? UposlenikID { get; set; }
        public ApplicationUser Uposlenik { get; set; }

        public List<OstecenjeInfo> ostecenjaVozila { get; set; }

        public List<OcjenaRezervacija> Ocjene { get; set; }

    }
}
