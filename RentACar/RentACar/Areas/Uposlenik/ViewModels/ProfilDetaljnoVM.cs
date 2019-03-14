using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Uposlenik.ViewModels
{

    public class ProfilDetaljnoVM
    {
        public string Ime { get; set; }
        public string Slika { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Adresa { get; set; }
        public int GradID { get; set; }
        public string JMBG { get; set; }
        public string Username { get; set; }
        public string Grad { get; set; }
        public string Spol { get; set; }
        public string Email { get; set; }
        public string Notification {get; set;}
        public string BrTelefona { get; set; }
        public List<SelectListItem> Gradovi { get; set; }

    }
}
