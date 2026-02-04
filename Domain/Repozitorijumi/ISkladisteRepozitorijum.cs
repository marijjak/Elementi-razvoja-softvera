using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repozitorijumi
{
    public interface ISkladisteRepozitorijum
    {
        bool NadjiPoId(Guid id, out Skladiste skladiste);
        bool Sacuvaj();
    }

}