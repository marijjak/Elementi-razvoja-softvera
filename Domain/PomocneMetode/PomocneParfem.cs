using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;

namespace Domain.PomocneMetode
{
    
        public static class PomocneParfem
        {
            public static bool GenerisiSerijskiBroj(Parfem parfem)
            {
                if (parfem == null)
                {
                    return false;
                }

                parfem.SerijskiBroj = $"PP-2025-{parfem.Id.ToString().Substring(0, 8).ToUpper()}";
                return true;
            }


        }
    
}