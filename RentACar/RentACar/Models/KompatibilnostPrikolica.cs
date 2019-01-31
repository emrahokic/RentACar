using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class KompatibilnostPrikolica
    {
        public int KompatibilnostPrikolicaID { get; set; }

        public string TipKuke { get; set; }
        public string Tezina { get; set; }

        public int PrikolicaID { get; set; }
        public Prikolica Prikolica { get; set; }
        public int VoziloID { get; set; }
        public Vozilo Vozilo { get; set; }


    }
}
