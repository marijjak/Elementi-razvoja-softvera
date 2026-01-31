using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Domain.PomocneMetode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BiljkeServis : IBiljkeServis
    {
        private readonly IBiljkeRepozitorijum _repo;
        private readonly IDogadjajiServis _dogadjajiServis;

        public BiljkeServis(IBiljkeRepozitorijum repo, IDogadjajiServis dogadjajiServis)
        {
            _repo = repo;
            _dogadjajiServis = dogadjajiServis;
        }

        public Biljka DodajBiljku(Biljka biljka)
        {
            if (biljka.JacinaArome < 1 || biljka.JacinaArome > 5)
                throw new Exception("Jačina arome mora biti između 1 i 5.");

            return _repo.Dodaj(biljka);
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

            if (!BiljkaPomocne.PromeniStanje(biljka, novoStanje))
            {
                return false;
            }

            return _repo.Azuriraj(biljka);
        }

        public IEnumerable<Biljka> SveBiljke()
        {
            return _repo.Sve();
        }
        public bool ZasadiNovuBiljku(string naziv, string latinski, string zemlja, double jacina)
        {
            try
            {
           

                Biljka novaBiljka = new Biljka(naziv, latinski, jacina, zemlja);

                if (!BiljkaPomocne.PromeniStanje(novaBiljka, StanjeBiljke.Posadjena))
                {
                    return false;
                }


                _repo.Dodaj(novaBiljka);


                if (!_dogadjajiServis.Zabelezi($"Zasađena nova biljka: {novaBiljka.OpstiNaziv} ({novaBiljka.LatinskiNaziv})", TipEvidencije.INFO, novaBiljka.Id))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _dogadjajiServis.Zabelezi($"Greška pri sadnji biljke {naziv}: {ex.Message}", TipEvidencije.ERROR);
                return false;
            }
        }
        public bool IzmeniMirisBiljke(Guid id, double procenat)
        {
            var biljka = _repo.NadjiPoId(id);
            if (biljka == null) return false;

            double stariMiris = biljka.JacinaArome;


            if (!BiljkaPomocne.PromeniJacinuArome(biljka, procenat))
            {
                return false;
            }
            _repo.Dodaj(biljka);

            if (!_dogadjajiServis.Zabelezi($"Promenjena jačina arome za '{biljka.OpstiNaziv}' sa {stariMiris:F1} na {biljka.JacinaArome:F1}", TipEvidencije.INFO, biljka.Id))
            {
                return false;
            }
            return true;
        }

        public bool OznaciBiljkuKaoUbranu(Guid biljkaId)
        {
            var biljka = _repo.NadjiPoId(biljkaId);
            if (biljka == null)
                return false;

            if (!BiljkaPomocne.OznaciKaoUbranu(biljka))
            {
                return false;
            }


            if (!_dogadjajiServis.Zabelezi($"Biljka '{biljka.OpstiNaziv}' je označena kao ubrana.", TipEvidencije.INFO, biljka.Id))
            {
                return false;
            }

            _repo.Dodaj(biljka);

            return true;
        }
        public bool PromeniJacinuUljaProcentualno(string unos, double procenat)
        {
            var biljka = _repo.Sve().FirstOrDefault(b =>
        b.Id.ToString() == unos ||
        b.OpstiNaziv.Equals(unos, StringComparison.OrdinalIgnoreCase));
            if (biljka != null)
            {

                try
                {
                    double staraVrednost = biljka.JacinaArome;

                    biljka.JacinaArome = biljka.JacinaArome * procenat;
                    if (biljka.JacinaArome > 5.0) biljka.JacinaArome = 5.0;
                    if (!_repo.Azuriraj(biljka))
                    {
                        return false;
                    }

                    double procenatPrikaz = procenat * 100;

                    if (!_dogadjajiServis.Zabelezi($"Procentualna promena jačine ulja za biljku '{biljka.OpstiNaziv}' sa {staraVrednost:F1} na {biljka.JacinaArome:F1} (Primenjeno {procenatPrikaz}%).", TipEvidencije.INFO, biljka.Id))
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _dogadjajiServis.Zabelezi($"Greška pri procentualnoj izmeni: {ex.Message}", TipEvidencije.ERROR, biljka.Id);
                    return false;
                }

            }
            else
            {
                _dogadjajiServis.Zabelezi($"BILJKA NIJE PRONAĐENA ZA ID: {unos}", TipEvidencije.ERROR);
                return false;
            }
            return true;

        }
    }
}
