using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Models;
using RentACar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Klijent.ViewModels
{
    public class RezervacijaDodajVM
    {
        public DateTime DatumRezervacije { get; set; }
        public DateTime DatumPreuzimanja { get; set; }
        public DateTime DatumPovrata { get; set; }
        public Vozilo Vozilo { get; set; }
        public VozilaVM.Row VoziloVM { get; set; }

    }
}
