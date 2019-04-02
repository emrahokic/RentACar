using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{
    public class PrijevozNoviVM
    {
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        public string Email { get; set; }
        [Required]
        public string DatumRodjenja { get; set; }
        public string Adresa { get; set; }
        [Required]
        public string jmbg { get; set; }
        [Required]
        public string Spol { get; set; }
        [Required]
        public string DatumPrijevoza { get; set; }
        [Required]
        public int NacinPlacanja { get; set; }
        [Required]
        public int TipPrijevoza { get; set; }
        public List<SelectListItem> nacinPlacanja { get; set; }
        public List<SelectListItem> tipPrijevoza { get; set; }
    }
}
