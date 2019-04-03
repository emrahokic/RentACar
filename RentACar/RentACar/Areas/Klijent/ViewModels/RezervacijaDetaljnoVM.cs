using RentACar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Areas.Klijent.ViewModels
{
    public class RezervacijaDetaljnoVM
    {
        public int RezervacijaID { get; set; }
        public DateTime DatumRezervacije { get; set; }
        public Slika SlikaVozila { get; set; }
        public DateTime DatumPreuzimanja { get; set; }
        public DateTime DatumPovrata { get; set; }
        public int Zakljucen { get; set; }
        public double Cijena { get; set; }
        public int BrojDanaIznajmljivanja { get; set; }
        public int NacinPlacanja { get; set; }
        public string Grad { get; set; }
        public string Vozilo { get; set; }
        public string Brend { get; set; }
        public string Poslovnica { get; set; }
        public OcjenaRezervacija ocjenaRezervacija { get; set; }
        public List<Row> dodatneUsluge { get; set; }

        public class Row
        {
            public int RezervisanaUslugaID { get; set; }
            public string Naziv { get; set; }
            public double UkupnaCijenaUsluge { get; set; }
            public int Kolicina { get; set; }
            public string Opis { get; set; }
            public double Cijena { get; set; }

        }
        public double Ukupno { get; set; }

        public double UkupnaCijenaUsluga
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < dodatneUsluge.Count; i++)
                {
                    sum += dodatneUsluge[i].Cijena * dodatneUsluge[i].Kolicina;
                }
                return sum;
            }
        }
    }
}
