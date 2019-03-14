using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class Prijevoz
    {
        public int PrijevozID { get; set; }

        public DateTime DatumRezervacije { get; set; }
        public DateTime DatumPrijevoza { get; set; }
        public double CijenaPoKilometru { get; set; }
        public double CijenaCekanjaPoSatu { get; set; }
        public double CijenaPoVozacu { get; set; }
        public int NacinPlacanja { get; set; }
        public int TipPrijevoza { get; set; }
        public int BrojVozila { get; set; }
        public int BrojVozaca { get; set; }

        public int KlijentID { get; set; }
        public ApplicationUser Klijent { get; set; }

        public List<OcjenaPrijevoz> Ocjene { get; set; }


    }
}
