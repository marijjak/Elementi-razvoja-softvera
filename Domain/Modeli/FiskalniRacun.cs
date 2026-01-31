using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class FiskalniRacun
    {
        public Guid Id { get; set; }
        public DateTime DatumIzdavanja { get; set; }

        public string ImeProdavca { get; set; } = string.Empty;
        public TipProdaje TipProdaje { get; set; }
        public NacinPlacanja NacinPlacanja { get; set; }

        public Dictionary<Guid, int> Stavke { get; set; } = new();

        public decimal UkupanIznos { get; set; }
    }



}
