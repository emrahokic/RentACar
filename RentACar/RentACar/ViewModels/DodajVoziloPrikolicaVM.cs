using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.ViewModels
{
    public class DodajVoziloPrikolicaVM
    {
        public int PrikolicaID { get; set; }
        public List<SelectListItem> vozila { get; set; }
    }
}
