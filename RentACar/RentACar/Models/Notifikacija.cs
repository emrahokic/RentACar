using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class Notifikacija
    {
        public int NotifikacijaID { get; set; }
        public DateTime Vrijeme { get; set; }
        public bool Otvorena { get; set; }
        public string Poruka { get; set; }

        public int? PoslovnicaID { get; set; }
        public Poslovnica Poslovnica { get; set; }

        public int? RezervacijaID { get; set; }
        public Rezervacija Rezervacija { get; set; }

        public int? UserID { get; set; }
        public ApplicationUser User { get; set; }
    }
}
