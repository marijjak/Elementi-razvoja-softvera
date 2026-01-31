using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Moq;
using NUnit.Framework;
using Services;

namespace Tests.Services
{
    [TestFixture]
    public class DogadjajiServisTests
    {
        private Mock<IDogadjajiRepozitorijum> _mockRepo = null;
        private DogadjajiServis _servis = null;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IDogadjajiRepozitorijum>();
            _servis = new DogadjajiServis(_mockRepo.Object);
        }

        [Test]
        public void Zabelezi_PozivaDodajMetoduRepozitorijuma()
        {
       
            var opis = "Testna poruka";
            var tip = TipEvidencije.INFO;
            var entitetId = Guid.NewGuid();
            _mockRepo.Setup(r => r.Dodaj(It.IsAny<Dogadjaj>())).Returns(true);

   
            var rezultat = _servis.Zabelezi(opis, tip, entitetId);
            Assert.That(rezultat, Is.True);

      
            _mockRepo.Verify(r => r.Dodaj(It.Is<Dogadjaj>(d =>
                d.Opis == opis &&
                d.Tip == tip &&
                d.EntitetId == entitetId)),
            Times.Once);
        }
    }
}
