using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IAmbalazaServis
    {
        bool KreirajAmbalazu(string naziv, string adresaPosiljaoca, Guid skladisteId, IEnumerable<Guid> parfemIds, out Ambalaza ambalaza); IEnumerable<Ambalaza> SveAmbalaze();
        bool DodajParfemeUAmbalazu(Guid ambalazaId, IEnumerable<Guid> parfemIds, out Ambalaza ambalaza);
    }
}
