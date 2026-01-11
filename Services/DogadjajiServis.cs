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
    public class DogadjajiServis : IDogadjajiServis
    {
        private readonly IDogadjajiRepozitorijum _repo;

        public DogadjajiServis(IDogadjajiRepozitorijum repo)
        {
            _repo = repo;
        }

        public void Zabelezi(string opis, string tip, Guid? entitetId = null)
        {
            var dogadjaj = new Dogadjaj(opis, tip, entitetId);
            _repo.Dodaj(dogadjaj);
        }

        public IEnumerable<Dogadjaj> SviDogadjaji()
        {
            return _repo.Sve()
                .OrderByDescending(d => d.Vreme);
        }
    }
}
