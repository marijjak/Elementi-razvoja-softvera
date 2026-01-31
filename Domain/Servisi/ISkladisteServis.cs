using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Servisi
{
    public interface ISkladisteServis
    {

        bool PostojiSkladiste(Guid skladisteId);
        bool DodajAmbalazuUSkladiste(Guid skladisteId, int kolicina);

        Task<bool> PosaljiPaketAsync(Guid ambalazaId);
    }
}
