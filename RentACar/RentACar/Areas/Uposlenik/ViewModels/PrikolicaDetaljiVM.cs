using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class PrikolicaDetaljiVM
    {
        public int PrikolicaID { get; set; }
        public double Sirina { get; set; }
        public double  Zapremina { get; set; }
        public double Duzina { get; set; }
        public int TipPrikolice { get; set; }
        public double Cijena { get; set; }

        public List<Row> rows { get; set; }
        public class Row
        {
            public int VoziloID { get; set; }
            public string TipKuke { get; set; }
            public string Tezina { get; set; }
            public string Auto { get; set; }
            public int tipAuta { get; set; }
        }
    }
}
