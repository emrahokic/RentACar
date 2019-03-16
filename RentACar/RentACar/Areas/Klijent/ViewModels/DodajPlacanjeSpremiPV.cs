using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Models;
using RentACar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Klijent.ViewModels
{
    public class DodajPlacanjeSpremiPV
    {
        public int RezervacijaID { get; set; }
        public string NacinPlacanja { get; set; }

    }
}
