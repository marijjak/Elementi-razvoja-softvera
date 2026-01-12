using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IPreradaServis
    {
        Parfem NapraviParfem(string nazivParfema, int brojBocica, int zapreminaBociceMl, string tipParfema);
    }
}
