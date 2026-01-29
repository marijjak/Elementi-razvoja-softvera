using Domain.Repozitorijumi;
using Domain.Servisi;
using Domain.PomocneMetode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SkladisteServis : ISkladisteServis
    {
        private readonly ISkladisteRepozitorijum _repo;

        public SkladisteServis(ISkladisteRepozitorijum repo)
        {
            _repo = repo;
        }
        public bool PostojiSkladiste(Guid skladisteId)
        {
            return _repo.NadjiPoId(skladisteId) != null;
        }
        public bool DodajAmbalazuUSkladiste(Guid skladisteId, int kolicina)
        {
            var skladiste = _repo.NadjiPoId(skladisteId);
            if (skladiste == null) return false;

            if (!skladiste.ImaMesta(kolicina))
                return false; 

            skladiste.DodajAmbalazu(kolicina);
            _repo.Sacuvaj();

            return true;
        }
    }
}
