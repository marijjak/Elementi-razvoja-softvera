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
        private readonly IMagacinskiCentarServis _magacinServis;
        private readonly IDistributivniCentarServis _distributivniCentarServis;
        private readonly ISkladisteProvider _skladisteProvider;
        private readonly IProdajaServis _prodajaServis;


        private Korisnik _ulogovan;

        public OpcijeMeni(
            IAutentifikacijaServis authServis,
            IBiljkeServis biljkeServis, IDogadjajiServis dogadjajiServis, IPreradaServis preradaServis,
            IPerfumeRepository parfemRepo,
            IAmbalazaServis ambalazaServis,
            IMagacinskiCentarServis magacinServis,
            IDistributivniCentarServis distributivniCentarServis,
            Korisnik ulogovan,
            ISkladisteProvider skladisteProvider,
            IProdajaServis prodajaServis)
        {
            _prodajaServis = prodajaServis;
            _authServis = authServis;
            _biljkeServis = biljkeServis;
            _dogadjajiServis = dogadjajiServis;
            _preradaServis = preradaServis;
            _parfemRepo = parfemRepo;
            _ambalazaServis = ambalazaServis;
            _magacinServis = magacinServis;
            _distributivniCentarServis = distributivniCentarServis;
            _ulogovan = ulogovan;
            _skladisteProvider = skladisteProvider;
        }
        private async Task ProcesuirajLogistiku()
        {
            Console.Clear();
            Console.WriteLine($"=== PROCESUIRANJE ISPORUKE ({_ulogovan.Uloga}) ===");

            var ambalaze = _ambalazaServis.SveAmbalaze()
                .Where(a => a.Status == StatusAmbalaze.Spakovana)
                .ToList();

            if (!ambalaze.Any())
            {
                Pauza("Nema spremnih ambalaža za slanje.");
                return;
            }

            foreach (var a in ambalaze)
                Console.WriteLine($"ID: {a.Id} | Naziv: {a.Naziv}");

            Console.Write("\nUnesite ID ambalaže za procesuiranje: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                try
                {
       
                    var adekvatanServis = _skladisteProvider.GetServisPoUlozi(_ulogovan.Uloga.ToString());

                    Console.WriteLine("Slanje u toku...");
                    bool uspeh = await adekvatanServis.ProcesuirajIsporukuAsync(id);

                    if (uspeh) Console.WriteLine("Isporuka uspešno procesuirana!");
                    else Console.WriteLine("Greška prilikom isporuke.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška: {ex.Message}");
                }
            }
            Pauza("");
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
                    Console.WriteLine("2. Pregled biljaka");
                }

                if (_ulogovan.Uloga == TipKorisnika.Prodavac)
                {
                    Console.WriteLine("3. Standardna isporuka (Magacinski centar)");
                    Console.WriteLine("4. Interaktivni prodajni katalog");
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
                            PregledBiljaka();
                        }
                        break;
                    case "3":
                        if (_ulogovan.Uloga == TipKorisnika.Prodavac)
                        {
                            ProcesuirajLogistiku().Wait();
                        }
                        break;

                    case "4":
                        if (_ulogovan.Uloga == TipKorisnika.Prodavac)
                        {
                            InteraktivniProdajniKatalog();
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
                Console.WriteLine("3. Prilagodi jačinu mirisa biljke");
                Console.WriteLine("4. Označi biljku kao ubranu");
                Console.WriteLine("5. Pregled važnih događaja");
                Console.WriteLine("6. Prerada biljaka u parfeme");
                Console.WriteLine("7. Upravljanje ambalažom");
                Console.WriteLine("8. Brza distribucija (Distributivni centar)");
                Console.WriteLine("9. Pregled fiskalnih računa (Dnevni promet)");
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
                    case "8":
                        ProcesuirajLogistiku().Wait();
                        break;

                    case "9":
                        PregledRacunaMeni(); 
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

                if (double.TryParse(unos, out jacina) && jacina >= 1.0 && jacina <= 5.0)
                {
                    break;
                }

                Console.WriteLine("Greška: Neispravan unos. Jačina mora biti broj između 1.0 i 5.0.");
            }


            
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
            PregledBiljaka(); 

            Console.Write("\nUnesite naziv biljke za promenu: ");
            string naziv = Console.ReadLine() ?? "";

   
            var biljka = _biljkeServis.SveBiljke().FirstOrDefault(b => b.OpstiNaziv.Equals(naziv, StringComparison.OrdinalIgnoreCase));

            if (biljka != null)
            {
                Console.Write("Unesite procenat promene (npr. 20 za povećanje, -10 za smanjenje): ");
                if (double.TryParse(Console.ReadLine(), out double procenat))
                {
                    if (_biljkeServis.PromeniJacinuUljaProcentualno(naziv, procenat))
                    {
                        Console.WriteLine($"\nNova jačina mirisa za {biljka.OpstiNaziv} je: {biljka.JacinaArome:F1}");
                    }
                    else
                    {
                        Console.WriteLine("\nGreška: Nije moguće promeniti jačinu mirisa.");
                    }
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
        private void InteraktivniProdajniKatalog()
        {
            Console.Clear();
            Console.WriteLine("\n===== INTERAKTIVNI PRODAJNI KATALOG =====");

            var dostupniParfemi = _parfemRepo.Svi()
                .Where(p => p.KolicinaNaStanju > 0)
                .ToList();

            if (!dostupniParfemi.Any())
            {
                Pauza("Nema dostupnih parfema na stanju.");
                return;
            }

            foreach (var parfem in dostupniParfemi)
            {
                Console.WriteLine(
                    $"ID: {parfem.Id} | {parfem.Naziv,-12} | {parfem.TipParfema,-12} | {parfem.ZapreminaBociceMl}ml | Na stanju: {parfem.KolicinaNaStanju}"
                );
            }

            Console.Write("\nUnesite ID parfema (ili Enter za izlaz): ");
            var unosId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(unosId))
            {
                return;
            }

            if (!Guid.TryParse(unosId, out var parfemId))
            {
                Pauza("Nevalidan ID parfema.");
                return;
            }

            var odabrani = dostupniParfemi.FirstOrDefault(p => p.Id == parfemId);
            if (odabrani == null)
            {
                Pauza("Parfem nije pronađen ili nije dostupan.");
                return;
            }

            Console.Write("Unesite količinu: ");
            if (!int.TryParse(Console.ReadLine(), out var kolicina) || kolicina <= 0)
            {
                Pauza("Nevalidna količina.");
                return;
            }

            if (kolicina > odabrani.KolicinaNaStanju)
            {
                Pauza("Tražena količina premašuje stanje na zalihama.");
                return;
            }

            var novaKolicina = odabrani.KolicinaNaStanju - kolicina;
            if (!_parfemRepo.AzurirajKolicinu(parfemId, novaKolicina))
            {
                Pauza("Nije moguće ažurirati stanje parfema.");
                return;
            }
            _dogadjajiServis.Zabelezi($"Prodavac {_ulogovan.ImePrezime} je prodao {kolicina}x {odabrani.Naziv}", TipEvidencije.INFO);
            PrikaziFiskalniRacun(odabrani, kolicina).Wait();
        }

        private async Task PrikaziFiskalniRacun(Parfem parfem, int kolicina)
        {
            decimal cenaPoBocici = IzracunajCenuPoBocici(parfem);
            decimal ukupno = cenaPoBocici * kolicina;

            var noviRacun = new FiskalniRacun
            {
                Id = Guid.NewGuid(),
                DatumIzdavanja = DateTime.Now,
                ImeProdavca = _ulogovan.ImePrezime,
                UkupanIznos = ukupno,
                TipProdaje = TipProdaje.Maloprodaja,
                NacinPlacanja = NacinPlacanja.Gotovina,
                Stavke = new Dictionary<Guid, int>()
            };
            noviRacun.Stavke.Add(parfem.Id, kolicina);

            if (await _prodajaServis.DodajNoviRacunAsync(noviRacun))
            {
                Console.WriteLine("USPEH: Račun je uspešno sačuvan u bazu!");
            }
            else
            {
                Console.WriteLine("GREŠKA: Došlo je do problema pri čuvanju računa.");
            }

            Console.Clear();
            Console.WriteLine("\n===== FISKALNI RAČUN =====");
            Console.WriteLine($"Datum: {DateTime.Now:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Parfem: {parfem.Naziv} ({parfem.TipParfema})");
            Console.WriteLine($"Zapremina: {parfem.ZapreminaBociceMl}ml");
            Console.WriteLine($"Količina: {kolicina}");
            Console.WriteLine($"Jedinična cena: {cenaPoBocici:N2} RSD");
            Console.WriteLine($"Ukupan iznos: {ukupno:N2} RSD");
            Pauza("Pritisnite bilo koji taster za nastavak...");
        }

        private static decimal IzracunajCenuPoBocici(Parfem parfem)
        {
            const decimal cenaPoMl = 10m;
            return parfem.ZapreminaBociceMl * cenaPoMl;
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
                Console.WriteLine("3. Dodaj parfeme u ambalažu");
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
                    case "3":
                        DodajParfemeUAmbalazu();
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
                Console.Write("ID skladišta (GUID) ( Primer: 3f2504e0-4f89-11d3-9a0c-0305e82c3301) : ");
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
                if (ambalaza == null)
                {
                    Pauza("Greška pri kreiranju ambalaže.");
                    return;
                }

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

        private void DodajParfemeUAmbalazu()
        {
            Console.Clear();
            Console.WriteLine("\n===== DODAVANJE PARFEMA U AMBALAŽU =====");

            var ambalaze = _ambalazaServis.SveAmbalaze().ToList();
            if (!ambalaze.Any())
            {
                Pauza("Nema dostupnih ambalaža.");
                return;
            }

            Console.WriteLine("\nDostupne ambalaže:");
            foreach (var ambalaza in ambalaze)
            {
                Console.WriteLine($"ID: {ambalaza.Id} | Naziv: {ambalaza.Naziv} | Parfemi: {ambalaza.ParfemIds.Count}");
            }

            Console.Write("\nUnesite ID ambalaže: ");
            string ambalazaUnos = Console.ReadLine() ?? "";
            if (!Guid.TryParse(ambalazaUnos, out Guid ambalazaId))
            {
                Pauza("Neispravan ID ambalaže.");
                return;
            }

          
            var dostupniParfemi = _parfemRepo.Svi()
              .Where(p => p.KolicinaNaStanju > 0)
                .ToList();

            if (!dostupniParfemi.Any())
            {
                Pauza("Nema dostupnih parfema koji nisu već u ambalaži.");
                return;
            }

            Console.WriteLine("\nDostupni parfemi:");
            foreach (var parfem in dostupniParfemi)
            {
                Console.WriteLine($"ID: {parfem.Id} | Naziv: {parfem.Naziv} | Tip: {parfem.TipParfema}");
            }

            Console.Write("\nUnesite ID-eve parfema (zarezom odvojeno): ");
            string parfemiUnos = Console.ReadLine() ?? "";
            var parfemIds = parfemiUnos
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(deo => Guid.TryParse(deo, out Guid parfemId) ? parfemId : Guid.Empty)
                .Where(id => id != Guid.Empty)
                .ToList();

            try
            {
                var ambalaza = _ambalazaServis.DodajParfemeUAmbalazu(ambalazaId, parfemIds);
                if (ambalaza == null)
                {
                    Pauza("Greška pri dodavanju parfema u ambalažu.");
                    return;
                }
                Pauza($"Uspešno dodati parfemi u ambalažu '{ambalaza.Naziv}'.");
            }
            catch (Exception ex)
            {
                Pauza($"Greška: {ex.Message}");
            }
        }


        private void PregledRacunaMeni()
        {
            Console.Clear();
            Console.WriteLine("\n===== PREGLED FISKALNIH RAČUNA (Dnevni promet) =====");

        

            bool uspeh = _prodajaServis.PokusajDobaviRacuneZaDan(_ulogovan, DateTime.Now, out List<FiskalniRacun> racuni);

           

            if (uspeh && racuni != null && racuni.Count > 0)
            {
                Console.WriteLine($"{"ID Računa",-38} | {"Iznos",-12} | {"Datum i vreme"}");
                Console.WriteLine(new string('-', 75));

                foreach (var r in racuni)
                {
                    Console.WriteLine($"{r.Id,-38} | {r.UkupanIznos,8:N2} RSD | {r.DatumIzdavanja:dd.MM.yyyy HH:mm}");
                }

                Console.WriteLine(new string('-', 75));
                Console.WriteLine($"UKUPAN PROMET: {racuni.Sum(r => r.UkupanIznos):N2} RSD");
            }
            else
            {
                Console.WriteLine("Trenutno nema izdatih računa za današnji dan.");
            }

            Pauza("\nKraj izveštaja.");
        }

    }
}
