using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class RezervacijaIndexVM
    {
        public List<Row> Rows { get; set; }

        public class Row
        {
            public int RezervacijaID { get; set; }
            public string VrstaRezervacije { get; set; }
            public DateTime DatumPreuzimanja { get; set; }
            public DateTime DatumPovrata { get; set; }
            public int Zakljucen { get; set; }
            
            public int? UposlenikID { get; set; }
            public string Vozilo { get; set; }
            public string Brend { get; set; }
            public string Poslovnica{ get; set; }


        }
    }
}
