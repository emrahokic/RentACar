using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required(ErrorMessage = "Ime je obavezno polje")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Ža-ž\s]+$")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno polje")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Ža-ž\s]+$")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Datum rođenja je obavezno polje")]
        [DataType(DataType.Date)]
        public DateTime DatumRodjenja { get; set; }

        public int? GradID { get; set; }
        public Grad Grad { get; set; }
        public string Adresa { get; set; }
        public DateTime DatumRegistracije { get; set; }

        public string JMBG { get; set; }
        public string Spol { get; set; }

        public int ApplicationUserRoleID { get; set; }
        public ApplicationUserRole ApplicationUserRole { get; set; }



    }
}
