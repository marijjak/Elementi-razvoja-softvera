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
        bool DodajBiljku(Biljka biljka);
        bool OznaciBiljkuKaoUbranu(Guid id);
        bool PromeniStanje(Guid biljkaId);
        IEnumerable<Biljka> SveBiljke();
        bool ZasadiNovuBiljku(string naziv, string latinski, string zemlja, double jacina);
        bool PromeniJacinuUljaProcentualno(string unos, double procenat);
    }
}
