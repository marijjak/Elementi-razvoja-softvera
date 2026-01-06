using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly IAutentifikacijaServis _authServis;
        private readonly IBiljkeServis _biljkeServis;
        private Korisnik _ulogovan;

        public OpcijeMeni(
            IAutentifikacijaServis authServis,
            IBiljkeServis biljkeServis,
            Korisnik ulogovan)
        {
            _authServis = authServis;
            _biljkeServis = biljkeServis;
            _ulogovan = ulogovan;
        }

        // ==================== GLAVNI MENI ====================
        public void PrikaziGlavniMeni()
        {
            bool kraj = false;

            while (!kraj)
            {
                Console.Clear();
                Console.WriteLine($"\n===== GLAVNI MENI - Ulogovan korisnik: {_ulogovan.ImePrezime} ({_ulogovan.Uloga}) =====");

                Console.WriteLine("1. Moj profil");

                if (_ulogovan.Uloga == TipKorisnika.MenadzerProdaje)
                {
                    Console.WriteLine("2. Menadžerske opcije");
                }

                Console.WriteLine("0. Odjava");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine() ?? "";

                switch (izbor.Trim())
                {
                    case "1":
                        PrikaziProfil();
                        break;

                    case "2":
                        if (_ulogovan.Uloga == TipKorisnika.MenadzerProdaje)
                        {
                            PrikaziMenadzerMeni();
                        }
                        else
                        {
                            Pauza("Nemate pravo pristupa.");
                        }
                        break;

                    case "0":
                        kraj = true;
                        Console.WriteLine("Uspešno ste se odjavili.");
                        break;

                    default:
                        Pauza("Nevalidna opcija.");
                        break;
                }
            }
        }

        // ==================== PROFIL ====================
        private void PrikaziProfil()
        {
            Console.Clear();
            Console.WriteLine("\n===== PROFIL =====");
            Console.WriteLine($"Ime i prezime: {_ulogovan.ImePrezime}");
            Console.WriteLine($"Korisničko ime: {_ulogovan.KorisnickoIme}");
            Console.WriteLine($"Uloga: {_ulogovan.Uloga}");
            Pauza("Pritisnite bilo koji taster za nastavak...");
        }

        // ==================== MENADŽERSKE OPCIJE ====================
        private void PrikaziMenadzerMeni()
        {
            bool nazad = false;

            while (!nazad)
            {
                Console.Clear();
                Console.WriteLine("\n===== MENADŽERSKE OPCIJE =====");
                Console.WriteLine("1. Upravljanje biljkama");
                Console.WriteLine("2. Pregled biljaka");
                Console.WriteLine("0. Nazad");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine() ?? "";

                switch (izbor.Trim())
                {
                    case "1":
                        // TODO: Implementirati kada bude servis
                        Pauza("Funkcionalnost u razvoju...");
                        break;

                    case "2":
                        PregledBiljaka();
                        break;

                    case "0":
                        nazad = true;
                        break;

                    default:
                        Pauza("Nevalidna opcija.");
                        break;
                }
            }
        }

        // ==================== PREGLED BILJAKA ====================
        private void PregledBiljaka()
        {
            Console.Clear();
            Console.WriteLine("\n===== PREGLED BILJAKA =====");

            // TODO: Implementirati kada bude spremno
            Console.WriteLine("Lista biljaka će biti ovde...");

            Pauza("Pritisnite bilo koji taster...");
        }

        // ==================== HELPER METODA ====================
        private void Pauza(string poruka)
        {
            Console.WriteLine(poruka);
            Console.WriteLine("\nPritisnite bilo koji taster...");
            Console.ReadKey();
        }
    }
}