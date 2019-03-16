using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class RezervacijaSredinaVM
    {
        public int RezervacijaID { get; set; }
        public string Vozilo { get; set; }
        public string Korisnik { get; set; }
        public string DatumPreuzimanja { get; set; }
        public string DatumPovrata { get; set; }
        public double Cijena { get; set; }
        public List<Row> dodatneUsluge { get; set; }
        public class Row
        {
            public string Naziv { get; set; }
            public string Opis { get; set; }
            public double Cijena { get; set; }
            public int Kolicina { get; set; }
            public double Ukupno { get; set; }

        }
    }
}
