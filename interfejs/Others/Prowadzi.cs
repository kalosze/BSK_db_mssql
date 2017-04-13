using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfejs.Others
{
    class Prowadzi
    {
        public Dictionary<string, int> kluczeObce { get; set; }

        public Prowadzi(Dictionary<string, int> klucze)
        {
            kluczeObce = klucze;
        }
    }
}
