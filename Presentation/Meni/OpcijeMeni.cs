using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Services;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly IAutentifikacijaServis _authServis;
        private readonly IBiljkeServis _biljkeServis;
        private readonly IDogadjajiServis _dogadjajiServis;
        private readonly IPreradaServis _preradaServis;
        private readonly IPerfumeRepository _parfemRepo;
        private readonly IAmbalazaServis _ambalazaServis;

        private Korisnik _ulogovan;

        public OpcijeMeni(
            IAutentifikacijaServis authServis,
            IBiljkeServis biljkeServis, IDogadjajiServis dogadjajiServis, IPreradaServis preradaServis,
            IPerfumeRepository parfemRepo,
            IAmbalazaServis ambalazaServis,
            Korisnik ulogovan)
        {
            _authServis = authServis;
            _biljkeServis = biljkeServis;
            _dogadjajiServis = dogadjajiServis;
            _preradaServis = preradaServis;
            _parfemRepo = parfemRepo;
            _ambalazaServis = ambalazaServis;
            _ulogovan = ulogovan;
        }

        // ==================== GLAVNI MENI  ====================
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
                Console.WriteLine("4. Označi biljku kao ubranu");
                Console.WriteLine("5. Pregled važnih događaja");
                Console.WriteLine("6. Prerada biljaka u parfeme");
                Console.WriteLine("7. Upravljanje ambalažom");
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

                    case "4":
                        OznaciBiljkuKaoUbranu();
                        break;

                    case "5":
                        PregledDogadjaja();
                        break;

                    case "6":
                        PreradaMeni();
                        break;
                    case "7":
                        UpravljanjeAmbalazom();
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

            double jacina;
            while (true)
            {
                Console.Write("Jačina arome (1.0 - 5.0): ");
                string unos = Console.ReadLine() ?? "";

                // Proveravamo da li je unos broj i da li je u dozvoljenim granicama
                if (double.TryParse(unos, out jacina) && jacina >= 1.0 && jacina <= 5.0)
                {
                    break; // Unos je ispravan, izlazimo iz petlje
                }

                Console.WriteLine("Greška: Neispravan unos. Jačina mora biti broj između 1.0 i 5.0.");
            }


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
                    _biljkeServis.PromeniJacinuUljaProcentualno(naziv, procenat);
                    Console.WriteLine($"\nNova jačina mirisa za {biljka.OpstiNaziv} je: {biljka.JacinaArome:F1}");
                }
            }
            else
            {
                Console.WriteLine("Biljka nije pronađena.");
            }
            Pauza("");
        }

        // ==================== OZNAČI BILJKU KAO UBRANU ====================
        private void OznaciBiljkuKaoUbranu()
        {
            Console.Clear();
            Console.WriteLine("\n Označavanje biljke kao ubrane. \n");

            // Prikažemo biljke da korisnik vidi stanje
            var biljke = _biljkeServis.SveBiljke();

            if (biljke == null || !biljke.Any())
            {
                Pauza("Nema biljaka u sistemu.");
                return;
            }

            foreach (var b in biljke)
            {
                Console.WriteLine($"ID: {b.Id} | Naziv: {b.OpstiNaziv} | Stanje: {b.Stanje}");
            }

            Console.Write("\nUnesite ID biljke koju želite da označite kao ubranu: ");
            string unos = Console.ReadLine() ?? "";

            if (!Guid.TryParse(unos, out Guid id))
            {
                Pauza("Neispravan ID.");
                return;
            }

            try
            {
                bool uspeh = _biljkeServis.OznaciBiljkuKaoUbranu(id);

                if (uspeh)
                    Pauza("Biljka je uspešno označena kao ubrana.");
                else
                    Pauza("Biljka nije pronađena.");
            }
            catch (Exception ex)
            {
                Pauza($"Greška: {ex.Message}");
            }
        }

        // ==================== PREGLED DOGAĐAJA ====================
        private void PregledDogadjaja()
        {
            Console.Clear();
            Console.WriteLine("\n===== VAŽNI DOGAĐAJI =====\n");

            var dogadjaji = _dogadjajiServis.SviDogadjaji();

            if (!dogadjaji.Any())
            {
                Pauza("Nema zabeleženih događaja.");
                return;
            }

            foreach (var d in dogadjaji)
            {
                Console.WriteLine(
                    $"{d.Vreme:dd.MM.yyyy HH:mm} | {d.Tip} | {d.Opis}"
                );
            }

            Pauza("");
        }

        private void PrikaziSveParfeme()
        {
            Console.Clear();
            Console.WriteLine("\n===== KATALOG PARFEMA =====");

            var parfemi = _parfemRepo.Svi();

            if (!parfemi.Any())
            {
                Pauza("Nema parfema na stanju.");
                return;
            }

            foreach (var p in parfemi)
            {
                Console.WriteLine($"{p.Naziv,-12} | {p.TipParfema,-10} | {p.ZapreminaBociceMl}ml | Na stanju: {p.KolicinaNaStanju}");
            }

            Pauza("");
        }
        private void KreirajParfemMeni()
        {
            Console.Clear();
            Console.WriteLine("\n===== NOVI PARFEM =====");

            Console.Write("Naziv parfema: ");
            string naziv = Console.ReadLine() ?? "";

            Console.Write("Tip (Eau de Parfum / Eau de Toilette): ");
            string tip = Console.ReadLine() ?? "";

            Console.Write("Broj bočica: ");
            int br = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Zapremina bočice (150 / 250): ");
            int zap = int.Parse(Console.ReadLine() ?? "0");

            try
            {
                var parfem = _preradaServis.NapraviParfem(naziv, br, zap, tip);
                Pauza($"Parfem uspešno napravljen! Serijski broj: {parfem.SerijskiBroj}");
            }
            catch (Exception ex)
            {
                Pauza("Greška: " + ex.Message);
            }

        }

        private void PreradaMeni()
        {
            bool nazad = false;

            while (!nazad)
            {
                Console.Clear();
                Console.WriteLine("\n===== PRERADA PARFEMA =====");
                Console.WriteLine("1. Pregled svih parfema (katalog)");
                Console.WriteLine("2. Napravi novi parfem");
                Console.WriteLine("0. Nazad");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine() ?? "";

                switch (izbor)
                {
                    case "1":
                        PrikaziSveParfeme();
                        break;
                    case "2":
                        KreirajParfemMeni();
                        break;
                    case "0":
                        nazad = true;
                        break;
                }
            }
        }

        private void UpravljanjeAmbalazom()
        {
            bool nazad = false;

            while (!nazad)
            {
                Console.Clear();
                Console.WriteLine("\n===== UPRAVLJANJE AMBALAŽOM =====");
                Console.WriteLine("1. Kreiraj ambalažu");
                Console.WriteLine("2. Pregled ambalaža");
                Console.WriteLine("0. Nazad");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine() ?? "";

                switch (izbor.Trim())
                {
                    case "1":
                        KreirajAmbalazu();
                        break;
                    case "2":
                        PregledAmbalaza();
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

        private void KreirajAmbalazu()
        {
            Console.Clear();
            Console.WriteLine("\n===== KREIRANJE AMBALAŽE =====");

            Console.Write("Naziv ambalaže: ");
            string naziv = Console.ReadLine() ?? "";

            Console.Write("Adresa pošiljaoca: ");
            string adresa = Console.ReadLine() ?? "";

            Guid skladisteId;
            while (true)
            {
                Console.Write("ID skladišta (GUID): ");
                string unosSkladiste = Console.ReadLine() ?? "";
                if (Guid.TryParse(unosSkladiste, out skladisteId))
                {
                    break;
                }

                Console.WriteLine("Neispravan GUID format za skladište.");
            }

            Console.Write("Unesite ID-eve parfema (zarezom odvojeno) ili ostavite prazno: ");
            string parfemiUnos = Console.ReadLine() ?? "";
            List<Guid> parfemIds = new List<Guid>();

            if (!string.IsNullOrWhiteSpace(parfemiUnos))
            {
                var delovi = parfemiUnos.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var deo in delovi)
                {
                    if (Guid.TryParse(deo, out Guid parfemId))
                    {
                        parfemIds.Add(parfemId);
                    }
                }
            }

            try
            {
                var ambalaza = _ambalazaServis.KreirajAmbalazu(naziv, adresa, skladisteId, parfemIds);
                Pauza($"Ambalaža kreirana! ID: {ambalaza.Id}");
            }
            catch (Exception ex)
            {
                Pauza($"Greška: {ex.Message}");
            }
        }

        private void PregledAmbalaza()
        {
            Console.Clear();
            Console.WriteLine("\n===== PREGLED AMBALAŽA =====");

            var ambalaze = _ambalazaServis.SveAmbalaze().ToList();

            if (!ambalaze.Any())
            {
                Pauza("Trenutno nema kreiranih ambalaža.");
                return;
            }

            foreach (var ambalaza in ambalaze)
            {
                Console.WriteLine($"ID: {ambalaza.Id} | Naziv: {ambalaza.Naziv} | Adresa: {ambalaza.AdresaPosiljaoca} | " +
                                  $"Skladište: {ambalaza.SkladisteId} | Status: {ambalaza.Status} | Parfemi: {ambalaza.ParfemIds.Count}");
            }

            Pauza("");
        }

    }

}
