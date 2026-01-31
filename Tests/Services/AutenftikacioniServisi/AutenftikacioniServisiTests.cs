using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Moq;
using NUnit.Framework;
using Services.AutenftikacioniServisi;

namespace Tests.Services.AutenftikacioniServisi
{
    [TestFixture]
    public class AutentifikacioniServisTests
    {
        [Test]
        public void Prijava_ValidniPodaci_VracaUspesnuPrijavu()
        {
            var repoMock = new Mock<IKorisniciRepozitorijum>();
            var korisnik = new Korisnik("pera", "tajna", "Pera Peric", TipKorisnika.MenadzerProdaje) { Id = 1 };

            repoMock.Setup(r => r.PronadjiKorisnikaPoKorisnickomImenu("pera")).Returns(korisnik);

            var servis = new AutentifikacioniServis(repoMock.Object);

            var (uspeh, rezultat) = servis.Prijava("pera", "tajna");

            Assert.That(uspeh, Is.True);
            Assert.That(rezultat.KorisnickoIme, Is.EqualTo("pera"));
        }

        [Test]
        public void Registracija_NevalidniPodaci_VracaNeuspeh()
        {
            var repoMock = new Mock<IKorisniciRepozitorijum>();
            var servis = new AutentifikacioniServis(repoMock.Object);

            var (uspeh, rezultat) = servis.Registracija(new Korisnik("", "", "", TipKorisnika.Prodavac));

            Assert.That(uspeh, Is.False);
            Assert.That(rezultat.Id, Is.EqualTo(0));
        }
    }
}