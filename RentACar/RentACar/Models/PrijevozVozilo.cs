using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class PrijevozVozilo
    {
        public int PrijevozVoziloID { get; set; }

        public int PocetnaKilometraza { get; set; }
        public int ZavrsnaKilometraza { get; set; }

        public int PrijevozID { get; set; }
        public Prijevoz Prijevoz { get; set; }

        public int VoziloID { get; set; }
        public Vozilo Vozilo { get; set; }

    }
}
