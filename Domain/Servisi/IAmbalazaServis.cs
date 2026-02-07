using Domain.Modeli;
using System;
using System.Collections.Generic;

namespace Domain.Servisi
{
    public interface IAmbalazaServis
    {
        bool KreirajAmbalazu(string naziv, string adresaPosiljaoca, Guid skladisteId, IEnumerable<Guid> parfemIds, out Ambalaza ambalaza);
        bool DodajParfemeUAmbalazu(Guid ambalazaId, IEnumerable<Guid> parfemIds, out Ambalaza ambalaza);
        bool PosaljiAmbalazu(Guid ambalazaId);
        IEnumerable<Ambalaza> Sve();
        bool ObrisiAmbalazu(Guid id);
    }
}