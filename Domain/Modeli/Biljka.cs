using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Domain.Modeli
{
    public class Biljka
    {
        public Guid Id { get; private set; }
        public string OpstiNaziv { get; private set; } = string.Empty;
        public string LatinskiNaziv { get; private set; } = string.Empty;
        public int JacinaArome { get; private set; }   // 1–5
        public string ZemljaPorekla { get; private set; } = string.Empty;
        public StanjeBiljke Stanje { get; private set; }

        // ZA JSON
        public Biljka() { }

        public Biljka(
            string opstiNaziv,
            string latinskiNaziv,
            int jacinaArome,
            string zemljaPorekla)
        {
            Id = Guid.NewGuid();
            OpstiNaziv = opstiNaziv;
            LatinskiNaziv = latinskiNaziv;
            JacinaArome = jacinaArome;
            ZemljaPorekla = zemljaPorekla;
            Stanje = StanjeBiljke.Posadjena;
        }

        public void PromeniStanje(StanjeBiljke novoStanje)
        {
            Stanje = novoStanje;
        }

        public override string ToString()
        {
            return $"{OpstiNaziv} ({LatinskiNaziv}) | Aroma: {JacinaArome} | {ZemljaPorekla} | {Stanje}";
        }
    }
}
