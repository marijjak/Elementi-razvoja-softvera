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
            // Baza podataka 
            IBazaPodataka bazaPodataka = new JsonBazaPodataka();

            // Repozitorijumi
            IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(bazaPodataka);
            IBiljkeRepozitorijum biljkeRepo = new BiljkeRepozitorijum(bazaPodataka);

            // Servisi
            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum);
            IBiljkeServis biljkeServis = new BiljkeServis(biljkeRepo);// TODO: Pass necessary dependencies


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
                Console.WriteLine("\n========== DOBRODOŠLI U SISTEM PARFIMERIJE O'SINJEL DE OR ==========");
                Console.WriteLine("1. Prijava");
                Console.WriteLine("2. Registracija");
                Console.Write("Izaberite opciju (1-2): ");
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
                        }
                        break;

                    default:
                        Console.WriteLine("\nNevalidna opcija. Pokušajte ponovo.");
                        break;
                }
            }

            // Uspešna prijava
            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");

            // Glavni meni aplikacije
            OpcijeMeni meni = new OpcijeMeni();
            meni.PrikaziMeni();
        }
    }
}