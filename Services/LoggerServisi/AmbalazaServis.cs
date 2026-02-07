using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Domain.BazaPodataka;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class AmbalazaServis : IAmbalazaServis
    {
        private readonly IAmbalazaRepozitorijum _repo;
        private readonly IDogadjajiServis _dogadjaji;
        private readonly IPerfumeRepository _parfemRepo;
        private readonly ISkladisteServis _skladiste;
        private readonly ILoggerServis _logger;
        private readonly IBazaPodataka _baza;

        public AmbalazaServis(
            IAmbalazaRepozitorijum repo,
            IDogadjajiServis dogadjaji,
            IPerfumeRepository parfemRepo,
            ISkladisteServis skladiste,
            ILoggerServis logger,
            IBazaPodataka baza)
        {
            _repo = repo;
            _dogadjaji = dogadjaji;
            _parfemRepo = parfemRepo;
            _skladiste = skladiste;
            _logger = logger;
            _baza = baza;
        }

        public bool KreirajAmbalazu(string naziv, string adresaPosiljaoca, Guid skladisteId, IEnumerable<Guid> parfemIds, out Ambalaza ambalaza)
        {
            ambalaza = new Ambalaza();
            try
            {
                var nova = new Ambalaza
                {
                    Id = Guid.NewGuid(),
                    Naziv = naziv,
                    AdresaPosiljaoca = adresaPosiljaoca,
                    SkladisteId = skladisteId,
                    ParfemIds = parfemIds.ToList(),
                    Status = StatusAmbalaze.Spakovana
                };

                var rezultat = _repo.Dodaj(nova);
                if (rezultat != null)
                {
                    ambalaza = rezultat;
                    _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Kreirana ambalaža: {naziv}");
                    return true;
                }
                return false;
            }
            catch
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Greška pri kreiranju ambalaže.");
                return false;
            }
        }

        public bool DodajParfemeUAmbalazu(Guid ambalazaId, IEnumerable<Guid> parfemIds, out Ambalaza ambalaza)
        {
            ambalaza = new Ambalaza();
            try
            {
                var pronadjena = _repo.NadjiPoId(ambalazaId)!;
                if (pronadjena == null || pronadjena.Id == Guid.Empty)
                {
                    _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, "Pokušaj pakovanja u nepostojeću ambalažu.");

                   
                    _dogadjaji.Zabelezi($"NEUSPELO PAKOVANJE: Ambalaža {ambalazaId} nije nađena.", TipEvidencije.WARNING);
                    return false;
                }

                pronadjena.ParfemIds.AddRange(parfemIds);
                bool uspeh = _repo.Azuriraj(pronadjena);
                if (uspeh) ambalaza = pronadjena;
                return uspeh;
            }
            catch {
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, "Kritična greška pri radu sa bazom ambalaža.");
                _dogadjaji.Zabelezi("DESIO SE ERROR: Greška u servisu.", TipEvidencije.ERROR);
                return false; }
        }

        public bool PosaljiAmbalazu(Guid ambalazaId)
        {
            try
            {
                var ambalaza = _repo.NadjiPoId(ambalazaId)!;
                if (ambalaza == null) {
                    _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Pokušaj slanja nepostojeće ambalaže: {ambalazaId}");
                    return false; }

                ambalaza.Status = StatusAmbalaze.Poslata;
                return _repo.Azuriraj(ambalaza);
            }
            catch { return false; }
        }

        public IEnumerable<Ambalaza> Sve()
        {
            try { return _repo.Sve(); }
            catch { return Enumerable.Empty<Ambalaza>(); }
        }

        public bool ObrisiAmbalazu(Guid id)
        {
            try
            {
                var zaBrisanje = _baza.Tabele.Ambalaze.FirstOrDefault(a => a.Id == id);
                if (zaBrisanje != null)
                {
                    _baza.Tabele.Ambalaze.Remove(zaBrisanje);
                    _baza.SacuvajPromene();
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
    }
}