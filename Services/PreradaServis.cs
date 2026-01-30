using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Domain.PomocneMetode;
using Domain.Konstante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PreradaServis : IPreradaServis
    {
        private readonly IBiljkeRepozitorijum _biljkeRepo;
        private readonly IPerfumeRepository _parfemRepo;
        private readonly IBiljkeServis _biljkeServis;
        private const int ML_PO_BILJCI = 50; // Specifikacija: 1 biljka = 50ml

        public PreradaServis(IBiljkeServis biljkeServis, IPerfumeRepository parfemRepo)
        {
            _biljkeServis = biljkeServis;
            _parfemRepo = parfemRepo;
        }

        
        public Parfem NapraviParfem(string nazivParfema, int brojBocica, int zapreminaBociceMl, string tipParfema)
        {
            if (zapreminaBociceMl != 150 && zapreminaBociceMl != 250)
                throw new Exception("Zapremina bočice mora biti 150 ili 250 ml.");

            int ukupnoMl = brojBocica * zapreminaBociceMl;
            int potrebneBiljke = (int)Math.Ceiling((double)ukupnoMl / KONSTANTE.MlPoBiljci);

            // 1. Dobavljanje trenutno ubranih biljaka
            var ubraneBiljke = _biljkeServis.SveBiljke()
                .Where(b => b.Stanje == StanjeBiljke.Ubrana)
                .Take(potrebneBiljke)
                .ToList();

            // 2. DOPUNA: Ako nedostaje biljaka, šalje se zahtev za sađenje (logika iz zahteva)
            while (ubraneBiljke.Count < potrebneBiljke)
            {
                // Jačina se nasumično generiše pri sađenju (npr. između 1.0 i 5.0)
                double nasumicnaJacina = new Random().NextDouble() * (5.0 - 1.0) + 1.0;

                if (!_biljkeServis.ZasadiNovuBiljku("Naknadno zasadjena", "L. Nova", "Srbija", nasumicnaJacina))
                {
                    throw new InvalidOperationException("Neuspešno sađenje biljke.");
                }
                var novaBiljka = _biljkeServis.SveBiljke().Last();
                if (!_biljkeServis.OznaciBiljkuKaoUbranu(novaBiljka.Id))
                {
                    throw new InvalidOperationException("Neuspešno označavanje biljke kao ubrane.");
                }

                ubraneBiljke.Add(novaBiljka);
            }

            // 3. Prerada i promena stanja u 'Preradjena'
            foreach (var biljka in ubraneBiljke)
            {
                if (!BiljkaPomocne.PromeniStanje(biljka, StanjeBiljke.Preradjena))
                {
                    throw new InvalidOperationException("Neuspešna promena stanja biljke.");
                }
                _biljkeServis.DodajBiljku(biljka);
            }

            double prosecnaJacina = ubraneBiljke.Average(b => b.JacinaArome);

            // 4. RAVNOTEŽA AROMA: Ako prosečna jačina pređe 4.0
            if (prosecnaJacina > 4.0)
            {
                // Odstupanje (npr. 4.65 - 4.0 = 0.65)
                double procenatOdstupanja = prosecnaJacina - 4.0;

                // Zahtev za novu biljku radi balansa
                if (!_biljkeServis.ZasadiNovuBiljku("Balansna Biljka", "B. Arome", "Srbija", 3.0))
                {
                    throw new InvalidOperationException("Neuspešno sađenje balansne biljke.");
                }

                // Pronalaženje te nove biljke
                var balansnaBiljka = _biljkeServis.SveBiljke().Last();

                //smanjiti jačinu... na 65% (procenat odstupanja) od vrednosti koju ima
                if (!_biljkeServis.PromeniJacinuUljaProcentualno(balansnaBiljka.Id.ToString(), procenatOdstupanja))
                {
                    throw new InvalidOperationException("Neuspešna promena jačine ulja.");
                }
            }

                // 5. Kreiranje parfema
                Parfem noviParfem = new Parfem
            {
                Naziv = nazivParfema,
                TipParfema = tipParfema,
                BrojBocica = brojBocica,
                ZapreminaBociceMl = zapreminaBociceMl,
                UkupnaKolicinaMl = ukupnoMl,
                KolicinaNaStanju = brojBocica
            };

            if (!PomocneParfem.GenerisiSerijskiBroj(noviParfem))
            {
                throw new InvalidOperationException("Neuspešno generisanje serijskog broja.");
            }
            _parfemRepo.Dodaj(noviParfem);

            return noviParfem;
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
