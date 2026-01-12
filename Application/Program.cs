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


            // TODO: Dodati ostale repozitorijume 
            biljkeRepozitorijum.ObrisiPrazne();

            // Servisi
            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum);
            IDogadjajiServis dogadjajiServis = new DogadjajiServis(dogadjajiRepozitorijum);
            IBiljkeServis biljkeServis = new BiljkeServis(biljkeRepozitorijum, dogadjajiServis);
            // TODO: Dodati ostale servise 

            // Ako nema nijednog korisnika u sistemu, dodati dva nova
            if (korisniciRepozitorijum.SviKorisnici().Count() == 0)
            {
                // Dodavanje početnih korisnika prema specifikaciji
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

            // Prezentacioni sloj - Autentifikacija
            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen = new Korisnik();

            // Petlja za prijavu/registraciju
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
                        // Pokušaj prijave
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
                        // Pokušaj registracije
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

            // Uspešna prijava
            Console.Clear();
            Console.WriteLine($"\nUspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");
            Console.WriteLine("Pritisnite bilo koji taster za nastavak...");
            Console.ReadKey();

            // Glavni meni aplikacije
            OpcijeMeni meni = new OpcijeMeni(
                autentifikacijaServis,
                biljkeServis, // TODO: Dodati IBiljkeServis kada bude implementiran
                dogadjajiServis,
                prijavljen
            );
            meni.PrikaziGlavniMeni();
        }
    }
}