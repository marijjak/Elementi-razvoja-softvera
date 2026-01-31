using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class KorisniciRepozitorijum : IKorisniciRepozitorijum
    {
        IBazaPodataka bazaPodataka;

        public KorisniciRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public Korisnik DodajKorisnika(Korisnik korisnik)
        {
            try
            {
                Korisnik postoji = PronadjiKorisnikaPoKorisnickomImenu(korisnik.KorisnickoIme);

                if (postoji.KorisnickoIme == string.Empty)
                {
                    korisnik.Id = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    bazaPodataka.Tabele.Korisnici.Add(korisnik);

                    bazaPodataka.SacuvajPromene();

                    return korisnik;
                }

                return new Korisnik();
            }
            catch
            {
                return new Korisnik();
            }
        }

        public Korisnik PronadjiKorisnikaPoKorisnickomImenu(string korisnickoIme)
        {
            try
            {
                foreach (Korisnik korisnik in bazaPodataka.Tabele.Korisnici)
                {
                    if (korisnik.KorisnickoIme == korisnickoIme)
                        return korisnik;
                }

                return new Korisnik();
            }
            catch
            {
                return new Korisnik();
            }
        }

        public IEnumerable<Korisnik> SviKorisnici()
        {
            try
            {
                return bazaPodataka.Tabele.Korisnici;
            }
            catch
            {
                return [];
            }
        }
    }
}
