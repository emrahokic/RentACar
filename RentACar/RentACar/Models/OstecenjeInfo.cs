using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class OstecenjeInfo
    {
        public int OstecenjeInfoID { get; set; }
        public DateTime DatumZapisa { get; set; }
        public string Opis { get; set; }
        public bool FizickoOstecenje { get; set; }

        public int RezervacijaID { get; set; }
        public Rezervacija Rezervacija { get; set; }

        public int MehanicarID { get; set; }
        public ApplicationUser Mehanicar { get; set; }

        public int VoziloID { get; set; }
        public Vozilo Vozilo { get; set; }

    }
}
