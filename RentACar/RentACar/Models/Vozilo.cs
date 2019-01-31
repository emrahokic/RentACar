using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class Vozilo
    {

        public int VoziloID { get; set; } //Id od Vozila

        public int BrojSasije { get; set; }
        public string RegistarskaOznaka { get; set; }
        public string Boja { get; set; }
        public string Model { get; set; }
        public int GodinaProizvodnje { get; set; }
        public int SnagaMotora { get; set; }
        public DateTime DatumMijenjanjUlja { get; set; }
        public int Domet { get; set; }
        public int BrojMjesta { get; set; }
        public int BrojVrata { get; set; }
        public double ZapreminaPrtljaznika { get; set; }
        public double ZapreminaPrtljaznikaNaprijed { get; set; }
        public string Naziv { get; set; }

        public int BrendID { get; set; }
        public Brend Brend { get; set; }

        public bool Klima { get; set; }
        public string TipVozila { get; set; }
        public string DodatniOpis { get; set; }
        public string Gorivo { get; set; }
        public string Pogon { get; set; }
        public string Transmisija { get; set; }
        public string GrupniTipVozila { get; set; }


        public List<Rezervacija> rezervisano { get; set; }

        public List<TrenutnaPoslovnica> trenutnePoslovnice { get; set; }

        public List<OstecenjeInfo> ostecenja { get; set; }
        
        public List<Slika> slike { get; set; }



    }
}
