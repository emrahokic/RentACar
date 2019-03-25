using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class VoziloRezervacijaVM
    {
        public List<Row> vozila { get; set; }
        public class Row
        {
            public int VoziloID { get; set; }
            public string slike { get; set; }
            public string Boja { get; set; }
            public string Model { get; set; }
            public int GodinaProizvodnje { get; set; }
            public int SnagaMotora { get; set; }
            public int Domet { get; set; }
            public int BrojMjesta { get; set; }
            public int BrojVrata { get; set; }
            public string Naziv { get; set; }
            public string Brend { get; set; }
            public bool Klima { get; set; }
            public int TipVozila { get; set; }
            public int Gorivo { get; set; }
            public int Transmisija { get; set; }
            public int GrupniTipVozila { get; set; }
            public double Cijena { get; set; }
        }
    }
}
