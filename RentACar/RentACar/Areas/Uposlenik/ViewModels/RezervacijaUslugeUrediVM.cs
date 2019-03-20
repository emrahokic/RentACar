using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class RezervacijaUslugeUrediVM
    {
        public List<Row> rows { get; set; }
       public class Row
        {
            public int RezervacijaID { get; set; }
            public int UslugaID { get; set; }
            public string Naziv { get; set; }
            public string Opis { get; set; }
            public double Cijena { get; set; }
            public int Kolicina { get; set; }
            public double Ukupno { get; set; }
        }
    }
}
