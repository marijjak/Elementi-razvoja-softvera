using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class AmbalazaServis : IAmbalazaServis
    {
        private readonly IAmbalazaRepozitorijum _repo;
        private readonly IDogadjajiServis _dogadjajiServis;
        private readonly IPerfumeRepository _parfemRepo;

        public AmbalazaServis(
            IAmbalazaRepozitorijum repo,
            IDogadjajiServis dogadjajiServis,
            IPerfumeRepository parfemRepo)
        {
            _repo = repo;
            _dogadjajiServis = dogadjajiServis;
            _parfemRepo = parfemRepo;
        }

        public Ambalaza KreirajAmbalazu(string naziv, string adresaPosiljaoca, Guid skladisteId, IEnumerable<Guid> parfemIds)
        {
            if (string.IsNullOrWhiteSpace(adresaPosiljaoca))
            {
                throw new ArgumentException("Adresa pošiljaoca je obavezna.");
            }

            Ambalaza ambalaza = new Ambalaza
            {
                Naziv = string.IsNullOrWhiteSpace(naziv) ? "Ambalaža" : naziv,
                AdresaPosiljaoca = adresaPosiljaoca,
                SkladisteId = skladisteId,
                ParfemIds = parfemIds?.ToList() ?? new List<Guid>(),
                Status = StatusAmbalaze.Spakovana
            };

            _repo.Dodaj(ambalaza);

            if (!_dogadjajiServis.Zabelezi(
             $"Kreirana ambalaža '{ambalaza.Naziv}' za skladište {ambalaza.SkladisteId}.",
                TipEvidencije.INFO,
                        ambalaza.Id))
            {
                throw new InvalidOperationException("Neuspešno beleženje događaja za ambalažu.");
            }

            return ambalaza;
        }

        public IEnumerable<Ambalaza> SveAmbalaze()
        {
            return _repo.Sve();
        }

        public Ambalaza DodajParfemeUAmbalazu(Guid ambalazaId, IEnumerable<Guid> parfemIds)
        {
            var ambalaza = _repo.NadjiPoId(ambalazaId);
            if (ambalaza == null)
            {
                throw new InvalidOperationException("Ambalaža nije pronađena.");
            }

            var parfemiZaDodavanje = parfemIds
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (!parfemiZaDodavanje.Any())
            {
                throw new InvalidOperationException("Niste uneli nijedan validan ID parfema.");
            }

            var postojeciParfemi = _parfemRepo.Svi().Select(p => p.Id).ToHashSet();
            var nepostojeci = parfemiZaDodavanje.Where(id => !postojeciParfemi.Contains(id)).ToList();
            if (nepostojeci.Any())
            {
                throw new InvalidOperationException("Jedan ili više parfema ne postoji u sistemu.");
            }

            var zauzetiParfemi = _repo.Sve()
                .Where(a => a.Id != ambalazaId)
                .SelectMany(a => a.ParfemIds)
                .ToHashSet();

            var duplikati = parfemiZaDodavanje.Where(id => zauzetiParfemi.Contains(id)).ToList();
            if (duplikati.Any())
            {
                throw new InvalidOperationException("Parfem ne može biti u više ambalaža istovremeno.");
            }

            foreach (var parfemId in parfemiZaDodavanje)
            {
                if (!ambalaza.ParfemIds.Contains(parfemId))
                {
                    ambalaza.ParfemIds.Add(parfemId);
                }
            }

            if (!_repo.Azuriraj(ambalaza))
            {
                throw new InvalidOperationException("Neuspešno ažuriranje ambalaže.");
            }


            if (!_dogadjajiServis.Zabelezi(
                $"Dodato {parfemiZaDodavanje.Count} parfema u ambalažu '{ambalaza.Naziv}'.",
                TipEvidencije.INFO,
                ambalaza.Id))
            {
                throw new InvalidOperationException("Neuspešno beleženje događaja za ambalažu.");
            }

            return ambalaza;
        }
    }
}