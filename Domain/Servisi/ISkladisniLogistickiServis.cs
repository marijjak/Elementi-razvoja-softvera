using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface ISkladisniLogistickiServis
    {
        Task<bool> ProcesuirajIsporukuAsync(Guid ambalazaId);
    }
    public interface ISkladisteProvider
    {
        ISkladisniLogistickiServis GetServisPoUlozi(string uloga);
    }
}
