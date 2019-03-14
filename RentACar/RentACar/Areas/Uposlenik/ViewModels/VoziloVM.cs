using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class VoziloVM
    {
        public List<Row> rows { get; set; }
        public class Row
        {
            public int VoziloID { get; set; }
            public string Brend { get; set; }
            public string Naziv { get; set; }
            public string Model { get; set; }
            public int BrojVrata { get; set; }
            public int TipVozila { get; set; }
            public int Transmisija { get; set; }
        }
    }
}
