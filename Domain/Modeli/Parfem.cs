using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class Parfem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string SerijskiBroj { get; set; } = string.Empty;

        public string Naziv { get; set; } = string.Empty;

        public string TipParfema { get; set; } = string.Empty;

        public int BrojBocica { get; set; }

        public int ZapreminaBociceMl { get; set; }   // 150 ili 250

        public int UkupnaKolicinaMl { get; set; }

        public int KolicinaNaStanju { get; set; }

        public DateTime DatumKreiranja { get; set; } = DateTime.Now;

        public void GenerisiSerijskiBroj()
        {
            SerijskiBroj = $"PP-2025-{Id.ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
