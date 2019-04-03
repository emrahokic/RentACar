using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Helper
{
        public enum Gorivo { Dizel=1,Benzin, Struja}
        public enum Transmisija { Manuelni=1,Automatik}
        public enum TipVozila { Normal=1,Hibrid,Elektricno}
        public enum InfoRezervacija { U_Obradi=1,Zavrsena,Odobrena,Odbijena,Vozilo_Predano,Vozilo_Vraceno}
        public enum NacinPlacanja { Karticno=1,Gotovina,PayPal,NijeDefinisano}
        public enum TipPrijevoza { Specijal=1}
        public enum TipPrikolice { Mala=1,Srednja,Velika,Zatvorena_Velika,Zatvorena_Srednja,Zatvorena_Mala}
        public enum GrupniTipVozila {Putnicko=1,Transportno,Old_Timer,Exotic}


   
}
