using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class Skladiste
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Naziv { get; set; } = string.Empty;

        public int MaxBrojAmbalaza { get; set; }
        public int TrenutniBrojAmbalaza { get; set; }
        public bool ImaMesta(int kolicina)
        {
            return TrenutniBrojAmbalaza + kolicina <= MaxBrojAmbalaza;
        }

        public void DodajAmbalazu(int kolicina)
        {
            TrenutniBrojAmbalaza += kolicina;
        }
    }
}
