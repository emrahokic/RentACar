using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class PrijevoziVM
    {
        public List<Row> prijevozi { get; set; }
        public class Row
        {
            public int PrijevozID { get; set; }
            public string Klijent { get; set; }
            public string DatumPrijevoza { get; set; }
            public string TipPrijevoza { get; set; }
            public int Zavrsna { get; set; }
        }
    }
}
