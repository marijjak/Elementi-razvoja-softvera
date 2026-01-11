using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class Dogadjaj
    {
        public Guid Id { get; set; }
        public DateTime Vreme { get; set; }
        public string Opis { get; set; } = string.Empty;
        public string Tip { get; set; } = string.Empty;
        public Guid? EntitetId { get; set; } // npr. ID biljke, parfema

        public Dogadjaj()
        {
            // za JSON
        }

        public Dogadjaj(string opis, string tip, Guid? entitetId = null)
        {
            Id = Guid.NewGuid();
            Vreme = DateTime.Now;
            Opis = opis;
            Tip = tip;
            EntitetId = entitetId;
        }
    }
}
