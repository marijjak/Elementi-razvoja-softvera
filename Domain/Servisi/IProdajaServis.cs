using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IProdajaServis
    {
        bool PokusajDobaviRacuneZaDan(
            Korisnik korisnik,
            DateTime datum,
            out List<FiskalniRacun> racuniZaDan);
        Task<bool> DodajNoviRacunAsync(FiskalniRacun racun);
    }

}
