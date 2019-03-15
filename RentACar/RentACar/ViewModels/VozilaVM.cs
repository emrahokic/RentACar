using RentACar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.ViewModels
{
    //ovaj ostaje ovdje
    public class VozilaVM
    {
        public string GradSearch { get; set; }
        public List<Row> rows { get; set; }
        public class Row
        {
            public int VoziloID { get; set; }
            public string Brend { get; set; }
            public string Naziv { get; set; }
            public int TipVozila { get; set; }
            public int Transmisija { get; set; }
            public int Gorivo { get; set; }
            public Slika Slika { get; set; }
            public int PoslovnicaID { get; set; }
            public string PoslovnicaNaziv { get; set; }
            public string PoslovnicaLokacija { get; set; }
        }
    }
}
