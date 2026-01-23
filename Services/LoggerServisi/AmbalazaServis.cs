using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public AmbalazaServis(IAmbalazaRepozitorijum repo, IDogadjajiServis dogadjajiServis)
        {
            _repo = repo;
            _dogadjajiServis = dogadjajiServis;
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
                ParfemIds = parfemIds?.ToList() ?? [],
                Status = StatusAmbalaze.Spakovana
            };

            _repo.Dodaj(ambalaza);

            _dogadjajiServis.Zabelezi(
                $"Kreirana ambalaža '{ambalaza.Naziv}' za skladište {ambalaza.SkladisteId}.",
                TipEvidencije.INFO,
                ambalaza.Id);

            return ambalaza;
        }

        public IEnumerable<Ambalaza> SveAmbalaze()
        {
            return _repo.Sve();
        }
    }
}
