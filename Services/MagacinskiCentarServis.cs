using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Servisi;
using Domain.Repozitorijumi;
using Domain.Enumeracije;

namespace Services
{
    public class MagacinskiCentarServis : IMagacinskiCentarServis, ISkladisniLogistickiServis, ISkladisteServis
    {
        private readonly IAmbalazaRepozitorijum _ambalazaRepo;
        private readonly IDogadjajiServis _dogadjajiServis;

        public MagacinskiCentarServis(IAmbalazaRepozitorijum ambalazaRepo, IDogadjajiServis dogadjajiServis)
        {
            _ambalazaRepo = ambalazaRepo;
            _dogadjajiServis = dogadjajiServis;
        }

        public async Task<bool> PosaljiPaketAsync(Guid ambalazaId)
        {
            var ambalaza = _ambalazaRepo.NadjiPoId(ambalazaId);
            if (ambalaza == null) return false;

            // Simulacija vremena nabavke od 2.5 sekunde
            await Task.Delay(2500);

            ambalaza.Status = StatusAmbalaze.Poslata;
            _ambalazaRepo.Azuriraj(ambalaza);

            _dogadjajiServis.Zabelezi($"Paket {ambalaza.Naziv} je otpremljen nakon 2.5s čekanja.", TipEvidencije.INFO, ambalazaId);

            return true;
        }
        public async Task<bool> ProcesuirajIsporukuAsync(Guid ambalazaId)
        => await PosaljiPaketAsync(ambalazaId);

        // Ove metode su neophodne da bi ispoštovao ISkladisteServis

        public bool PostojiSkladiste(Guid skladisteId)
        {
            // Možeš vratiti true ili dodati pravu proveru ako je potrebno
            return true;
        }

        public bool DodajAmbalazuUSkladiste(Guid skladisteId, int kolicina)
        {
            // Privremena implementacija da bi se kod kompajlirao
            return true;
        }
    }
}