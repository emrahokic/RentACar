using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class Drzava
    {
        public int DrzavaID { get; set; }
        public string Naziv { get; set; }
        public string Skracenica { get; set; }

        public List<Regija> regije { get; set; }

    }
}
