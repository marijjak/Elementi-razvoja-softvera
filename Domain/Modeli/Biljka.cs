using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public  class Biljka
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string OpstiNaziv { get; set; } = string.Empty;
        public string LatinskiNaziv { get; set; } = string.Empty;
        public int JacinaArome { get; set; }   // 1–5 
        public string ZemljaPorekla { get; set; } = string.Empty;
        public StanjeBiljke Stanje { get; set; } = StanjeBiljke.Posadjena;
        public Biljka() { }
        public Biljka(
        string opstiNaziv,
        string latinskiNaziv,
        int jacinaArome,
        string zemljaPorekla)
        {
            OpstiNaziv = opstiNaziv;
            LatinskiNaziv = latinskiNaziv;
            JacinaArome = jacinaArome;
            ZemljaPorekla = zemljaPorekla;
            Stanje = StanjeBiljke.Posadjena;
        }
        public override string ToString()
        {
            return $"{OpstiNaziv} ({LatinskiNaziv}) | Aroma: {JacinaArome} | {ZemljaPorekla} | {Stanje}";
        }
    }
}
