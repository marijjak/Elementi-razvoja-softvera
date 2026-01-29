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
        private readonly IMagacinskiCentarServis _magacinServis;
        private readonly IDistributivniCentarServis _distributivniCentarServis;

        public SkladisteProvider(IMagacinskiCentarServis magacin, IDistributivniCentarServis distributivni)
        {
            _magacinServis = magacin;
            _distributivniCentarServis = distributivni;
        }

        public ISkladisniLogistickiServis GetServisPoUlozi(string uloga)
        {
            if (uloga == "Prodavac") return _magacinServis;
            if (uloga == "MenadzerProdaje") return _distributivniCentarServis;

            throw new Exception("Nemate ovlascenja za ovu akciju.");
        }
    }
}