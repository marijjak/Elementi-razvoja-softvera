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
                else
                {
                    Console.WriteLine("3. Pregeld biljaka");

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
                    case "3":
                        PregledBiljaka();
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
                Console.WriteLine("3. Prilagodi jačinu mirisa biljke");
                Console.WriteLine("0. Nazad");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine() ?? "";

                switch (izbor.Trim())
                {
                    case "1":
                        Console.WriteLine("Dodajte novu biljku:");
                        DodajNovuBiljku();
                        break;

                    case "2":
                        PregledBiljaka();
                        break;
                    case "3":
                        PrilagodiMirisBiljke();
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

       
            var biljke = _biljkeServis.SveBiljke();

            if (biljke == null || !biljke.Any())
            {
                Console.WriteLine("Trenutno nema dostupnih biljaka u bazi.");
            }
            else
            {
                foreach (var b in biljke)
                {
                    Console.WriteLine($"Naziv: {b.OpstiNaziv,-10} | " +
                        $"Latinski: {b.LatinskiNaziv,-10} | " +
                        $"Poreklo: {b.ZemljaPorekla,-12} | " +
                        $"Jacina: {b.JacinaArome:F1} | " +
                        $"Stanje: {b.Stanje}");
                }
            }


            Pauza("Pritisnite bilo koji taster...");
        }

        // ==================== HELPER METODA ====================
        private void Pauza(string poruka)
        {
            Console.WriteLine(poruka);
            Console.WriteLine("\nPritisnite bilo koji taster...");
            Console.ReadKey();
        }
        // ==================== DODAJ BILJKU ====================
        private void DodajNovuBiljku()
        {
            Console.Clear();
            Console.WriteLine("\n===== DODAVANJE NOVE BILJKE =====");

            Console.Write("Opšti naziv: ");
            string naziv = Console.ReadLine() ?? "";

            Console.Write("Latinski naziv: ");
            string latinski = Console.ReadLine() ?? "";

            Console.Write("Zemlja porekla: ");
            string zemlja = Console.ReadLine() ?? "";

            Console.Write("Jačina arome (1.0 - 5.0): ");
            if (!double.TryParse(Console.ReadLine(), out double jacina)) jacina = 1.0;

            // Poziv servisa koji će preko repozitorijuma sačuvati biljku u JSON
            bool uspeh = _biljkeServis.ZasadiNovuBiljku(naziv, latinski, zemlja, jacina);

            if (uspeh)
                Console.WriteLine("\nUspeh: Biljka je zasađena i trajno sačuvana u JSON bazi.");
            else
                Console.WriteLine("\nGreška: Došlo je do problema prilikom čuvanja.");

            Pauza("");
        }
        private void PrilagodiMirisBiljke()
        {
            Console.Clear();
            PregledBiljaka(); // Prvo prikažemo listu da korisnik vidi ID ili naziv

            Console.Write("\nUnesite naziv biljke za promenu: ");
            string naziv = Console.ReadLine() ?? "";

            // Pronađi prvu biljku sa tim nazivom
            var biljka = _biljkeServis.SveBiljke().FirstOrDefault(b => b.OpstiNaziv.Equals(naziv, StringComparison.OrdinalIgnoreCase));

            if (biljka != null)
            {
                Console.Write("Unesite procenat promene (npr. 20 za povećanje, -10 za smanjenje): ");
                if (double.TryParse(Console.ReadLine(), out double procenat))
                {
                    biljka.PromeniJacinuArome(procenat);
                    _biljkeServis.DodajBiljku(biljka); 
                    Console.WriteLine($"\nNova jačina mirisa za {biljka.OpstiNaziv} je: {biljka.JacinaArome:F1}");
                }
            }
            else
            {
                Console.WriteLine("Biljka nije pronađena.");
            }
            Pauza("");
        }
    }
}