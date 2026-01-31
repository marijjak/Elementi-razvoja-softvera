using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PomocneMetode
{
    public static class RacunPomocneMetode
    {
        public static bool DaLiJeMenadzer(Korisnik korisnik)
        {
            return korisnik != null && korisnik.Uloga  == TipKorisnika.MenadzerProdaje;
        }

        public static List<FiskalniRacun> FiltrirajPoDatumu(
            List<FiskalniRacun> racuni,
            DateTime datum)
        {
            return racuni
                .Where(r => r.DatumIzdavanja.Date == datum.Date)
                .ToList();
        }
    }

}
