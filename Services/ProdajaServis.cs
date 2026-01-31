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
        private readonly ISkladisteServis _distributivniCentar;
        private readonly ISkladisteServis _magacinskiCentar;
        private readonly IAmbalazaRepozitorijum _ambalazaRepo;


        public ProdajaServis(
            IFiskalniRacunRepozitorijum racunRepo,
            ILoggerServis logger,
            IDogadjajiServis dogadjaji,
            ISkladisteServis distributivniCentar,
            ISkladisteServis magacinskiCentar,
            IAmbalazaRepozitorijum ambalazaRepo)
        {
            _racunRepo = racunRepo;
            _logger = logger;
            _dogadjaji = dogadjaji;
            _distributivniCentar = distributivniCentar;
            _magacinskiCentar = magacinskiCentar;
            _ambalazaRepo = ambalazaRepo;
        }

        public IEnumerable<FiskalniRacun> VidiSveRacune(Korisnik korisnik)
        {
            try
            {
                if (korisnik.Uloga != TipKorisnika.MenadzerProdaje)
                {
                    _dogadjaji.Zabelezi($"Neovlašćen pokušaj pristupa računima: {korisnik.KorisnickoIme}", TipEvidencije.WARNING);
                    return Enumerable.Empty<FiskalniRacun>();
                }

                _dogadjaji.Zabelezi($"Menadžer {korisnik.ImePrezime} je pregledao dnevni promet.", TipEvidencije.INFO);
                return _racunRepo.GetSviRacuni();
            }
            catch (Exception ex)
            {
                _dogadjaji.Zabelezi($"Greška pri pregledu računa: {ex.Message}", TipEvidencije.ERROR);
                return Enumerable.Empty<FiskalniRacun>();
            }
        }

        public bool PokusajDobaviRacuneZaDan(Korisnik korisnik, DateTime datum, out List<FiskalniRacun> racuniZaDan)
        {
            racuniZaDan = new List<FiskalniRacun>();

            if (!RacunPomocneMetode.DaLiJeMenadzer(korisnik))
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, "Neovlascen pokusaj pristupa fiskalnim racunima.");
                return false;
            }

            var sviRacuni = _racunRepo.GetSviRacuni().ToList();
            racuniZaDan = RacunPomocneMetode.FiltrirajPoDatumu(sviRacuni, datum);

            _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Menadzer je dobavio racune za datum {datum.ToShortDateString()}.");

            return true;
        }

        public async Task<bool> DodajNoviRacunAsync(FiskalniRacun racun)
        {
            try
            {
                ISkladisteServis trenutnoSkladiste = _magacinskiCentar;

                var dostupnaAmbalaza = _ambalazaRepo.Sve().FirstOrDefault(a => a.Status == StatusAmbalaze.Spakovana);

                if (dostupnaAmbalaza == null)
                {
                    _dogadjaji.Zabelezi("Nema dostupne ambalaže u skladištu!", TipEvidencije.ERROR);
                    return false;
                }

                bool uspehSkladiste = await trenutnoSkladiste.PosaljiPaketAsync(dostupnaAmbalaza.Id);

                if (!uspehSkladiste) return false;

                return _racunRepo.Dodaj(racun);
            }
            catch (Exception ex)
            {
                _dogadjaji.Zabelezi($"Greška pri dodavanju fiskalnog računa: {ex.Message}", TipEvidencije.ERROR);
                return false;
            }
        }
    }
}