using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class RezervisanaUsluga
    {
        public int RezervisanaUslugaID { get; set; }

        public double UkupnaCijenaUsluge { get; set; }
        public int Kolicina { get; set; }
        public string Opis { get; set; }

        public int RezervacijaID { get; set; }
        public Rezervacija Rezervacija { get; set; }
        public int DodatneUslugeID { get; set; }
        public DodatneUsluge DodatneUsluge { get; set; }



    }
}
