using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repozitorijumi
{
    public interface IFiskalniRacunRepozitorijum
    {
        bool Dodaj(FiskalniRacun racun);

        IEnumerable<FiskalniRacun> GetSviRacuni();
    }


}
