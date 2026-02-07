using Domain.Enumeracije;
using Domain.Konstante;
using Domain.Modeli;
using Domain.PomocneMetode;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Korisnik {korisnik.KorisnickoIme} je pokušao neovlašćen pristup računima.");
                    return Enumerable.Empty<FiskalniRacun>();
                }

                _dogadjaji.Zabelezi($"Menadžer {korisnik.ImePrezime} je pregledao dnevni promet.", TipEvidencije.INFO);
                _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Menadžer {korisnik.KorisnickoIme} je pregledao listu računa.");

                return _racunRepo.GetSviRacuni();
            }
            catch
            {
                
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Došlo je do greške u bazi prilikom dobavljanja računa.");
                return Enumerable.Empty<FiskalniRacun>();
            }
        }

        public bool PokusajDobaviRacuneZaDan(Korisnik korisnik, DateTime datum, out List<FiskalniRacun> racuniZaDan)
        {

            var sviRacuni = _racunRepo.GetSviRacuni();
            var uspeh = RacunPomocneMetode.PokusajDobaviRacuneZaDan(korisnik, sviRacuni, datum, out racuniZaDan);

            if (!uspeh)
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Neovlašćen pokušaj dobavljanja izveštaja za dan: {datum.ToShortDateString()}.");
                return false;
            }

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
                    var zauzetiParfemi = _ambalazaRepo.Sve()
                      .SelectMany(a => a.ParfemIds)
                      .ToHashSet();

                    var parfemIds = racun.Stavke
                        .SelectMany(s => Enumerable.Repeat(s.Key, s.Value))
                        .ToList();

                    if (parfemIds.Any(id => zauzetiParfemi.Contains(id)))
                    {
                        _dogadjaji.Zabelezi("Parfem već postoji u drugoj ambalaži.", TipEvidencije.ERROR);
                        _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspela prodaja: Parfemi su već rezervisani u drugoj ambalaži.");
                        return false;
                    }

                    dostupnaAmbalaza = new Ambalaza
                    {
                        Naziv = "Automatska ambalaža",
                        AdresaPosiljaoca = "O'Sinjel De Or, Paris",
                        SkladisteId = Guid.Parse(KONSTANTE.DefaultSkladisteId),
                        ParfemIds = parfemIds,
                        Status = StatusAmbalaze.Spakovana
                    };

                    if (_ambalazaRepo.Dodaj(dostupnaAmbalaza) == null)
                    {
                        _dogadjaji.Zabelezi("Nije moguće kreirati novu ambalažu.", TipEvidencije.ERROR);
                        _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Greška: Sistem nije mogao da generiše automatsku ambalažu.");
                        return false;
                    }

                    _dogadjaji.Zabelezi("Kreirana je nova ambalaža za prodaju.", TipEvidencije.INFO, dostupnaAmbalaza.Id);
                    _logger.EvidentirajDogadjaj(TipEvidencije.INFO, "Automatski generisana ambalaža za potrebe nove prodaje.");
                }

                bool uspehSkladiste = await trenutnoSkladiste.PosaljiPaketAsync(dostupnaAmbalaza.Id);

                if (!uspehSkladiste)
                {
                    _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Skladište je odbilo slanje paketa za ambalažu: {dostupnaAmbalaza.Id}");
                    return false;
                }

                bool uspehRacun = _racunRepo.Dodaj(racun);

                if (uspehRacun)
                {
                    _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Prodaja uspešna. Izdat fiskalni račun ID: {racun.Id}");
                }
                else
                {
                    _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Fiskalni račun nije mogao biti sačuvan u bazi.");
                }

                return uspehRacun;
            }
            catch
            {
               
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Kritična greška u procesu prodaje. Operacija je prekinuta.");
                return false;
            }
        }
    }
}