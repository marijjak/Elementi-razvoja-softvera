using Domain.Modeli;
using NUnit.Framework;

namespace Tests.Domain
{
    [TestFixture]
    public class ParfemTests
    {
        [Test]
        public void DefaultConstructor_PostavljaPodrazumevaneVrednosti()
        {
            var parfem = new Parfem();

            Assert.That(parfem.Naziv, Is.EqualTo(string.Empty));
            Assert.That(parfem.TipParfema, Is.EqualTo(string.Empty));
            Assert.That(parfem.BiljkaIds, Is.Not.Null);
        }
    }
}