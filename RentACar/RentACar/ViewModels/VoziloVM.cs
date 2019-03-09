﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.ViewModels
{
    public class VoziloVM
    {
        public List<Row> rows { get; set; }
        public class Row
        {
            public int VoziloID { get; set; }
            public string Brend { get; set; }
            public string Naziv { get; set; }
            public string Model { get; set; }
            public int BrojVrata { get; set; }
            public string TipVozila { get; set; }
            public string Transmisija { get; set; }
        }
    }
}