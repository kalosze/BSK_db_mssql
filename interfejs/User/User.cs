using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfejs
{
    public class User
    {
        string login { get; set; }
        string pass { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        int accesLvl { get; set;}
        //moze zadziala statyczna tablica uzytkownikow
        public static List<User> Users;

        //Konstruktor uzytkownika
        public User(string login, string pass, string name, string surname, int accesLvl)
        {
            this.login = login;
            this.pass = pass;
            this.name = name;
            this.surname = surname;
            this.accesLvl = accesLvl;
        }

    }
}
