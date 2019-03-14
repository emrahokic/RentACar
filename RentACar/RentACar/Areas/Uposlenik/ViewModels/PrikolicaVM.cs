using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class PrikolicaVM
    {
        public List<Row> rows { get; set; }
        public class Row
        {
            public int PrikolicaID { get; set; }
            public double Sirina { get; set; }
            public double Zapremina { get; set; }
            public double Duzina { get; set; }
            public int TipPrikolice { get; set; }
        }
    }
}
