
using System.Linq;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services
{
    public class AmbalazaServis : IAmbalazaServis
    {
        private readonly IAmbalazaRepozitorijum _repo;
        private readonly IDogadjajiServis _dogadjajiServis;
        private readonly IPerfumeRepository _parfemRepo;
        private readonly ISkladisteServis _skladisteServis;

        public AmbalazaServis(
            IAmbalazaRepozitorijum repo,
            IDogadjajiServis dogadjajiServis,
           IPerfumeRepository parfemRepo,
            ISkladisteServis skladisteServis)
        {
            _repo = repo;
            _dogadjajiServis = dogadjajiServis;
            _parfemRepo = parfemRepo;
            _skladisteServis = skladisteServis;
        }

        public Ambalaza KreirajAmbalazu(string naziv, string adresaPosiljaoca, Guid skladisteId, IEnumerable<Guid> parfemIds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(adresaPosiljaoca))
                {
                    _dogadjajiServis.Zabelezi("Adresa pošiljaoca je obavezna.", TipEvidencije.WARNING);
                    return null;
                }
                if (!_skladisteServis.PostojiSkladiste(skladisteId))
                {
                    _dogadjajiServis.Zabelezi("Skladište nije pronađeno.", TipEvidencije.WARNING);
                    return null;
                }
                if (!_skladisteServis.DodajAmbalazuUSkladiste(skladisteId, 1))
                {
                    _dogadjajiServis.Zabelezi("Skladište nema dovoljno kapaciteta za novu ambalažu.", TipEvidencije.WARNING);
                    return null;
                }
                var parfemiZaDodavanje = parfemIds?
                    .Where(id => id != Guid.Empty)
                    .ToList() ?? new List<Guid>();

                if (parfemiZaDodavanje.Any())
                {
                    var parfemi = _parfemRepo.Svi().ToList();
                    var parfemiMap = parfemi.ToDictionary(p => p.Id, p => p);
                    var trazeneKolicine = parfemiZaDodavanje
                        .GroupBy(id => id)
                        .ToDictionary(g => g.Key, g => g.Count());

                    foreach (var zahtev in trazeneKolicine)
                    {
                        if (!parfemiMap.TryGetValue(zahtev.Key, out var parfem))
                        {
                            _dogadjajiServis.Zabelezi("Jedan ili više parfema ne postoji u sistemu.", TipEvidencije.WARNING);
                            return null;
                        }

                        if (parfem.KolicinaNaStanju < zahtev.Value)
                        {
                            _dogadjajiServis.Zabelezi("Nema dovoljno parfema na stanju.", TipEvidencije.WARNING);
                            return null;
                        }
                    }

                    foreach (var zahtev in trazeneKolicine)
                    {
                        var parfem = parfemiMap[zahtev.Key];
                        var novaKolicina = parfem.KolicinaNaStanju - zahtev.Value;
                        if (!_parfemRepo.AzurirajKolicinu(parfem.Id, novaKolicina))
                        {
                            _dogadjajiServis.Zabelezi("Neuspešno ažuriranje stanja parfema.", TipEvidencije.ERROR);
                            return null;
                        }
                    }
                }

                Ambalaza ambalaza = new Ambalaza
                {
                    Naziv = string.IsNullOrWhiteSpace(naziv) ? "Ambalaža" : naziv,
                    AdresaPosiljaoca = adresaPosiljaoca,
                    SkladisteId = skladisteId,
                    ParfemIds = parfemiZaDodavanje,
                    Status = StatusAmbalaze.Spakovana
                };

                _repo.Dodaj(ambalaza);

                if (!_dogadjajiServis.Zabelezi(
                 $"Kreirana ambalaža '{ambalaza.Naziv}' za skladište {ambalaza.SkladisteId}.",
                    TipEvidencije.INFO,
                            ambalaza.Id))
                {
                    return null;
                }

                return ambalaza;
            }
            catch (Exception ex)
            {
                _dogadjajiServis.Zabelezi($"Greška pri kreiranju ambalaže: {ex.Message}", TipEvidencije.ERROR);
                return null;
            }
        }

        public IEnumerable<Ambalaza> SveAmbalaze()
        {
            return _repo.Sve();
        }

        public Ambalaza DodajParfemeUAmbalazu(Guid ambalazaId, IEnumerable<Guid> parfemIds)
        {
            try
            {
                var ambalaza = _repo.NadjiPoId(ambalazaId);
                if (ambalaza == null)
                {
                    _dogadjajiServis.Zabelezi("Ambalaža nije pronađena.", TipEvidencije.WARNING);
                    return null;
                }

                var parfemiZaDodavanje = parfemIds?
                    .Where(id => id != Guid.Empty)
                    .ToList() ?? new List<Guid>();

                if (!parfemiZaDodavanje.Any())
                {
                    _dogadjajiServis.Zabelezi("Niste uneli nijedan validan ID parfema.", TipEvidencije.WARNING);
                    return null;
                }

                var parfemi = _parfemRepo.Svi().ToList();
                var parfemiMap = parfemi.ToDictionary(p => p.Id, p => p);
                var trazeneKolicine = parfemiZaDodavanje
                    .GroupBy(id => id)
                    .ToDictionary(g => g.Key, g => g.Count());

                foreach (var zahtev in trazeneKolicine)
                {
                    if (!parfemiMap.TryGetValue(zahtev.Key, out var parfem))
                    {
                        _dogadjajiServis.Zabelezi("Jedan ili više parfema ne postoji u sistemu.", TipEvidencije.WARNING);
                        return null;
                    }

                    if (parfem.KolicinaNaStanju < zahtev.Value)
                    {
                        _dogadjajiServis.Zabelezi("Nema dovoljno parfema na stanju.", TipEvidencije.WARNING);
                        return null;
                    }
                }

                foreach (var zahtev in trazeneKolicine)
                {
                    var parfem = parfemiMap[zahtev.Key];
                    var novaKolicina = parfem.KolicinaNaStanju - zahtev.Value;
                    if (!_parfemRepo.AzurirajKolicinu(parfem.Id, novaKolicina))
                    {
                        _dogadjajiServis.Zabelezi("Neuspešno ažuriranje stanja parfema.", TipEvidencije.ERROR);
                        return null;
                    }
                }

                foreach (var parfemId in parfemiZaDodavanje)
                {
                    ambalaza.ParfemIds.Add(parfemId);
                }

                if (!_repo.Azuriraj(ambalaza))
                {
                    _dogadjajiServis.Zabelezi("Neuspešno ažuriranje ambalaže.", TipEvidencije.ERROR);
                    return null;
                }


                if (!_dogadjajiServis.Zabelezi(
                    $"Dodato {parfemiZaDodavanje.Count} parfema u ambalažu '{ambalaza.Naziv}'.",
                    TipEvidencije.INFO,
                    ambalaza.Id))
                {
                    return null;
                }

                return ambalaza;
            }
            catch (Exception ex)
            {
                _dogadjajiServis.Zabelezi($"Greška pri dodavanju parfema u ambalažu: {ex.Message}", TipEvidencije.ERROR);
                return null;
            }
        }
    }
}