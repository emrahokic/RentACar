using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class PrijevozDodajVoziloVM
    {
        public int PrijevozID { get; set; }
        public string klijent { get; set; }
        public string jmbg { get; set; }
        public string DatumPrijevoza { get; set; }
        public int NacinPlacanja { get; set; }
        public int TipPrijevoza { get; set; }
        public double CijenaSat { get; set; }
        public double CijenaKilometar { get; set; }
        public double CijenaVozac { get; set; }

        public List<Row1> vozila { get; set; }
        public List<Row2> vozaci { get; set; }
        public class Row1
        {
            public int VoziloID { get; set; }
            public string slika { get; set; }
            public string Naziv { get; set; }
            public string GodinaProizvodnje { get; set; }
        }

        public class Row2
        {
            public int VozacID { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string  Mail { get; set; }
        }
    }

}
