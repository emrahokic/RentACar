using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Models;
using RentACar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Klijent.ViewModels
{
    public class DodatneUslugeDodajVM
    {
        public int RezervacijaID { get; set; }
        public bool DodajPrikolicu { get; set; }
        public bool Kuka { get; set; }
        public List<UslugeVm> rows { get; set; }
        public class UslugeVm
        {
            public bool Selected { get; set; }
            public int DodatnaUslugaID { get; set; }
            public string Naziv { get; set; }
            public string Opis { get; set; }
            public double Cijena{ get; set; }
            public int Kolicina { get; set; }
        }

    }
}
