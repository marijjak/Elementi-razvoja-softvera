using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repozitorijumi
{
    public class PerfumeRepository : IPerfumeRepository
    {
        private readonly IBazaPodataka _baza;

        public PerfumeRepository(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public Parfem Dodaj(Parfem parfem)
        {
            int redniBroj = _baza.Tabele.Parfemi.Count + 1;

            parfem.SerijskiBroj = $"PP-2025-{redniBroj:D4}";

            _baza.Tabele.Parfemi.Add(parfem);
            _baza.SacuvajPromene();

            return parfem;
        }

        public IEnumerable<Parfem> Svi()
        {
            return _baza.Tabele.Parfemi;
        }
        public bool AzurirajKolicinu(Guid parfemId, int novaKolicina)
        {
            var parfem = _baza.Tabele.Parfemi.FirstOrDefault(p => p.Id == parfemId);

            if (parfem == null || novaKolicina < 0)
            {
                return false;
            }

            parfem.KolicinaNaStanju = novaKolicina;
            _baza.SacuvajPromene();
            return true;
        }
    }
}
