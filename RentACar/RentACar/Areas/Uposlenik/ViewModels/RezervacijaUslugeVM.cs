using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class RezervacijaUslugeVM
    {
        public int RezervacijaID { get; set; }
        public List<SelectListItem> usluge { get; set; }
    }
}
