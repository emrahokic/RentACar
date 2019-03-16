using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class RezervacijaPocetakVM
    {
        public int VoziloID { get; set; }
        public string  NazivVozila { get; set; }
        public string UposlenikID { get; set; }
        public List<SelectListItem> nacinPlacanja { get; set; }
    }
}
