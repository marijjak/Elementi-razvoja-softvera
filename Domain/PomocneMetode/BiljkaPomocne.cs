using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.PomocneMetode 
{
    public static class BiljkaPomocne
    {
        public static bool PromeniStanje(Biljka biljka, StanjeBiljke novoStanje)
        {
            if (biljka == null)
            {
                return false;
            }

            biljka.Stanje = novoStanje;
            return true;
        }

        public static bool PromeniJacinuArome(Biljka biljka, double procenat)
        {
            if (biljka == null)
            {
                return false;
            }

            double novaJacina = biljka.JacinaArome * (1 + procenat / 100);

            if (novaJacina < 1.0) novaJacina = 1.0;
            if (novaJacina > 5.0) novaJacina = 5.0;

            biljka.JacinaArome = novaJacina;
            return true;
        }

        public static bool OznaciKaoUbranu(Biljka biljka)
        {
            if (biljka == null || biljka.Stanje != StanjeBiljke.Posadjena)
            {
                return false;
            }

            biljka.Stanje = StanjeBiljke.Ubrana;
            return true;
        }
    }
}

     
    
