using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.AutenftikacioniServisi
{
    public class AutentifikacioniServis : IAutentifikacijaServis
    {
        private readonly IKorisniciRepozitorijum korisniciRepozitorijum;

        public AutentifikacioniServis(IKorisniciRepozitorijum korisniciRepozitorijum)
        {
            this.korisniciRepozitorijum = korisniciRepozitorijum;
        }

        public (bool, Korisnik) Prijava(string korisnickoIme, string lozinka)
        {
            try
            {
                // Pronalazi korisnika po korisničkom imenu
                Korisnik korisnik = korisniciRepozitorijum.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme);

                // Ako korisnik ne postoji
                if (korisnik.KorisnickoIme == string.Empty)
                {
                    return (false, new Korisnik());
                }

                // Provera lozinke
                if (korisnik.Lozinka == lozinka)
                {
                    return (true, korisnik);
                }

                // Pogrešna lozinka
                return (false, new Korisnik());
            }
            catch
            {
                return (false, new Korisnik());
            }
        }

        public (bool, Korisnik) Registracija(Korisnik noviKorisnik)
        {
            try
            {
                // Validacija podataka
                if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Lozinka) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.ImePrezime))
                {
                    return (false, new Korisnik());
                }

                // Pokušaj dodavanja korisnika
                Korisnik dodatiKorisnik = korisniciRepozitorijum.DodajKorisnika(noviKorisnik);

                // Ako je ID postavljen, korisnik je uspešno dodat
                if (dodatiKorisnik.Id > 0)
                {
                    return (true, dodatiKorisnik);
                }

                // Korisnik već postoji ili greška
                return (false, new Korisnik());
            }
            catch
            {
                return (false, new Korisnik());
            }
        }
    }
}