using Domain.Modeli;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 namespace Domain.Repozitorijumi
{
    public interface IBiljkeRepozitorijum
    {
        Biljka Dodaj(Biljka biljka);
        Biljka NadjiPoId(Guid id);
        IEnumerable<Biljka> Sve();
    }
}