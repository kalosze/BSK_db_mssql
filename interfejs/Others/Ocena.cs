using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfejs.Others
{
    class Ocena
    {
        public int id { get; }
        public float ocena { get; set; }
        public DateTime data { get; set; }
        public Dictionary<string, int> kluczeObce { get; set; }

        public Ocena(int id, float ocena, DateTime data, Dictionary<string, int> klucze)
        {
            this.id = id;
            this.ocena = ocena;
            this.data = data;
            this.kluczeObce = klucze;
        }

    }
}
