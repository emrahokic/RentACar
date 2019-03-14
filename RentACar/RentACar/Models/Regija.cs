using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class Regija
    {
        public int RegijaID { get; set; }
        public string Naziv { get; set; }

        public int DrzavaID { get; set; }
        public Drzava Drzava { get; set; }

    }
}
