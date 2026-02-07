using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Domain.PomocneMetode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class BiljkeServis : IBiljkeServis
    {
        private readonly IBiljkeRepozitorijum _repo;
        private readonly IDogadjajiServis _dogadjajiServis;
        private readonly ILoggerServis _loggerServis;

        public BiljkeServis(IBiljkeRepozitorijum repo, IDogadjajiServis dogadjajiServis, ILoggerServis loggerServis)
        {
            _repo = repo;
            _dogadjajiServis = dogadjajiServis;
            _loggerServis = loggerServis;
        }

        public bool DodajBiljku(Biljka biljka)
        {
            try
            {
                if (biljka.JacinaArome < 1.0 || biljka.JacinaArome > 5.0)
                {
                    _dogadjajiServis.Zabelezi("Jačina arome мора бити између 1 и 5.", TipEvidencije.WARNING);
                    _loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Nevalidna aroma za biljku: {biljka.OpstiNaziv}");
                    return false;
                }

                if (_repo.Dodaj(biljka) != null)
                {
                    _loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Dodata biljka: {biljka.OpstiNaziv}");
                    return true;
                }
                return false;
            }
            catch
            {
                _loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Greška pri dodavanju biljke u bazu.");
                return false;
            }
        }

        public bool ZasadiNovuBiljku(string naziv, string latinski, string zemlja, double jacina)
        {
            try
            {
                Biljka novaBiljka = new Biljka(naziv, latinski, jacina, zemlja);

                if (!BiljkaPomocne.PromeniStanje(novaBiljka, StanjeBiljke.Posadjena)) return false;

                if (_repo.Dodaj(novaBiljka) == null) return false;

                _loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Zasađena nova biljka: {naziv}");

                if (jacina > 4.0)
                {
                    _loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Biljka {naziv} ima visoku aromu: {jacina:F2}");
                }

                return _dogadjajiServis.Zabelezi($"Zasađena nova biljka: {novaBiljka.OpstiNaziv}", TipEvidencije.INFO, novaBiljka.Id);
            }
            catch
            {
                _loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Kritična greška pri sadnji biljke: {naziv}");
                return false;
            }
        }

        public bool PromeniJacinuUljaProcentualno(string unos, double procenat)
        {
            var biljka = _repo.Sve().FirstOrDefault(b =>
                b.Id.ToString() == unos ||
                b.OpstiNaziv.Equals(unos, StringComparison.OrdinalIgnoreCase));

            if (biljka == null)
            {
                _loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Biljka '{unos}' nije pronađena za promenu arome.");
                _dogadjajiServis.Zabelezi($"BILJKA NIJE PRONAĐENA ZA ID: {unos}", TipEvidencije.ERROR);
                return false;
            }

            try
            {
                double staraVrednost = biljka.JacinaArome;

                if (!BiljkaPomocne.PromeniJacinuArome(biljka, procenat)) return false;

                if (!_repo.Azuriraj(biljka)) return false;

                _loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Aroma promenjena: {biljka.OpstiNaziv} ({staraVrednost:F1} -> {biljka.JacinaArome:F1})");

                return _dogadjajiServis.Zabelezi($"Procentualna promena jačine ulja za biljku '{biljka.OpstiNaziv}'", TipEvidencije.INFO, biljka.Id);
            }
            catch
            {
                _loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Greška pri ažuriranju arome u bazi za: {biljka.OpstiNaziv}");
                return false;
            }
        }

        public bool OznaciBiljkuKaoUbranu(Guid biljkaId)
        {
            var biljka = _repo.NadjiPoId(biljkaId);
            if (biljka == null) return false;

            if (!BiljkaPomocne.OznaciKaoUbranu(biljka)) return false;

            if (_repo.Azuriraj(biljka))
            {
                _loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Biljka {biljka.OpstiNaziv} je ubrana.");
                return _dogadjajiServis.Zabelezi($"Biljka '{biljka.OpstiNaziv}' je označena kao ubrana.", TipEvidencije.INFO, biljka.Id);
            }
            return false;
        }

        public bool PromeniStanje(Guid biljkaId)
        {
            var biljka = _repo.NadjiPoId(biljkaId);
            if (biljka == null) return false;

            var novoStanje = biljka.Stanje switch
            {
                StanjeBiljke.Posadjena => StanjeBiljke.Ubrana,
                StanjeBiljke.Ubrana => StanjeBiljke.Preradjena,
                _ => biljka.Stanje
            };

            if (!BiljkaPomocne.PromeniStanje(biljka, novoStanje)) return false;

            bool uspeh = _repo.Azuriraj(biljka);
            if (uspeh) _loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Stanje promenjeno за {biljka.OpstiNaziv} na {novoStanje}");

            return uspeh;
        }

        public IEnumerable<Biljka> SveBiljke() => _repo.Sve();
    }
}