using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Domain.Servisi;

namespace Services
{
    public class SkladisteProvider : ISkladisteProvider
    {
        // POGLEDAJ OVDE: Dodato 's' u ISkladisniLogistickiServis
        private readonly ISkladisniLogistickiServis _magacin;
        private readonly ISkladisniLogistickiServis _distribucija;

        public SkladisteProvider(
            ISkladisniLogistickiServis magacin,
            ISkladisniLogistickiServis distribucija)
        {
            _magacin = magacin;
            _distribucija = distribucija;
        }

        public ISkladisniLogistickiServis GetServisPoUlozi(string uloga)
        {
            if (uloga == "MenadzerProdaje")
            {
                return _distribucija;
            }

            return _magacin;
        }
    }
}