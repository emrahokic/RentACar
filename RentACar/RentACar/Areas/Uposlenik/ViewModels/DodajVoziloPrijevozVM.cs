using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class DodajVoziloPrijevozVM
    {
        public int PrijevozID { get; set; }
        public List<SelectListItem> vozila { get; set; }
    }
}
