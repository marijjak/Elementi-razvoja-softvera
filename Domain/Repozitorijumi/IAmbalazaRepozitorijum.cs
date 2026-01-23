using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IAmbalazaRepozitorijum
    {
        Ambalaza Dodaj(Ambalaza ambalaza);
        Ambalaza? NadjiPoId(Guid id);
        IEnumerable<Ambalaza> Sve();

        void Azuriraj(Ambalaza ambalaza);
    }
}
