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
        Ambalaza KreirajAmbalazu(string naziv, string adresaPosiljaoca, Guid skladisteId, IEnumerable<Guid> parfemIds);
        IEnumerable<Ambalaza> SveAmbalaze();
        Ambalaza DodajParfemeUAmbalazu(Guid ambalazaId, IEnumerable<Guid> parfemIds);
    }
}
