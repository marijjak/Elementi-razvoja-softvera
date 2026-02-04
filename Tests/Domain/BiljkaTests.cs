using Domain.Modeli;
using NUnit.Framework;

namespace Tests.Domain
{
    [TestFixture]
    public class BiljkaTests
    {
        [Test]
        public void Konstruktor_PostavljaPolja()
        {
            var biljka = new Biljka("Lavanda", "Lavandula", 4.2, "Francuska");

            Assert.That(biljka.OpstiNaziv, Is.EqualTo("Lavanda"));
            Assert.That(biljka.LatinskiNaziv, Is.EqualTo("Lavandula"));
            Assert.That(biljka.JacinaArome, Is.EqualTo(4.2));
            Assert.That(biljka.ZemljaPorekla, Is.EqualTo("Francuska"));
        }

        [Test]
        public void DefaultConstructor_PodrazumevaneVrednosti()
        {
            var biljka = new Biljka();

            Assert.That(biljka.OpstiNaziv, Is.EqualTo(string.Empty));
            Assert.That(biljka.LatinskiNaziv, Is.EqualTo(string.Empty));
            Assert.That(biljka.ZemljaPorekla, Is.EqualTo(string.Empty));
        }
    }
}