using Domain.Modeli;
using Domain.Repozitorijumi;
using System.Collections.Generic;
using Domain.BazaPodataka;

namespace Database.Repozitorijumi
{
    public class FiskalniRacunRepozitorijum : IFiskalniRacunRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public FiskalniRacunRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public bool Dodaj(FiskalniRacun racun)
        {
            try
            {
                _baza.Tabele.FiskalniRacuni.Add(racun);
                _baza.SacuvajPromene();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<FiskalniRacun> GetSviRacuni()
        {
            try
            {
                return _baza.Tabele.FiskalniRacuni;
            }
            catch
            {
                return new List<FiskalniRacun>();
            }
        }
    }
}