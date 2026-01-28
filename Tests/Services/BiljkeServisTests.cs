using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Moq;
using NUnit.Framework;
using Services;

namespace Tests.Services
{
    [TestFixture]
    public class BiljkeServisTests
    {
        private Mock<IBiljkeRepozitorijum> _mockRepo = null;
        private Mock<IDogadjajiServis> _mockLogger = null;
        private BiljkeServis _servis = null;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IBiljkeRepozitorijum>();
            _mockLogger = new Mock<IDogadjajiServis>();
            _servis = new BiljkeServis(_mockRepo.Object, _mockLogger.Object);
        }

        [Test]
        public void ZasadiNovuBiljku_Uspesno_PozivaLogger()
        {

            _mockLogger.Setup(l => l.Zabelezi(It.IsAny<string>(), It.IsAny<TipEvidencije>(), It.IsAny<Guid?>()))
    .Returns(true);
            // Act
            var rezultat = _servis.ZasadiNovuBiljku("Kamilica", "Matricaria chamomilla", "Srbija", 4.0);

            // Assert

            Assert.That(rezultat, Is.True);
            // Proveravamo da li BiljkeServis šalje poruku tvom loggeru
            _mockLogger.Verify(l => l.Zabelezi(
                It.Is<string>(s => s.Contains("Zasađena nova biljka")),
                TipEvidencije.INFO,
                It.IsAny<Guid?>()),
            Times.Once);
        }

    }
}
