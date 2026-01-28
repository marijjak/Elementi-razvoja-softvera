using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repozitorijumi
{
    public  class SkladisteRepozitorijum:ISkladisteRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public SkladisteRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public Skladiste NadjiPoId(Guid id)
        {
            return _baza.Tabele.Skladista.FirstOrDefault(s => s.Id == id);
        }

        public void Sacuvaj()
        {
            _baza.SacuvajPromene();
        }

    }
}
