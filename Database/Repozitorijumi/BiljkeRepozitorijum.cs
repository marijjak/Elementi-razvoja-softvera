using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class BiljkeRepozitorijum : IBiljkeRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public BiljkeRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public Biljka Dodaj(Biljka biljka)
        {
            _baza.Tabele.Biljke.Add(biljka);
            _baza.SacuvajPromene();
            return biljka;
        }

        public Biljka NadjiPoId(Guid id)
        {
            return _baza.Tabele.Biljke
                .FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Biljka> Sve()
        {
            return _baza.Tabele.Biljke;
        }
    }
}
