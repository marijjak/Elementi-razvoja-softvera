using Domain.Repozitorijumi;
using Domain.Modeli;
using Domain.BazaPodataka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repozitorijumi
{
    public class DogadjajiRepozitorijum : IDogadjajiRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public DogadjajiRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public void Dodaj(Dogadjaj dogadjaj)
        {
            _baza.Tabele.Dogadjaji.Add(dogadjaj);
            _baza.SacuvajPromene();
        }

        public IEnumerable<Dogadjaj> Sve()
        {
            return _baza.Tabele.Dogadjaji;
        }
    }
}
