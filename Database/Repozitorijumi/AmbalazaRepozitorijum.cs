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

        public Ambalaza Dodaj(Ambalaza ambalaza)
        {
            _baza.Tabele.Ambalaze.Add(ambalaza);
            _baza.SacuvajPromene();
            return ambalaza;
        }

        public Ambalaza? NadjiPoId(Guid id)
        {
            return _baza.Tabele.Ambalaze.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Ambalaza> Sve()
        {
            return _baza.Tabele.Ambalaze;
        }

        public void Azuriraj(Ambalaza ambalaza)
        {
            var postojeca = _baza.Tabele.Ambalaze.FirstOrDefault(a => a.Id == ambalaza.Id);

            if (postojeca != null)
            {
                postojeca.Naziv = ambalaza.Naziv;
                postojeca.AdresaPosiljaoca = ambalaza.AdresaPosiljaoca;
                postojeca.SkladisteId = ambalaza.SkladisteId;
                postojeca.ParfemIds = ambalaza.ParfemIds;
                postojeca.Status = ambalaza.Status;

                _baza.SacuvajPromene();
            }
        }
    }
}