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
            // Baza
            IBazaPodataka bazaPodataka = new JsonBazaPodataka();

            // Repozitorijumi
            IKorisniciRepozitorijum korisniciRepo = new KorisniciRepozitorijum(bazaPodataka);
            IBiljkeRepozitorijum biljkeRepo = new BiljkeRepozitorijum(bazaPodataka);

            // Servisi
            IAutentifikacijaServis authServis = new AutentifikacioniServis(korisniciRepo);
            IBiljkeServis biljkeServis = new BiljkeServis(biljkeRepo);

            // Početni korisnici
            if (!korisniciRepo.SviKorisnici().Any())
            {
                korisniciRepo.DodajKorisnika(new Korisnik(
                    "menadzer",
                    "menadzer123",
                    "Petar Petrović",
                    TipKorisnika.MenadzerProdaje));

                korisniciRepo.DodajKorisnika(new Korisnik(
                    "prodavac",
                    "prodavac123",
                    "Marko Marković",
                    TipKorisnika.Prodavac));
            }
            // START MENIJA
            OpcijeMeni meni = new OpcijeMeni(authServis, biljkeServis);
            meni.Pokreni();
        }
    }
}
