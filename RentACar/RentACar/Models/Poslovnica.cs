using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class Poslovnica
    {
        public int PoslovnicaID { get; set; }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }

        public List<TrenutnaPoslovnica> trenutneposlovnice { get; set; }


        public int GradID { get; set; }
        public Grad Grad { get; set; }


    }
}
