using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Klijent.ViewModels
{
    public class RezervacijaUrediVM
    {
        //Rezervacija info
        public int RezervacijaID { get; set; }
        public DateTime DatumPreuzimanja { get; set; }
        public DateTime DatumRezervacije { get; set; }
        public DateTime DatumPovrata { get; set; }
        public int Zakljucen { get; set; }
        public List<SelectListItem> zakljuceni { get; set; }
        public double Cijena { get; set; }
        public int BrojDanaIznajmljivanja { get; set; }
        public int NacinPlacanja { get; set; }
        public List<SelectListItem> naciniPlacanja { get; set; }

        //vozilo
        public string SlikaVozila { get; set; }
        public string Vozilo { get; set; }

        //klijent info
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Adresa { get; set; }
        public string Spol { get; set; }
        public string jmbg { get; set; }

        //prikolica
        public bool imaPrikolicu { get; set; }
        public int? PrikolicaID { get; set; }
        public List<SelectListItem> prikolice { get; set; }

    }
}
