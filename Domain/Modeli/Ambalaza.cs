using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class Ambalaza
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Naziv { get; set; } = string.Empty;

        public string AdresaPosiljaoca { get; set; } = string.Empty;

        public Guid SkladisteId { get; set; }

        public List<Guid> ParfemIds { get; set; } = [];

        public StatusAmbalaze Status { get; set; } = StatusAmbalaze.Spakovana;

        public DateTime DatumKreiranja { get; set; } = DateTime.Now;
    }
}
