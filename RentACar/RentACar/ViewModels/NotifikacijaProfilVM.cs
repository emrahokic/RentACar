using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.ViewModels
{
    public class NotifikacijaProfilVM
    {

        public List<Row> notifikacije { get; set; }
        public int BrojNeprocitanih { get; set; }
        public class Row
        {
            public string Poruka { get; set; }
            public string Vrijeme { get; set; }
            public bool Otvorena { get; set; }
            public int RezervacijaID { get; set; }

        }
    }
}
