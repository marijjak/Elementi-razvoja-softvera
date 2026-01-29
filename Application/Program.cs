using Database.BazaPodataka;
using Database.Repozitorijumi;
using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Presentation.Authentifikacija;
using Presentation.Meni;
using Services;
using Services.AutenftikacioniServisi;

namespace Loger_Bloger
{
    public class Program
    {
        public static void Main()
        {
            // Baza podataka - JSON implementacija
            IBazaPodataka bazaPodataka = new JsonBazaPodataka();

            // Repozitorijumi
            IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(bazaPodataka);
            IBiljkeRepozitorijum biljkeRepozitorijum = new BiljkeRepozitorijum(bazaPodataka);
            IDogadjajiRepozitorijum dogadjajiRepozitorijum = new DogadjajiRepozitorijum(bazaPodataka);
            IPerfumeRepository perfumeRepo = new PerfumeRepository(bazaPodataka);
            IAmbalazaRepozitorijum ambalazaRepozitorijum = new AmbalazaRepozitorijum(bazaPodataka);
            ISkladisteRepozitorijum skladisteRepozitorijum = new SkladisteRepozitorijum(bazaPodataka);

            if (!bazaPodataka.Tabele.Skladista.Any())
            {
                Guid pocetnoSkladisteId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301");
                var pocetnoSkladiste = new Skladiste
                {
                    Id = pocetnoSkladisteId,
                    Naziv = "Glavno skladište",
                    MaxBrojAmbalaza = 100,
                    TrenutniBrojAmbalaza = 0
                };
                bazaPodataka.Tabele.Skladista.Add(pocetnoSkladiste);
                bazaPodataka.SacuvajPromene();
                Console.WriteLine($"Kreirano početno skladište: {pocetnoSkladiste.Naziv} (ID: {pocetnoSkladiste.Id}).");
            }

            // TODO: Dodati ostale repozitorijume 
            if (!biljkeRepozitorijum.ObrisiPrazne())
            {
                Console.WriteLine("Upozorenje: čišćenje praznih biljaka nije uspelo.");
            }
           

            // Servisi
            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum);
            IDogadjajiServis dogadjajiServis = new DogadjajiServis(dogadjajiRepozitorijum);
            IBiljkeServis biljkeServis = new BiljkeServis(biljkeRepozitorijum, dogadjajiServis);
            IPreradaServis preradaServis = new PreradaServis(biljkeServis, perfumeRepo);
            ISkladisteServis skladisteServis = new SkladisteServis(skladisteRepozitorijum);
            IAmbalazaServis ambalazaServis = new AmbalazaServis(ambalazaRepozitorijum, dogadjajiServis, perfumeRepo, skladisteServis);

            MagacinskiCentarServis magacinServis = new MagacinskiCentarServis(ambalazaRepozitorijum, dogadjajiServis);
            DistributivniCentarServis distributivniCentarServis = new DistributivniCentarServis(ambalazaRepozitorijum, dogadjajiServis);
            ISkladisteProvider skladisteProvider = new SkladisteProvider(magacinServis, distributivniCentarServis);
          
            if (korisniciRepozitorijum.SviKorisnici().Count() == 0)
            {
              
                Korisnik menadzer = new Korisnik(
                    "menadzer",
                    "menadzer123",
                    "Petar Petrović",
                    TipKorisnika.MenadzerProdaje
                );

                Korisnik prodavac = new Korisnik(
                    "prodavac",
                    "prodavac123",
                    "Marko Marković",
                    TipKorisnika.Prodavac
                );

                korisniciRepozitorijum.DodajKorisnika(menadzer);
                korisniciRepozitorijum.DodajKorisnika(prodavac);

                Console.WriteLine("Kreirani početni korisnici:");
                Console.WriteLine("1. Menadžer prodaje - korisničko ime: 'menadzer', lozinka: 'menadzer123'");
                Console.WriteLine("2. Prodavac - korisničko ime: 'prodavac', lozinka: 'prodavac123'");
                Console.WriteLine();
            }

           
            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen = new Korisnik();

        
            bool uspesnoPrijavljen = false;
            while (!uspesnoPrijavljen)
            {
                Console.Clear();
                Console.WriteLine("\n===== DOBRODOŠLI U SISTEM PARFIMERIJE O'SINJEL DE OR =====");
                Console.WriteLine("1. Prijava");
                Console.WriteLine("2. Registracija");
                Console.WriteLine("0. Izlaz");
                Console.Write("Izbor: ");

                string opcija = Console.ReadLine() ?? "";

                switch (opcija.Trim())
                {
                    case "1":
                      
                        if (am.TryLogin(out prijavljen))
                        {
                            uspesnoPrijavljen = true;
                        }
                        else
                        {
                            Console.WriteLine("\nPogrešno korisničko ime ili lozinka. Pokušajte ponovo.");
                            Console.WriteLine("Pritisnite bilo koji taster...");
                            Console.ReadKey();
                        }
                        break;

                    case "2":
                       
                        if (am.TryRegister(out prijavljen))
                        {
                            Console.WriteLine($"\nUspešno ste se registrovali kao: {prijavljen.ImePrezime}");
                            uspesnoPrijavljen = true;
                        }
                        else
                        {
                            Console.WriteLine("\nRegistracija nije uspela. Korisničko ime možda već postoji ili su podaci nevalidni.");
                            Console.WriteLine("Pritisnite bilo koji taster...");
                            Console.ReadKey();
                        }
                        break;

                    case "0":
                        Console.WriteLine("Izlaz iz sistema...");
                        return;

                    default:
                        Console.WriteLine("\nNevalidna opcija. Pokušajte ponovo.");
                        Console.WriteLine("Pritisnite bilo koji taster...");
                        Console.ReadKey();
                        break;
                }
            }

            Console.Clear();
            Console.WriteLine($"\nUspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");
            Console.WriteLine("Pritisnite bilo koji taster za nastavak...");
            Console.ReadKey();

      
            OpcijeMeni meni = new OpcijeMeni(
                autentifikacijaServis,
                biljkeServis, 
                dogadjajiServis,
                preradaServis,
                perfumeRepo,
                ambalazaServis,
                magacinServis,
                distributivniCentarServis,
                prijavljen,
                skladisteProvider
            );
            meni.PrikaziGlavniMeni();
        }
    }
}