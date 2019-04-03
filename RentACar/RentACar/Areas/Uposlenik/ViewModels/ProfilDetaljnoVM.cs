using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Areas.Uposlenik.Controlers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{

    public class ProfilDetaljnoVM
    {
        [Required]
        [RegularExpression(@"[A-Za-z -']+")]
        public string Ime { get; set; }
        public string Slika { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z -']+")]
        public string Prezime { get; set; }
        [Required]
        public DateTime DatumRodjenja { get; set; }
        [RegularExpression(@"[A-Za-z0-9]+(?:\s[A-Za-z0-9'_-]+)+")]
        public string Adresa { get; set; }
        public int GradID { get; set; }
        [Required]
        public string JMBG { get; set; }
        [Required]
        public string Username { get; set; }
        public string Grad { get; set; }
        public string Spol { get; set; }
        [Required]
        [RegularExpression(@"\w+\.?\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}")]
        public string Email { get; set; }
        public string Notification {get; set;}
        [Required]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{3,4}")]
        public string BrTelefona { get; set; }
        public List<SelectListItem> Gradovi { get; set; }

    }
}
