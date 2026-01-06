using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly IAutentifikacijaServis _authServis;
        private readonly IBiljkeServis _biljkeServis;
        private Korisnik _ulogovan = new Korisnik();

        public OpcijeMeni(
              IAutentifikacijaServis authServis,
              IBiljkeServis biljkeServis)
        {
            _authServis = authServis;
            _biljkeServis = biljkeServis;
            _ulogovan = new Korisnik(); // Inicijalizuje se prazan
        }

        public void Pokreni()
        {
            while (true)
            {
                PrikaziLoginMeni();

                if (_ulogovan.KorisnickoIme == string.Empty)
                    return;

                PrikaziGlavniMeni();
            }
        }

        // ================= LOGIN / REG =================
        private void PrikaziLoginMeni()
        {
            _ulogovan = new Korisnik();

            while (_ulogovan.KorisnickoIme == string.Empty)
            {
                Console.Clear();
                Console.WriteLine("===== DOBRODOŠLI =====");
                Console.WriteLine("1. Prijava");
                Console.WriteLine("2. Registracija");
                Console.WriteLine("0. Izlaz");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine() ?? "";

                switch (izbor)
                {
                    case "1":
                        Prijava();
                        break;

                    case "2":
                        Registracija();
                        break;

                    case "0":
                        return;

                    default:
                        Pauza("Nevalidna opcija.");
                        break;
                }
            }
        }

        private void Prijava()
        {
            Console.Write("Korisničko ime: ");
            string korisnickoIme = Console.ReadLine() ?? "";

            Console.Write("Lozinka: ");
            string lozinka = Console.ReadLine() ?? "";

            var rezultat = _authServis.Prijava(korisnickoIme, lozinka);

            if (rezultat.Item1)
                _ulogovan = rezultat.Item2;
            else
                Pauza("Pogrešno korisničko ime ili lozinka.");
        }

        private void Registracija()
        {
            Console.Write("Korisničko ime: ");
            string korisnickoIme = Console.ReadLine() ?? "";

            Console.Write("Lozinka: ");
            string lozinka = Console.ReadLine() ?? "";

            Console.Write("Ime i prezime: ");
            string imePrezime = Console.ReadLine() ?? "";

            Korisnik novi = new Korisnik(
                korisnickoIme,
                lozinka,
                imePrezime,
                TipKorisnika.Prodavac // default uloga
            );

            var rezultat = _authServis.Registracija(novi);

            if (rezultat.Item1)
                _ulogovan = rezultat.Item2;
            else
                Pauza("Registracija neuspešna.");
        }

        // ================= GLAVNI MENI =================
        private void PrikaziGlavniMeni()
        {
            bool kraj = false;

            while (!kraj)
            {
                Console.Clear();
                Console.WriteLine($"Ulogovan korisnik: {_ulogovan.ImePrezime} ({_ulogovan.Uloga})");
                Console.WriteLine("====================================");
                Console.WriteLine("1. Moj profil");

                if (_ulogovan.Uloga == TipKorisnika.MenadzerProdaje)
                    Console.WriteLine("2. Menadžerske opcije");

                Console.WriteLine("0. Odjava");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine() ?? "";

                switch (izbor)
                {
                    case "1":
                        PrikaziProfil();
                        break;

                    case "2":
                        if (_ulogovan.Uloga == TipKorisnika.MenadzerProdaje)
                            Pauza("Menadžerske opcije (biće dodate kasnije).");
                        else
                            Pauza("Nemate pravo pristupa.");
                        break;

                    case "0":
                        _ulogovan = new Korisnik();
                        kraj = true;
                        break;

                    default:
                        Pauza("Nevalidna opcija.");
                        break;
                }
            }
        }

        private void PrikaziProfil()
        {
            Console.WriteLine("\n===== PROFIL =====");
            Console.WriteLine($"Ime i prezime: {_ulogovan.ImePrezime}");
            Console.WriteLine($"Korisničko ime: {_ulogovan.KorisnickoIme}");
            Console.WriteLine($"Uloga: {_ulogovan.Uloga}");
            Pauza("Pritisnite bilo koji taster za nastavak...");

        }

        private void Pauza(string poruka)
        {
            Console.WriteLine(poruka);
            Console.WriteLine("\nPritisnite bilo koji taster...");
            Console.ReadKey();
        }
    }
}
