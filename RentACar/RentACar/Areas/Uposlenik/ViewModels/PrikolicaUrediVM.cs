using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class PrikolicaUrediVM
    {
        public int PrikolicaID { get; set; }
        public double Sirina { get; set; }
        public double Zapremina { get; set; }
        public double Duzina { get; set; }
        public List<SelectListItem> TipPrikolice { get; set; }
        public double Cijena { get; set; }
    }
}
