using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfejs
{
    class Uczen
    {
        public int id { get; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public DateTime dataUrodzenia { get; set; }
        public string klasa { get; set; }
        public int nrWdzienniku { get; set; }

        public Uczen(int id, string imie, string nazwisko, DateTime dataUrodzenia, string klasa, int nrWdzienniku)
        {
            this.id = id;
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.dataUrodzenia = dataUrodzenia;
            this.klasa = klasa;
            this.nrWdzienniku = nrWdzienniku;
        }
    }
}
