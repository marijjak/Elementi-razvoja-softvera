using Domain.Enumeracije;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DistributivniCentarServis : IDistributivniCentarServis, ISkladisniLogistickiServis, ISkladisteServis
    {
        private readonly IAmbalazaRepozitorijum _ambalazaRepo;
        private readonly IDogadjajiServis _dogadjajiServis;

        public DistributivniCentarServis(IAmbalazaRepozitorijum ambalazaRepo, IDogadjajiServis dogadjajiServis)
        {
            _ambalazaRepo = ambalazaRepo;
            _dogadjajiServis = dogadjajiServis;
        }

        public async Task<int> PosaljiPaketeAsync(IEnumerable<Guid> ambalazaIds)
        {
            if (ambalazaIds == null)
            {
                return 0;
            }

            var paketiZaSlanje = ambalazaIds
                .Distinct()
                .Select(id => _ambalazaRepo.NadjiPoId(id))
                .Where(a => a != null && a.Status == StatusAmbalaze.Spakovana)
                .Take(3)
                .ToList();

            if (!paketiZaSlanje.Any())
            {
                return 0;
            }

            await Task.Delay(500);

            foreach (var ambalaza in paketiZaSlanje)
            {
                if (ambalaza == null) continue;

                ambalaza.Status = StatusAmbalaze.Poslata;
                _ambalazaRepo.Azuriraj(ambalaza);

                _dogadjajiServis.Zabelezi(
                    $"Paket {ambalaza.Naziv} je otpremljen iz distributivnog centra za 0.5s.",
                    TipEvidencije.INFO,
                    ambalaza.Id);
            }

            return paketiZaSlanje.Count;
        }
        public async Task<bool> ProcesuirajIsporukuAsync(Guid ambalazaId)
        {
           
            var rezultat = await PosaljiPaketeAsync(new List<Guid> { ambalazaId });
            return rezultat > 0;
        }

        public bool PostojiSkladiste(Guid skladisteId) => true;
        public bool DodajAmbalazuUSkladiste(Guid skladisteId, int kolicina) => true;


        public async Task<bool> PosaljiPaketAsync(Guid ambalazaId)
        {
            return await ProcesuirajIsporukuAsync(ambalazaId);
        }
    }

}

