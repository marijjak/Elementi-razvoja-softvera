using Database.BazaPodataka;
using Database.Repozitorijumi;
using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Konstante;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Presentation.Authentifikacija;
using Presentation.Meni;
using Services;
using Services.AutenftikacioniServisi;
using System;
using System.Linq;

namespace Loger_Bloger
{
    public class Program
    {
        public static void Main()
        {
            
            IBazaPodataka bazaPodataka = new JsonBazaPodataka();
            ILoggerServis loggerServis = new LoggerServis();
            loggerServis.InicijalizujLogFajl();

           
            IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(bazaPodataka);
            IBiljkeRepozitorijum biljkeRepozitorijum = new BiljkeRepozitorijum(bazaPodataka);
            IDogadjajiRepozitorijum dogadjajiRepozitorijum = new DogadjajiRepozitorijum(bazaPodataka);
            IPerfumeRepository perfumeRepo = new PerfumeRepository(bazaPodataka);
            IAmbalazaRepozitorijum ambalazaRepozitorijum = new AmbalazaRepozitorijum(bazaPodataka);
            ISkladisteRepozitorijum skladisteRepozitorijum = new SkladisteRepozitorijum(bazaPodataka);
            IFiskalniRacunRepozitorijum prodajaRepo = new FiskalniRacunRepozitorijum(bazaPodataka);

            
            if (!bazaPodataka.Tabele.Skladista.Any())
            {
                Guid pocetnoSkladisteId = Guid.Parse(KONSTANTE.DefaultSkladisteId);
                var pocetnoSkladiste = new Skladiste
                {
                    Id = pocetnoSkladisteId,
                    Naziv = "Glavno skladište",
                    Lokacija = "Paris",
                    MaxBrojAmbalaza = 100,
                    TrenutniBrojAmbalaza = 0
                };
                bazaPodataka.Tabele.Skladista.Add(pocetnoSkladiste);
                bazaPodataka.SacuvajPromene();
                Console.WriteLine($"Kreirano početno skladište: {pocetnoSkladiste.Naziv} (ID: {pocetnoSkladiste.Id}).");
            }

            if (!biljkeRepozitorijum.ObrisiPrazne())
            {
                Console.WriteLine("Upozorenje: Čišćenje praznih biljaka nije uspelo.");
            }

           
            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum);
            IDogadjajiServis dogadjajiServis = new DogadjajiServis(dogadjajiRepozitorijum);

            
            IBiljkeServis biljkeServis = new BiljkeServis(biljkeRepozitorijum, dogadjajiServis, loggerServis);
            IPreradaServis preradaServis = new PreradaServis(biljkeServis, perfumeRepo, biljkeRepozitorijum, loggerServis);

            ISkladisteServis skladisteServis = new SkladisteServis(skladisteRepozitorijum);

            IAmbalazaServis ambalazaServis = new AmbalazaServis(
                ambalazaRepozitorijum,
                dogadjajiServis,
                perfumeRepo,
                skladisteServis,
                loggerServis,
                bazaPodataka
            );
            ISkladisniLogistickiServis magacinServis = new MagacinskiCentarServis(ambalazaRepozitorijum, dogadjajiServis);
            ISkladisniLogistickiServis distributivniCentarServis = new DistributivniCentarServis(ambalazaRepozitorijum, dogadjajiServis);

            IProdajaServis prodajaServis = new ProdajaServis(
                prodajaRepo,
                loggerServis,
                dogadjajiServis,
                (ISkladisteServis)distributivniCentarServis,
                (ISkladisteServis)magacinServis,
                ambalazaRepozitorijum
            );

            ISkladisteProvider skladisteProvider = new SkladisteProvider(magacinServis, distributivniCentarServis);

            
            if (!korisniciRepozitorijum.SviKorisnici().Any())
            {
                Korisnik menadzer = new Korisnik("menadzer", "menadzer123", "Petar Petrović", TipKorisnika.MenadzerProdaje);
                Korisnik prodavac = new Korisnik("prodavac", "prodavac123", "Marko Marković", TipKorisnika.Prodavac);

                korisniciRepozitorijum.DodajKorisnika(menadzer);
                korisniciRepozitorijum.DodajKorisnika(prodavac);

                Console.WriteLine("Kreirani početni korisnici (menadzer/menadzer123, prodavac/prodavac123).");
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
                        if (am.TryLogin(out prijavljen)) uspesnoPrijavljen = true;
                        else { Console.WriteLine("\nPogrešna prijava."); Console.ReadKey(); }
                        break;
                    case "2":
                        if (am.TryRegister(out prijavljen)) uspesnoPrijavljen = true;
                        else { Console.WriteLine("\nRegistracija nije uspela."); Console.ReadKey(); }
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nNevalidna opcija.");
                        Console.ReadKey();
                        break;
                }
            }

           
            OpcijeMeni meni = new OpcijeMeni(
                autentifikacijaServis,
                biljkeServis,
                dogadjajiServis,
                preradaServis,
                perfumeRepo,
                ambalazaServis,
                (MagacinskiCentarServis)magacinServis,
                (DistributivniCentarServis)distributivniCentarServis,
                prijavljen!,
                skladisteProvider,
                prodajaServis
            );

            meni.PrikaziGlavniMeni();
        }
    }
}