using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfejs
{
    public class CurrentUser : User
    {
        public CurrentUser(string login, string pass, string name, string surname, string stanowisko, int etykieta) : base(login, pass, name, surname, stanowisko, etykieta)
        {
        }

    }
}
