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
        public Guid Id { get;  set; } 
        public string OpstiNaziv { get;  set; } = string.Empty;
        public string LatinskiNaziv { get;  set; } = string.Empty;
        public double JacinaArome { get; set; }   // 1–5 
        public string ZemljaPorekla { get;  set; } = string.Empty;
        public StanjeBiljke Stanje { get;  set; } = StanjeBiljke.Posadjena;
        public Biljka() { }  
        public Biljka(
            string opstiNaziv,
            string latinskiNaziv,
            double jacinaArome,
            string zemljaPorekla)
        {
            Id= Guid.NewGuid();
            OpstiNaziv = opstiNaziv;
            LatinskiNaziv = latinskiNaziv;
            JacinaArome = jacinaArome;
            ZemljaPorekla = zemljaPorekla;
            Stanje = StanjeBiljke.Posadjena;
        }

    }
}
