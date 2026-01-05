using System;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 namespace Domain.Repozitorijumi
{
    public interface IBiljkeRepozitorijum
    {
        // Definicija metoda za rad sa biljkama ovde 
        Biljka Dodaj(Biljka biljka);
        Biljka NadjiPoId(Guid id);
        IEnumerable<Biljka> Sve();
    }
}