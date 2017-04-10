using System;
using System.Collections.Generic;

namespace interfejs
{
    class Database
    {
        public enum TabeleEnum
        {
            Przedmiot,
            Uczen,
            Ocena,
            Prowadzi
        }
        public class Przedmiot
        {
            public string ID { get; set; }
            public string Nazwa { get; set; }
        }

        public class Uczen
        {
            public string ID { get; set; }
            public string Imie { get; set; }
            public string Nazwisko { get; set; }
            public string DataUr { get; set; }
            public string TelOp { get; set; }
            public string Klasa { get; set; }
            public string NrWDz { get; set; }
        }

        public class Ocena
        {
            public string ID { get; set; }
            public string Ocen { get; set; }
            public string Data { get; set; }
            public string ID_Ucznia { get; set; }
            public string ID_Nauczyciela { get; set; }
            public string ID_Przedmiotu { get; set; }
        }

        public class Prowadzi
        {
            public string ID_Nauczyciela { get; set; }
            public string ID_Przedmiotu { get; set; }
        }

        public class Etykiety
        {
            public string ID { get; set; }
            public string NazwaTabeli { get; set; }
            public string Etykieta { get; set; }
        }

        public static List<Przedmiot> przedmioty = new List<Przedmiot>
        {
            new Przedmiot() { ID = "1", Nazwa = "Matematyka" },
            new Przedmiot() { ID = "2", Nazwa = "Język Polski" },
            new Przedmiot() { ID = "3", Nazwa = "Historia" },
            new Przedmiot() { ID = "4", Nazwa = "WF" },
            new Przedmiot() { ID = "5", Nazwa = "Informatyka" },
        };

        public static List<Uczen> uczniowie = new List<Uczen>
        {
            new Uczen() { ID = "1", Imie ="Mariusz", Nazwisko="Wąski", DataUr="1993-04-20", Klasa="1A", NrWDz="1", TelOp="123123123" },
            new Uczen() { ID = "2", Imie ="Władysław", Nazwisko="Wąski", DataUr="1993-02-10", Klasa="1A", NrWDz="2", TelOp="123123123" }
        };

        public static List<Ocena> oceny = new List<Ocena>
        {
            new Ocena() { ID ="1", Data ="2017-04-10", Ocen="4", ID_Przedmiotu="1", ID_Nauczyciela="3", ID_Ucznia="1" },
            new Ocena() { ID ="2", Data ="2017-04-10", Ocen="2", ID_Przedmiotu="1", ID_Nauczyciela="3", ID_Ucznia="2" }
        };

        public static List<Prowadzi> prowadza = new List<Prowadzi>
        {
            new Prowadzi() { ID_Nauczyciela="3", ID_Przedmiotu="1" },
            new Prowadzi() { ID_Nauczyciela="4", ID_Przedmiotu="5" }
        };

        public static List<Tuple<int, List<object>, Type>> tabele = new List<Tuple<int, List<object>, Type>>
        {
            new Tuple<int, List<object>, Type>(0, new List<object>(przedmioty), typeof(Przedmiot)),
            new Tuple<int, List<object>, Type>(0, new List<object>(uczniowie), typeof(Uczen)),
            new Tuple<int, List<object>, Type>(0, new List<object>(oceny), typeof(Ocena)),
            new Tuple<int, List<object>, Type>(0, new List<object>(prowadza), typeof(Prowadzi))
        };
    }
}
