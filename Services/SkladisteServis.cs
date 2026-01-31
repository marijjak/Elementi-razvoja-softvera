using Domain.Modeli;
using Domain.PomocneMetode;
using Domain.Repozitorijumi;
using Domain.Servisi;
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
            return _repo.NadjiPoId(skladisteId, out Skladiste _);
        }
        public bool DodajAmbalazuUSkladiste(Guid skladisteId, int kolicina)
        {
            // ISPRAVLJENO: Koristimo 'out' da dobijemo stvarni objekat skladišta
            Skladiste skladiste;
            bool postoji = _repo.NadjiPoId(skladisteId, out skladiste);

            if (!postoji || skladiste == null) return false;

            // ISPRAVLJENO: Provera kapaciteta direktno na objektu
            if (skladiste.TrenutniBrojAmbalaza + kolicina > skladiste.MaxBrojAmbalaza)
                return false;

            skladiste.TrenutniBrojAmbalaza += kolicina;
            _repo.Sacuvaj();

            return true;
        }
    }
}
