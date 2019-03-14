using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class RezervacijaDodajVM
    {
        public DateTime DatumRezervacije { get; set; }
        public DateTime DatumRentanja { get; set; }
        public string VrstaRezervacije { get; set; }
        public DateTime DatumPreuzimanja { get; set; }
        public DateTime DatumPovrata { get; set; }
        public string NacinPlacanja { get; set; }
        public int VoziloID { get; set; }
        public int PoslovnicaID { get; set; }
        public List<SelectListItem> Vozila{ get; set; }

    }
}
