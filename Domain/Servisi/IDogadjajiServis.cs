using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;

namespace Domain.Servisi
{
    public interface IDogadjajiServis
    {
        void Zabelezi(string opis, TipEvidencije tip, Guid? entitetId = null);
        IEnumerable<Dogadjaj> SviDogadjaji();
    }
}
