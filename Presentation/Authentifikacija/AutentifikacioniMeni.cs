using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Authentifikacija
{
    public class AutentifikacioniMeni
    {
        private readonly IAutentifikacijaServis autentifikacijaServis;

        public AutentifikacioniMeni(IAutentifikacijaServis autentifikacijaServis)
        {
            this.autentifikacijaServis = autentifikacijaServis;
        }

        public bool TryLogin(out Korisnik korisnik)
        {
            korisnik = new Korisnik();
            bool uspesnaPrijava = false;
            string? korisnickoIme = "", lozinka = "";

            Console.WriteLine("\n========== PRIJAVA ==========");
            Console.Write("Korisničko ime: ");
            korisnickoIme = Console.ReadLine() ?? "";

            Console.Write("Lozinka: ");
            lozinka = Console.ReadLine() ?? "";

            (uspesnaPrijava, korisnik) = autentifikacijaServis.Prijava(korisnickoIme.Trim(), lozinka.Trim());

            return uspesnaPrijava;
        }

        public bool TryRegister(out Korisnik korisnik)
        {
            korisnik = new Korisnik();
            bool uspesnaRegistracija = false;

            Console.WriteLine("\n========== REGISTRACIJA ==========");

            Console.Write("Korisničko ime: ");
            string korisnickoIme = Console.ReadLine() ?? "";

            Console.Write("Lozinka: ");
            string lozinka = Console.ReadLine() ?? "";

            Console.Write("Ime i prezime: ");
            string imePrezime = Console.ReadLine() ?? "";

            Console.WriteLine("\nIzaberite ulogu:");
            Console.WriteLine("1. Prodavac");
            Console.WriteLine("2. Menadžer prodaje");
            Console.Write("Uloga (1-2): ");
            string ulogaOpcija = Console.ReadLine() ?? "";

            TipKorisnika uloga = ulogaOpcija.Trim() == "2"
                ? TipKorisnika.MenadzerProdaje
                : TipKorisnika.Prodavac;

           
            Korisnik noviKorisnik = new Korisnik(
                korisnickoIme.Trim(),
                lozinka.Trim(),
                imePrezime.Trim(),
                uloga
            );

           
            (uspesnaRegistracija, korisnik) = autentifikacijaServis.Registracija(noviKorisnik);

            return uspesnaRegistracija;
        }
    }
}