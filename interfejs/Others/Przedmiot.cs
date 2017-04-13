using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfejs
{
    
    class Przedmiot
    {
        public int id { get; }
        public string nazwa { get; set; }

        public Przedmiot(int id, string nazwa)
        {
            this.id = id;
            this.nazwa = nazwa;
        }
    }
}
