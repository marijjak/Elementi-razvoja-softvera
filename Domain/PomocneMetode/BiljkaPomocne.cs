using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.PomocneMetode 
{
    public class BiljkaPomocne : Biljka

    {
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

     
    
