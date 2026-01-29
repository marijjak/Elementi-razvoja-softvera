using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IMagacinskiCentarServis : ISkladisniLogistickiServis
    {
        Task<bool> PosaljiPaketAsync(Guid ambalazaId);
    }
}
