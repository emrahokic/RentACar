using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class TrenutnaPoslovnica
    {
        public int TrenutnaPoslovnicaID { get; set; }
        public DateTime DatumUlaza { get; set; }
        public DateTime? DatumIzlaza { get; set; }

        public int VoziloID { get; set; }
        public Vozilo Vozilo { get; set; }

        public int PoslovnicaID { get; set; }
        public Poslovnica Poslovnica { get; set; }

    }
}
