using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class UgovorZaposlenja
    {
        public int UgovorZaposlenjaID { get; set; }

        public int UposlenikID { get; set; }
        public ApplicationUser Uposlenik{ get; set; }
        public int PoslovnicaID { get; set; }
        public Poslovnica Poslovnica { get; set; }

        public DateTime DatumZaposlenja { get; set; }
        public string RadnoMjesto{ get; set; }

    }
}
