using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class OcjenaRezervacija
    {

        public int OcjenaRezervacijaID { get; set; }
        public int OcjenaVrijednost { get; set; }
        public string Poruka { get; set; }

        public int RezervacijaID { get; set; }
        public Rezervacija Rezervacija { get; set; }

        public int KlijentID { get; set; }
        public ApplicationUser Klijent { get; set; }
    }
}
