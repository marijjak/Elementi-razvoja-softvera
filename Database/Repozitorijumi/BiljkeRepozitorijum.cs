using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class BiljkeRepozitorijum : IBiljkeRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public BiljkeRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public Biljka? Dodaj(Biljka biljka)
        {
          try
            {
                var postojeca = _baza.Tabele.Biljke.FirstOrDefault(b => b.Id == biljka.Id);

                if (postojeca != null)
                {
                    postojeca.JacinaArome = biljka.JacinaArome;
                    postojeca.Stanje = biljka.Stanje;
                }
                else
                {
                    _baza.Tabele.Biljke.Add(biljka);
                }

                _baza.SacuvajPromene();
                return biljka;
            }
            catch
            {
                return null;
            }

        }

        public Biljka? NadjiPoId(Guid id)
        {
            try
            {
                return _baza.Tabele.Biljke
                    .FirstOrDefault(b => b.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Biljka> Sve()
        {
            try
            {
                return _baza.Tabele.Biljke;
            }
            catch
            {
                return [];
            }
        }
        public bool ObrisiPrazne()
        {
            try
            {
                var zaBrisanje = _baza.Tabele.Biljke.Where(b => string.IsNullOrEmpty(b.OpstiNaziv)).ToList();

                foreach (var b in zaBrisanje)
                {
                    _baza.Tabele.Biljke.Remove(b);
                }

                _baza.SacuvajPromene();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Azuriraj(Biljka biljka)
        {
            try
            {
                var postojeca = _baza.Tabele.Biljke.FirstOrDefault(b => b.Id == biljka.Id);

                if (postojeca == null)
                {
                    return false;
                }

                postojeca.Stanje = biljka.Stanje;
                postojeca.JacinaArome = biljka.JacinaArome;
                postojeca.OpstiNaziv = biljka.OpstiNaziv;
                postojeca.LatinskiNaziv = biljka.LatinskiNaziv;
                postojeca.ZemljaPorekla = biljka.ZemljaPorekla;

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
