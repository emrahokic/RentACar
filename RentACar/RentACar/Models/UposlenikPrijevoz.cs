using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Models
{
    public class UposlenikPrijevoz
    {
        public int UposlenikPrijevozID { get; set; }

        public int BrojVozaca { get; set; }
        public int UposlenikID { get; set; }
        public ApplicationUser Uposlenik { get; set; }
        public int PrijevozID { get; set; }
        public Prijevoz Prijevoz { get; set; }

    }
}
