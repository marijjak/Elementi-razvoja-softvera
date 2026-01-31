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
                Korisnik korisnik = korisniciRepozitorijum.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme);

                if (korisnik.KorisnickoIme == string.Empty)
                {
                    return (false, new Korisnik());
                }

                if (korisnik.Lozinka == lozinka)
                {
                    return (true, korisnik);
                }

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
                if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Lozinka) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.ImePrezime))
                {
                    return (false, new Korisnik());
                }

                Korisnik dodatiKorisnik = korisniciRepozitorijum.DodajKorisnika(noviKorisnik);

                if (dodatiKorisnik.Id > 0)
                {
                    return (true, dodatiKorisnik);
                }

                return (false, new Korisnik());
            }
            catch
            {
                return (false, new Korisnik());
            }
        }
    }
}