using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;

namespace Domain.PomocneMetode 
{
    public class PomocneParfem : Parfem
    {
        public void GenerisiSerijskiBroj()
        {
            SerijskiBroj = $"PP-2025-{Id.ToString().Substring(0, 8).ToUpper()}";
           // return true;
        }


    }
}
