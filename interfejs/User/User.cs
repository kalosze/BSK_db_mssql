using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfejs
{
    public class User
    {
        private string login1;
        private string pass1;

        public int id { get; }
        public string login { get; }
        public string pass { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public int etykieta { get; set;}
        public string stanowisko { get; }        //moze zadziala statyczna tablica uzytkownikow
        //public static List<User> Users;

        //Konstruktor uzytkownika
        public User(int id,string login, string pass, string name, string surname,string stanowisko, int accesLvl)
        {
            this.id= id;
            this.login = login;
            this.pass = pass;
            this.name = name;
            this.surname = surname;
            this.stanowisko = stanowisko;
            this.etykieta = accesLvl;
        }

    }
}
