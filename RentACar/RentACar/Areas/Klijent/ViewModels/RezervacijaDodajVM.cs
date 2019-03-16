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
        public int NacinPlacanja { get; set; }
        public Prikolica Prikolica { get; set; }
        public Vozilo Vozilo { get; set; }
        public VozilaVM.Row VoziloVM { get; set; }
        //Za prikaz prikloica
        public List<Prikolica> ListaPrikolica { get; set; }

        public ApplicationUser Klijent { get; set; }

        
        public List<DodatneUslugeVM> ListaDodatnihUsluga { get; set; }
            

        public class DodatneUslugeVM
        {
            public bool Izabrana { get; set; }
            public DodatneUsluge DodatneUsluge { get; set; }
        }
    }
}
