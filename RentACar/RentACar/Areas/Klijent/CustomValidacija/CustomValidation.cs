using RentACar.Areas.Klijent.ViewModels;
using RentACar.Data;
using RentACar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Klijent.CustomValidacija
{
    public class CustomValidation
    {


        public sealed class CheckDatumPreuzimanja : ValidationAttribute
        {
            public int VrijemeOtvaranja { get; set; }
            public int VrijemeZatvaranja { get; set; }
            protected override ValidationResult IsValid(object datumPreuzimanja, ValidationContext validationContext)
            {
                
                DateTime DatumPreuzimanja =(DateTime) datumPreuzimanja;
                if (DatumPreuzimanja >= DateTime.Now)
                {

                    if (DatumPreuzimanja < DateTime.Now.AddDays(5))
                    {
                        if (DatumPreuzimanja.Hour > VrijemeOtvaranja && DatumPreuzimanja.Hour < VrijemeZatvaranja)
                        {

                            return ValidationResult.Success;
                        }
                        else
                        {
                            return new ValidationResult("Vrijeme preuzimanja mora biti u radnom vremenu poslovnice od "+VrijemeOtvaranja+"h do"+VrijemeZatvaranja+"h");

                        }
                    }
                    else
                    {

                    return new ValidationResult("Datum preuzimanja " + DatumPreuzimanja +" nije validan, mozete izvrsiti rezervaciju samo za 5 dana unaprijed!");
                    }


                }
                else
                {
                    return new ValidationResult("Datum i vrijeme preuzimanja ne moze biti stariji od trenutnog datuma i vremena!");
                }
            }
        }

        public sealed class CheckVoziloID : ValidationAttribute
        {
            protected override ValidationResult IsValid(object _id, ValidationContext validationContext)
            {

                Vozilo v =(Vozilo) _id;

                if (v.VoziloID != null)
                {
                    var _context = (ApplicationDbContext)validationContext
                         .GetService(typeof(ApplicationDbContext));
                    object id = _context.Vozilo.Where(x=> x.VoziloID == v.VoziloID).FirstOrDefault().VoziloID;
                    if (id == null || (int) id == 0)
                    {
                        return new ValidationResult("Something went wrong please go back!");

                    }
                    else
                    {
                        bool rezervisanoVec = _context.TrenutnaPoslovnica.Where(y => y.VoziloID == (int)id).FirstOrDefault().VoziloRezervisano;
                        if (rezervisanoVec)
                        {
                            return new ValidationResult("Something went wrong please go back!");

                        }
                        else
                        {
                            return ValidationResult.Success;



                        }

                    }

                }
                else
                {
                    return new ValidationResult("Something went wrong please go back!!");
                }
            }
        }

        public sealed class CheckDatumPovrata : ValidationAttribute
        {
            public object datumPovrata { get; set; }
            public int VrijemeOtvaranja { get; set; }
            public int VrijemeZatvaranja { get; set; }
            protected override ValidationResult IsValid(object datumPovrata, ValidationContext validationContext)
            {
                 DateTime DatumPovrata =(DateTime) datumPovrata;
                RezervacijaDodajVM vm = (RezervacijaDodajVM) validationContext.ObjectInstance;
                if (DatumPovrata > vm.DatumPreuzimanja)
                {
                    if (DatumPovrata.Hour > VrijemeOtvaranja && DatumPovrata.Hour < VrijemeZatvaranja)
                    {

                      
                            return ValidationResult.Success;
                       
                    }
                    else
                    {
                        return new ValidationResult("Vrijeme povrata mora biti u radnom vremenu poslovnice od " + VrijemeOtvaranja + "h do" + VrijemeZatvaranja + "h");

                    }
                }
                else
                {
                    return new ValidationResult("Datum i vrijeme povrata ne moze biti stariji od datuma i vremena preuzimanja!");
                }
            }
        }


    }
}
