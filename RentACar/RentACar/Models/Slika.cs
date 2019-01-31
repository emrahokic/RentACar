using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class Slika
    {

        public int SlikaID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public int Pozicija { get; set; }

        public int VoziloID { get; set; }
        public Vozilo Vozilo { get; set; }

    }
}
