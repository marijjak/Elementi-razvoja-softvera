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
                // Provera da li korisnik već postoji
                Korisnik postoji = PronadjiKorisnikaPoKorisnickomImenu(korisnik.KorisnickoIme);

                // Ako korisnik sa tim korisničkim imenom već postoji, ne dodaje se
                if (postoji.KorisnickoIme == string.Empty)
                {
                    // Jedinstveni ID se generiše na osnovu trenutnog vremena
                    korisnik.Id = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    // Ne postoji korisnik sa datim korisničkim imenom - dodati
                    bazaPodataka.Tabele.Korisnici.Add(korisnik);

                    // Čuvanje promena
                    bazaPodataka.SacuvajPromene();

                    return korisnik;
                }

                // Ako već postoji korisnik, vraća se prazan objekat
                return new Korisnik();
            }
            catch
            {
                // U slučaju greške vraća se prazan objekat
                return new Korisnik();
            }
        }

        public Korisnik PronadjiKorisnikaPoKorisnickomImenu(string korisnickoIme)
        {
            try
            {
                // Iterira kroz sve korisnike u bazi i traži korisnika sa odgovarajućim korisničkim imenom
                foreach (Korisnik korisnik in bazaPodataka.Tabele.Korisnici)
                {
                    if (korisnik.KorisnickoIme == korisnickoIme)
                        return korisnik;
                }

                // Ako nije pronađen nijedan korisnik sa tim korisničkim imenom vraća se prazan objekat
                return new Korisnik();
            }
            catch
            {
                // U slučaju greške vraća se prazan objekat
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
