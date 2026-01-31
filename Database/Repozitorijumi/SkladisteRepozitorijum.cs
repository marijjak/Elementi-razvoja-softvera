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

        public bool NadjiPoId(Guid id, out Skladiste skladiste)
        {
            var pronadjeno = _baza.Tabele.Skladista
                .FirstOrDefault(s => s.Id == id);

            if (pronadjeno == null)
            {
                skladiste = null!;
                return false;
            }

            skladiste = pronadjeno;
            return true;
        }



        public void Sacuvaj()
        {
            _baza.SacuvajPromene();
        }

    }
}
