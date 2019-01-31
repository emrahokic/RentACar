using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class Opcina
    {
        public int OpcinaID { get; set; }
        public string Naziv { get; set; }
        public int PostanskiBroj { get; set; }

        public List<Grad> gradovi { get; set; }

        public int RegijaID { get; set; }
        public Regija Regija { get; set; }
    }
}
