using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Konstante;
using Domain.Modeli;
using Domain.PomocneMetode;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services
{
    public class PreradaServis : IPreradaServis
    {
        private readonly IBiljkeRepozitorijum _biljkeRepo;
        private readonly IPerfumeRepository _parfemRepo;
        private readonly IBiljkeServis _biljkeServis;
        private readonly ILoggerServis _loggerServis; 

        public PreradaServis(IBiljkeServis biljkeServis, IPerfumeRepository parfemRepo, IBiljkeRepozitorijum biljkeRepo, ILoggerServis loggerServis)
        {
            _biljkeServis = biljkeServis;
            _parfemRepo = parfemRepo;
            _biljkeRepo = biljkeRepo;
            _loggerServis = loggerServis;
        }


        public bool NapraviParfem(string nazivParfema, int brojBocica, int zapreminaBociceMl, string tipParfema, out Parfem parfem)
        {
            parfem = null;
            try
            {
                if (zapreminaBociceMl != 150 && zapreminaBociceMl != 250)
                {
                    _loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Neuspešna prerada: Nedozvoljena zapremina bočice ({zapreminaBociceMl}ml).");
                    return false;
                }

                int ukupnoMl = brojBocica * zapreminaBociceMl;
                int potrebneBiljke = (int)Math.Ceiling((double)ukupnoMl / KONSTANTE.MlPoBiljci);

                var ubraneBiljke = _biljkeServis.SveBiljke()
                    .Where(b => b.Stanje == StanjeBiljke.Ubrana)
                    .Take(potrebneBiljke)
                    .ToList();

                if (ubraneBiljke.Count < potrebneBiljke)
                {
                    _loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Nedovoljno ubranih biljaka. Sistem automatski sadi i bere {potrebneBiljke - ubraneBiljke.Count} biljaka.");
                }

                while (ubraneBiljke.Count < potrebneBiljke)
                {
                    double nasumicnaJacina = new Random().NextDouble() * (5.0 - 1.0) + 1.0;

                    if (!_biljkeServis.ZasadiNovuBiljku("Naknadno zasadjena", "L. Nova", "Srbija", nasumicnaJacina))
                    {
                        return false;
                    }
                    var novaBiljka = _biljkeServis.SveBiljke().Last();
                    if (!_biljkeServis.OznaciBiljkuKaoUbranu(novaBiljka.Id))
                    {
                        return false;
                    }

                    ubraneBiljke.Add(novaBiljka);
                }

                foreach (var biljka in ubraneBiljke)
                {
                    if (!BiljkaPomocne.PromeniStanje(biljka, StanjeBiljke.Preradjena))
                    {
                        return false;
                    }
                    if (!_biljkeServis.DodajBiljku(biljka))
                    {
                        return false;
                    }
                }

                double prosecnaJacina = ubraneBiljke.Average(b => b.JacinaArome);

                if (prosecnaJacina > 4.0)
                {
                    _loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Parfem '{nazivParfema}' ima visoku prosečnu jačinu arome: {prosecnaJacina:F2}. Aktivira se balansiranje.");

                    if (!_biljkeServis.ZasadiNovuBiljku("Balansna Biljka", "B. Arome", "Srbija", 4.65))
                    {
                        return false;
                    }

                    var balansnaBiljka = _biljkeServis.SveBiljke().Last();

                    if (!_biljkeServis.PromeniJacinuUljaProcentualno(balansnaBiljka.Id.ToString(), -35))
                    {
                        return false;
                    }
                }

                var noviParfem = new Parfem
                {
                    Naziv = nazivParfema,
                    TipParfema = tipParfema,
                    BrojBocica = brojBocica,
                    ZapreminaBociceMl = zapreminaBociceMl,
                    UkupnaKolicinaMl = ukupnoMl,
                    KolicinaNaStanju = brojBocica,
                    BiljkaIds = ubraneBiljke.Select(b => b.Id).ToList(),
                    RokTrajanja = DateTime.Now.AddYears(2)
                };

                if (!PomocneParfem.GenerisiSerijskiBroj(noviParfem))
                {
                    return false;
                }
                if (_parfemRepo.Dodaj(noviParfem) == null)
                {
                    _loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Greška pri čuvanju parfema '{nazivParfema}' u bazu.");
                    return false;
                }

                parfem = noviParfem;
                _loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Uspešno proizveden parfem: {nazivParfema} ({brojBocica} komada).");
                return true;
            }
            catch (Exception ex)
            {
                _loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Kritična greška u PreradaServis: {ex.Message}");
                return false;
            }
        }

        public bool ImaDovoljnoBiljaka(int ukupnoMl)
        {
            int potrebneBiljke = (int)Math.Ceiling((double)ukupnoMl / KONSTANTE.MlPoBiljci);

            int dostupno = _biljkeRepo.Sve()
                .Count(b => b.Stanje == StanjeBiljke.Ubrana);

            bool imaDovoljno = dostupno >= potrebneBiljke;

            if (!imaDovoljno)
            {
                _loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Provera resursa: Nedostaje biljaka za količinu od {ukupnoMl}ml.");
            }

            return imaDovoljno;
        }
    }
}