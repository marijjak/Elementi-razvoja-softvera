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
        public Biljka() { }  //za json
        public Biljka(
            string opstiNaziv,
            string latinskiNaziv,
            int jacinaArome,
            string zemljaPorekla)
        {
            Id= Guid.NewGuid();
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
        public void PromeniJacinuArome(double procenat)
        {
            // Izračunavanje nove jačine
            double novaJacina = JacinaArome * (1 + procenat / 100);

            // Ograničavanje na opseg 1.0 - 5.0
            if (novaJacina < 1.0) novaJacina = 1.0;
            if (novaJacina > 5.0) novaJacina = 5.0;

            JacinaArome = novaJacina;
        }

        public void OznaciKaoUbranu()
        {
            if (Stanje != StanjeBiljke.Posadjena)
                throw new InvalidOperationException("Biljka mora biti posađena da bi bila ubrana.");

            Stanje = StanjeBiljke.Ubrana;
        }
    }
}
