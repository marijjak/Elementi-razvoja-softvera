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

        public PreradaServis(IBiljkeServis biljkeServis, IPerfumeRepository parfemRepo, IBiljkeRepozitorijum biljkeRepo)
        {
            _biljkeServis = biljkeServis;
            _parfemRepo = parfemRepo;
            _biljkeRepo = biljkeRepo;
        }


        public bool NapraviParfem(string nazivParfema, int brojBocica, int zapreminaBociceMl, string tipParfema, out Parfem parfem)
        {
            parfem = null;
            try
            {
                if (zapreminaBociceMl != 150 && zapreminaBociceMl != 250)
                {
                    return false;
                }

                int ukupnoMl = brojBocica * zapreminaBociceMl;
                int potrebneBiljke = (int)Math.Ceiling((double)ukupnoMl / KONSTANTE.MlPoBiljci);

                var ubraneBiljke = _biljkeServis.SveBiljke()
                    .Where(b => b.Stanje == StanjeBiljke.Ubrana)
                    .Take(potrebneBiljke)
                    .ToList();

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
                    return false;
                }

                parfem = noviParfem;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ImaDovoljnoBiljaka(int ukupnoMl)
        {
            int potrebneBiljke = (int)Math.Ceiling((double)ukupnoMl / KONSTANTE.MlPoBiljci);

            int dostupno = _biljkeRepo.Sve()
                .Count(b => b.Stanje == StanjeBiljke.Ubrana);

            return dostupno >= potrebneBiljke;
        }


    }
}