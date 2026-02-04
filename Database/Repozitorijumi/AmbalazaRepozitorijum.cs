using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class AmbalazaRepozitorijum : IAmbalazaRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public AmbalazaRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public Ambalaza? Dodaj(Ambalaza ambalaza)
        {
            try
            {
                _baza.Tabele.Ambalaze.Add(ambalaza);
                _baza.SacuvajPromene();
                return ambalaza;
            }
            catch
            {
                return null;
            }
        }

        public Ambalaza? NadjiPoId(Guid id)
        {
            try
            {
                return _baza.Tabele.Ambalaze.FirstOrDefault(a => a.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Ambalaza> Sve()
        {
            try
            {
                return _baza.Tabele.Ambalaze;
            }
            catch
            {
                return [];
            }
        }

        public bool Azuriraj(Ambalaza ambalaza)
        {
            try
            {
                var postojeca = _baza.Tabele.Ambalaze.FirstOrDefault(a => a.Id == ambalaza.Id);

                if (postojeca == null)
                {
                    return false;
                }

                postojeca.Naziv = ambalaza.Naziv;
                postojeca.AdresaPosiljaoca = ambalaza.AdresaPosiljaoca;
                postojeca.SkladisteId = ambalaza.SkladisteId;
                postojeca.ParfemIds = ambalaza.ParfemIds;
                postojeca.Status = ambalaza.Status;

                _baza.SacuvajPromene();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}