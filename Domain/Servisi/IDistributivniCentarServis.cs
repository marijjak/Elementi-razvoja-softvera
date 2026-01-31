using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Servisi;


namespace Domain.Servisi
{
    public interface IDistributivniCentarServis : ISkladisniLogistickiServis
    {
        Task<int> PosaljiPaketeAsync(IEnumerable<Guid> ambalazaIds);
    }
}
