using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IDistributivniCentarServis
    {
        Task<int> PosaljiPaketeAsync(IEnumerable<Guid> ambalazaIds);
    }
}
