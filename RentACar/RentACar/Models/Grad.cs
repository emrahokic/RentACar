
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class Grad
    {
        public int GradID { get; set; }
        public string Naziv { get; set; }

        public List<Poslovnica> poslovnice { get; set; }

        public int OpcinaID { get; set; }
        public Opcina Opcina { get; set; }

    }
}
