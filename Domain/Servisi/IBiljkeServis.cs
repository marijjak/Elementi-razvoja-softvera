using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IBiljkeServis
    {
        Biljka DodajBiljku(Biljka biljka);
        void PromeniStanje(Guid biljkaId);
        IEnumerable<Biljka> SveBiljke();
    }
}
