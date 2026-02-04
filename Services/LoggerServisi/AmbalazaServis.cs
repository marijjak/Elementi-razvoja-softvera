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

        public bool KreirajAmbalazu(string naziv, string adresaPosiljaoca, Guid skladisteId, IEnumerable<Guid> parfemIds, out Ambalaza ambalaza)
        {
            ambalaza = null;
            try
            {
                if (string.IsNullOrWhiteSpace(adresaPosiljaoca))
                {
                    _dogadjajiServis.Zabelezi("Adresa pošiljaoca je obavezna.", TipEvidencije.WARNING);
                    return false;
                }
                if (!_skladisteServis.PostojiSkladiste(skladisteId))
                {
                    _dogadjajiServis.Zabelezi("Skladište nije pronađeno.", TipEvidencije.WARNING);
                    return false;
                }
                if (!_skladisteServis.DodajAmbalazuUSkladiste(skladisteId, 1))
                {
                    _dogadjajiServis.Zabelezi("Skladište nema dovoljno kapaciteta za novu ambalažu.", TipEvidencije.WARNING);
                    return false;
                }
                var parfemiZaDodavanje = parfemIds?
                    .Where(id => id != Guid.Empty)
                    .ToList() ?? new List<Guid>();

                if (parfemiZaDodavanje.Any())
                {
                    var zauzetiParfemi = _repo.Sve()
                        .SelectMany(a => a.ParfemIds)
                        .ToHashSet();
                    if (parfemiZaDodavanje.Any(id => zauzetiParfemi.Contains(id)))
                    {
                        _dogadjajiServis.Zabelezi("Parfem može biti u samo jednoj ambalaži.", TipEvidencije.WARNING);
                        return false;
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
                            return false;
                        }

                        if (parfem.KolicinaNaStanju < zahtev.Value)
                        {
                            _dogadjajiServis.Zabelezi("Nema dovoljno parfema na stanju.", TipEvidencije.WARNING);
                            return false;
                        }
                    }

                    foreach (var zahtev in trazeneKolicine)
                    {
                        var parfem = parfemiMap[zahtev.Key];
                        var novaKolicina = parfem.KolicinaNaStanju - zahtev.Value;
                        if (!_parfemRepo.AzurirajKolicinu(parfem.Id, novaKolicina))
                        {
                            _dogadjajiServis.Zabelezi("Neuspešno ažuriranje stanja parfema.", TipEvidencije.ERROR);
                            return false;
                        }
                    }
                }

                ambalaza = new Ambalaza
                {
                    Naziv = string.IsNullOrWhiteSpace(naziv) ? "Ambalaža" : naziv,
                    AdresaPosiljaoca = adresaPosiljaoca,
                    SkladisteId = skladisteId,
                    ParfemIds = parfemiZaDodavanje,
                    Status = StatusAmbalaze.Spakovana
                };

                if (_repo.Dodaj(ambalaza) == null)
                {
                    return false;
                }

                if (!_dogadjajiServis.Zabelezi(
                 $"Kreirana ambalaža '{ambalaza.Naziv}' za skladište {ambalaza.SkladisteId}.",
                    TipEvidencije.INFO,
                            ambalaza.Id))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _dogadjajiServis.Zabelezi($"Greška pri kreiranju ambalaže: {ex.Message}", TipEvidencije.ERROR);
                return false;
            }
        }

        public IEnumerable<Ambalaza> SveAmbalaze()
        {
            return _repo.Sve();
        }

        public bool DodajParfemeUAmbalazu(Guid ambalazaId, IEnumerable<Guid> parfemIds, out Ambalaza ambalaza)
        {
            ambalaza = null;
            try
            {
                ambalaza = _repo.NadjiPoId(ambalazaId);
                if (ambalaza == null)
                {
                    _dogadjajiServis.Zabelezi("Ambalaža nije pronađena.", TipEvidencije.WARNING);
                    return false;
                }

                var parfemiZaDodavanje = parfemIds?
                    .Where(id => id != Guid.Empty)
                    .ToList() ?? new List<Guid>();

                if (!parfemiZaDodavanje.Any())
                {
                    _dogadjajiServis.Zabelezi("Niste uneli nijedan validan ID parfema.", TipEvidencije.WARNING);
                    return false;
                }

                var zauzetiParfemi = _repo.Sve()
                    .Where(a => a.Id != ambalazaId)
                    .SelectMany(a => a.ParfemIds)
                    .ToHashSet();
                if (parfemiZaDodavanje.Any(id => zauzetiParfemi.Contains(id)))
                {
                    _dogadjajiServis.Zabelezi("Parfem može biti u samo jednoj ambalaži.", TipEvidencije.WARNING);
                    return false;
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
                        return false;
                    }

                    if (parfem.KolicinaNaStanju < zahtev.Value)
                    {
                        _dogadjajiServis.Zabelezi("Nema dovoljno parfema na stanju.", TipEvidencije.WARNING);
                        return false;
                    }
                }

                foreach (var zahtev in trazeneKolicine)
                {
                    var parfem = parfemiMap[zahtev.Key];
                    var novaKolicina = parfem.KolicinaNaStanju - zahtev.Value;
                    if (!_parfemRepo.AzurirajKolicinu(parfem.Id, novaKolicina))
                    {
                        _dogadjajiServis.Zabelezi("Neuspešno ažuriranje stanja parfema.", TipEvidencije.ERROR);
                        return false;
                    }
                }

                foreach (var parfemId in parfemiZaDodavanje)
                {
                    ambalaza.ParfemIds.Add(parfemId);
                }

                if (!_repo.Azuriraj(ambalaza))
                {
                    _dogadjajiServis.Zabelezi("Neuspešno ažuriranje ambalaže.", TipEvidencije.ERROR);
                    return false;
                }


                if (!_dogadjajiServis.Zabelezi(
                    $"Dodato {parfemiZaDodavanje.Count} parfema u ambalažu '{ambalaza.Naziv}'.",
                    TipEvidencije.INFO,
                    ambalaza.Id))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _dogadjajiServis.Zabelezi($"Greška pri dodavanju parfema u ambalažu: {ex.Message}", TipEvidencije.ERROR);
                return false;
            }
        }
    }
}