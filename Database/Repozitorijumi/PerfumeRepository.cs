using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.PomocneMetode;
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

        public Parfem? Dodaj(Parfem parfem)
        {
            try
            {
                if (!PomocneParfem.GenerisiSerijskiBroj(parfem))
                {
                    return null;
                }

                _baza.Tabele.Parfemi.Add(parfem);
                _baza.SacuvajPromene();

                return parfem;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Parfem> Svi()
        {
            try
            {
                return _baza.Tabele.Parfemi;
            }
            catch
            {
                return [];
            }
        }
        public bool AzurirajKolicinu(Guid parfemId, int novaKolicina)
        {
            try
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
            catch
            {
                return false;
            }
        }
    }
}