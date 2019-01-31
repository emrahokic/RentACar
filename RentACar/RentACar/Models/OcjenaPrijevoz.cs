﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class OcjenaPrijevoz
    {
        public int OcjenaPrijevozID { get; set; }
        public int OcjenaVrijednost { get; set; }
        public string Poruka { get; set; }

       
        public int PrijevozID { get; set; }
        public PrijevozVozilo PrijevozVozilo { get; set; }

        public int KlijentID { get; set; }
        public ApplicationUser Klijent { get; set; }
    }
}
