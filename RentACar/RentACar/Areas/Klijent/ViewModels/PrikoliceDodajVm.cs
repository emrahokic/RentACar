using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Models;
using RentACar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Klijent.ViewModels
{
    public class PrikoliceDodajVM
    {
        public int RezervacijaID { get; set; }
        public List<PrikoliceVm> rows { get; set; }
        public string Selected { get; set; } 
        public class PrikoliceVm
        {

            public int PrikolicaID { get; set; }
            public double Sirina { get; set; }
            public double Zapremina { get; set; }
            public double Duzina { get; set; }
            public int TipPrikolice { get; set; }
            public double Cijena{ get; set; }
        }

    }
}
