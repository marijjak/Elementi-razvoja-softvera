using Domain.Enumeracije;
using Domain.Modeli;
using NUnit.Framework;

namespace Tests.Domain
{
    [TestFixture]
    public class KorisnikTests
    {
        [Test]
        public void Constructor_PostavljaPolja()
        {
            var korisnik = new Korisnik("ana", "sifra", "Ana Anic", TipKorisnika.MenadzerProdaje);

            Assert.That(korisnik.KorisnickoIme, Is.EqualTo("ana"));
            Assert.That(korisnik.Lozinka, Is.EqualTo("sifra"));
            Assert.That(korisnik.ImePrezime, Is.EqualTo("Ana Anic"));
            Assert.That(korisnik.Uloga, Is.EqualTo(TipKorisnika.MenadzerProdaje));
        }

        [Test]
        public void DefaultConstructor_PodrazumevaneVrednosti()
        {
            var korisnik = new Korisnik();

            Assert.That(korisnik.Id, Is.EqualTo(0));
            Assert.That(korisnik.KorisnickoIme, Is.EqualTo(string.Empty));
            Assert.That(korisnik.Lozinka, Is.EqualTo(string.Empty));
            Assert.That(korisnik.ImePrezime, Is.EqualTo(string.Empty));
        }
    }
}