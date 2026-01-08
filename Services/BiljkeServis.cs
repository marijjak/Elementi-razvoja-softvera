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

        public BiljkeServis(IBiljkeRepozitorijum repo)
        {
            _repo = repo;
        }

        public Biljka DodajBiljku(Biljka biljka)
        {
            if (biljka.JacinaArome < 1 || biljka.JacinaArome > 5)
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
             
                Biljka novaBiljka = new Biljka(naziv, latinski,(int)jacina, zemlja);

                // Postavljamo početno stanje prema zahtevu zadatka
                novaBiljka.PromeniStanje(StanjeBiljke.Posadjena);

             
                _repo.Dodaj(novaBiljka);
             

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

            biljka.PromeniJacinuArome(procenat);

            _repo.Dodaj(biljka); 
            return true;
        }

    }
}
