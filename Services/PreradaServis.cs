using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
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
        private const int ML_PO_BILJCI = 50; // Specifikacija: 1 biljka = 50ml

        public PreradaServis(IBiljkeRepozitorijum biljkeRepo, IPerfumeRepository parfemRepo)
        {
            _biljkeRepo = biljkeRepo;
            _parfemRepo = parfemRepo;
        }

        // Naziv i parametri MORAJU biti isti kao u interfejsu
        public Parfem NapraviParfem(string nazivParfema, int brojBocica, int zapreminaBociceMl, string tipParfema)
        {
            // 1. Validacija zapremine prema specifikaciji
            if (zapreminaBociceMl != 150 && zapreminaBociceMl != 250)
                throw new Exception("Zapremina bočice mora biti 150 ili 250 ml.");

            int ukupnoMl = brojBocica * zapreminaBociceMl;
            int potrebneBiljke = (int)Math.Ceiling((double)ukupnoMl / ML_PO_BILJCI);

            // 2. Pronalaženje ubranih biljaka
            var ubraneBiljke = _biljkeRepo.Sve()
                .Where(b => b.Stanje == StanjeBiljke.Ubrana)
                .Take(potrebneBiljke)
                .ToList();

            if (ubraneBiljke.Count < potrebneBiljke)
                throw new Exception("Nema dovoljno ubranih biljaka.");

            // 3. Promena stanja u "Prerađena" i čuvanje u bazu
            foreach (var biljka in ubraneBiljke)
            {
                biljka.PromeniStanje(StanjeBiljke.Preradjena);
                _biljkeRepo.Azuriraj(biljka); // Ovo osigurava da se status promeni u JSON-u
            }

            // 4. Kreiranje novog parfema
            Parfem noviParfem = new Parfem
            {
                Naziv = nazivParfema,
                TipParfema = tipParfema,
                BrojBocica = brojBocica,
                ZapreminaBociceMl = zapreminaBociceMl,
                UkupnaKolicinaMl = ukupnoMl,
                KolicinaNaStanju = brojBocica
            };

            // 5. Generisanje serijskog broja i spasavanje
            noviParfem.GenerisiSerijskiBroj();
            _parfemRepo.Dodaj(noviParfem);

            return noviParfem;
        }
    }
}
