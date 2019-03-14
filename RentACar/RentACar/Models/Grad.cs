
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class Grad
    {
        public int GradID { get; set; }
        public string Naziv { get; set; }
        public int PostanskiBroj { get; set; }

        public List<Poslovnica> poslovnice { get; set; }

        public int RegijaID { get; set; }
        public Regija Regija { get; set; }

    }
}
