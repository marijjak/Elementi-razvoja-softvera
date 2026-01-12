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

        public int KolicinaNaStanju { get; set; }

        public DateTime DatumKreiranja { get; set; } = DateTime.Now;
    }
}
