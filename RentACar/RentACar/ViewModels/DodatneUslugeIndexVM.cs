using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.ViewModels
{
    public class DodatneUslugeIndexVM
    {
        public List<Rows> rows { get; set; }
    }

    public class Rows
    {
        public int DodatneUslugeID { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public double Cijena { get; set; }

    }
}
