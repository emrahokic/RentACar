using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Models;
using RentACar.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Klijent.ViewModels
{
    public class DodajPlacanjeSpremiPV
    {
        
        [Required(ErrorMessage ="Odaberite nacin placanja")]
        public string NacinPlacanja { get; set; }
    }
}
