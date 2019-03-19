using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Models;
using RentACar.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static RentACar.Areas.Klijent.CustomValidacija.CustomValidation;

namespace RentACar.Areas.Klijent.ViewModels
{
    public class RezervacijaDodajVM
    {
       
        public DateTime DatumRezervacije { get; set; }

        [Required(ErrorMessage = "Datum preuzimanaj je obavezan")]
        [DataType(DataType.DateTime)]
        [CheckDatumPreuzimanja(VrijemeOtvaranja = 8,VrijemeZatvaranja = 18)]
        public DateTime DatumPreuzimanja { get; set; }

        [CheckDatumPovrata(VrijemeOtvaranja = 8, VrijemeZatvaranja = 18)]
        [Required(ErrorMessage = "Datum povrata je obavezan")]
        public DateTime DatumPovrata { get; set; }

        public string Vozilo { get; set; }


        public VozilaVM.Row VoziloVM { get; set; }

     

    }
}
