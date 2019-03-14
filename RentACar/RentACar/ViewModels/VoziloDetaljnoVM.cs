using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.ViewModels
{
    public class VoziloDetaljnoVM
    {
        public int VoziloID { get; set; }
        public int BrojSasije { get; set; }
        public string RegistarskaOznaka { get; set; }
        public string Boja { get; set; }
        public string Model { get; set; }
        public int GodinaProizvodnje { get; set; }
        public int SnagaMotora { get; set; }
        public string DatumMijenjanjUlja { get; set; }
        public int Domet { get; set; }
        public int BrojMjesta { get; set; }
        public int BrojVrata { get; set; }
        public double ZapreminaPrtljaznika { get; set; }
        public double ZapreminaPrtljaznikaNaprijed { get; set; }
        public string Naziv { get; set; }
        public string Brend { get; set; }
        public bool Klima { get; set; }
        public int TipVozila { get; set; }
        public string DodatniOpis { get; set; }
        public int Gorivo { get; set; }
        public string Pogon { get; set; }
        public int Transmisija { get; set; }
        public int GrupniTipVozila { get; set; }
        public List<string> Slike { get; set; }
        public int Kilometraza { get; set; }

        public List<Row> prikolice { get; set; }
        
        public class Row
        {
            public int PrikolicaID { get; set; }
            public double Sirina { get; set; }
            public double Zapremina { get; set; }
            public double Duzina { get; set; }
            public int TipPrikolice { get; set; }
            public string TipKuke { get; set; }
            public string Tezina { get; set; }
        }
    }
}
