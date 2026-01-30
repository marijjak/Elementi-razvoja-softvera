using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repozitorijumi
{
    public interface IPerfumeRepository
    {
        Parfem Dodaj(Parfem parfem);
        IEnumerable<Parfem> Svi();

        bool AzurirajKolicinu(Guid parfemId, int novaKolicina);
    }
}
