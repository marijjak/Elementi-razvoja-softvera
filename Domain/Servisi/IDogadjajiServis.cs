using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IDogadjajiServis
    {
        void Zabelezi(string opis, string tip, Guid? entitetId = null);
        IEnumerable<Dogadjaj> SviDogadjaji();
    }
}
