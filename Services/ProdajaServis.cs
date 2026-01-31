using Domain.Enumeracije;
using Domain.Modeli;
using Domain.PomocneMetode;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProdajaServis : IProdajaServis
    {
        private readonly IFiskalniRacunRepozitorijum _racunRepo;
        private readonly ILoggerServis _logger;
        private readonly IDogadjajiServis _dogadjaji;

        public ProdajaServis(
            IFiskalniRacunRepozitorijum racunRepo,
            ILoggerServis logger, IDogadjajiServis dogadjaji)
        {
            _racunRepo = racunRepo;
            _logger = logger;
            _dogadjaji = dogadjaji;
        }

        public IEnumerable<FiskalniRacun> VidiSveRacune(Korisnik korisnik)
        {
            if (korisnik.Uloga != TipKorisnika.MenadzerProdaje)
            {
                _dogadjaji.Zabelezi($"Neovlašćen pokušaj pristupa računima: {korisnik.KorisnickoIme}", TipEvidencije.WARNING);
                throw new UnauthorizedAccessException("Samo menadžer može videti račune.");
            }

            _dogadjaji.Zabelezi($"Menadžer {korisnik.ImePrezime} je pregledao dnevni promet.", TipEvidencije.INFO);
            return _racunRepo.GetSviRacuni();
        }

        public bool PokusajDobaviRacuneZaDan(
            Korisnik korisnik,
            DateTime datum,
            out List<FiskalniRacun> racuniZaDan)
        {
            racuniZaDan = new List<FiskalniRacun>();

            // PROVERA POZIVA (kako si tražila)
            if (!RacunPomocneMetode.DaLiJeMenadzer(korisnik))
            {
                _logger.EvidentirajDogadjaj(
                    TipEvidencije.WARNING,
                    "Neovlascen pokusaj pristupa fiskalnim racunima.");

                return false;
            }

            var sviRacuni = _racunRepo.GetSviRacuni().ToList();


            racuniZaDan = RacunPomocneMetode
                .FiltrirajPoDatumu(sviRacuni, datum);

            _logger.EvidentirajDogadjaj(
                TipEvidencije.INFO,
                $"Menadzer je dobavio racune za datum {datum.ToShortDateString()}.");

            return true;
        }

        public bool DodajNoviRacun(FiskalniRacun racun)
        {
            // Pozivamo 'Dodaj' jer se tako zove u tvom repozitorijumu
            bool uspeh = _racunRepo.Dodaj(racun);

            if (uspeh)
            {
                _dogadjaji.Zabelezi($"Prodat račun: {racun.UkupanIznos} RSD", TipEvidencije.INFO);
            }
            return uspeh;
        }
    }

}
