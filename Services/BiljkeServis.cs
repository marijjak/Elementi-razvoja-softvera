using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
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
            if (biljka.JacinaArome <1 || biljka.JacinaArome > 5)
                throw new Exception("Jačina arome mora biti između 1 i 5.");

            return _repo.Dodaj(biljka);
        }

        public void PromeniStanje(Guid biljkaId)
        {
            var biljka = _repo.NadjiPoId(biljkaId);
            if (biljka == null) return;

            var novoStanje = biljka.Stanje switch
            {
                StanjeBiljke.Posadjena => StanjeBiljke.Ubrana,
                StanjeBiljke.Ubrana => StanjeBiljke.Preradjena,
                _ => biljka.Stanje
            };

            biljka.PromeniStanje(novoStanje);
        }

        public IEnumerable<Biljka> SveBiljke()
        {
            return _repo.Sve();
        }
        public bool ZasadiNovuBiljku(string naziv, string latinski, string zemlja, double jacina)
        {
            try
            {
                // Kreiramo novi model biljke
             
                Biljka novaBiljka = new Biljka(naziv, latinski,jacina, zemlja);

                // Postavljamo početno stanje prema zahtevu zadatka
                novaBiljka.PromeniStanje(StanjeBiljke.Posadjena);

             
                _repo.Dodaj(novaBiljka);

                _dogadjajiServis.Zabelezi($"Zasađena nova biljka: {novaBiljka.OpstiNaziv} ({novaBiljka.LatinskiNaziv})", "Sadnja", novaBiljka.Id);


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IzmeniMirisBiljke(Guid id, double procenat)
        {
            var biljka = _repo.NadjiPoId(id); // Pronalaženje biljke
            if (biljka == null) return false;

            double stariMiris = biljka.JacinaArome;


            biljka.PromeniJacinuArome(procenat);

            _repo.Dodaj(biljka);

            _dogadjajiServis.Zabelezi($"Promenjena jačina arome za '{biljka.OpstiNaziv}' sa {stariMiris:F1} na {biljka.JacinaArome:F1}", "Izmena", biljka.Id);

            return true;
        }

        public bool OznaciBiljkuKaoUbranu(Guid biljkaId)
        {
            var biljka = _repo.NadjiPoId(biljkaId);
            if (biljka == null)
                return false;

            biljka.OznaciKaoUbranu();

            _dogadjajiServis.Zabelezi($"Biljka '{biljka.OpstiNaziv}' je označena kao ubrana.", "Biljka", biljka.Id);

            _repo.Dodaj(biljka);

            return true;
        }
        public void PromeniJacinuUljaProcentualno(Guid id, double procenat)
        {
            var biljka = _repo.Sve().FirstOrDefault(b => b.Id == id);
            if (biljka != null)
            {
                // Ако је проценат 0.65, јачина постаје 65% тренутне вредности
                biljka.JacinaArome = biljka.JacinaArome * procenat;
                _repo.Azuriraj(biljka);
            }
        }

    }
}
