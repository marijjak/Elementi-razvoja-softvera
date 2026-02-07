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
                return korisnik != null && korisnik.Uloga == TipKorisnika.MenadzerProdaje;
            }

            public static List<FiskalniRacun> FiltrirajPoDatumu(
                List<FiskalniRacun> racuni,
                DateTime datum)
            {
                return racuni
                    .Where(r => r.DatumIzdavanja.Date == datum.Date)
                    .ToList();
            }

            public static List<FiskalniRacun> FiltrirajPoDatumu(
                IEnumerable<FiskalniRacun> racuni,
                DateTime datum)
            {
                if (racuni == null)
                {
                    return new List<FiskalniRacun>();
                }

                return racuni
                    .Where(r => r.DatumIzdavanja.Date == datum.Date)
                    .ToList();
            }

            public static bool PokusajDobaviRacuneZaDan(
                Korisnik korisnik,
                IEnumerable<FiskalniRacun> racuni,
                DateTime datum,
                out List<FiskalniRacun> racuniZaDan)
            {
                racuniZaDan = new List<FiskalniRacun>();

                if (!DaLiJeMenadzer(korisnik))
                {
                    return false;
                }

                racuniZaDan = FiltrirajPoDatumu(racuni, datum);
                return true;
            }

            public static string FormatirajNacinPlacanja(NacinPlacanja nacinPlacanja)
            {
                return nacinPlacanja switch
                {
                    NacinPlacanja.Gotovina => "Gotovina",
                    NacinPlacanja.UplataNaRacun => "Uplata na račun",
                    NacinPlacanja.KarticnoPlacanje => "Kartično plaćanje",
                    _ => nacinPlacanja.ToString()
                };
            }
        }

    }
