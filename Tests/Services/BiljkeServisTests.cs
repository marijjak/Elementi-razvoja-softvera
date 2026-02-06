using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Moq;
using NUnit.Framework;
using Services;
using System;

namespace Tests.Services
{
    [TestFixture]
    public class BiljkeServisTests
    {
        private Mock<IBiljkeRepozitorijum> _mockRepo = null!;
        private Mock<IDogadjajiServis> _mockDogadjaji = null!; 
        private Mock<ILoggerServis> _mockLogger = null!; 
        private BiljkeServis _servis = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IBiljkeRepozitorijum>();
            _mockDogadjaji = new Mock<IDogadjajiServis>();
            _mockLogger = new Mock<ILoggerServis>(); 

           
            _servis = new BiljkeServis(_mockRepo.Object, _mockDogadjaji.Object, _mockLogger.Object);
        }

        [Test]
        public void ZasadiNovuBiljku_Uspesno_PozivaLogger()
        {
            
            _mockRepo
                .Setup(r => r.Dodaj(It.IsAny<Biljka>()))
                .Returns((Biljka b) => b);

            _mockDogadjaji
                .Setup(l => l.Zabelezi(It.IsAny<string>(), It.IsAny<TipEvidencije>(), It.IsAny<Guid?>()))
                .Returns(true);

            
            var rezultat = _servis.ZasadiNovuBiljku("Kamilica", "Matricaria chamomilla", "Srbija", 4.0);

            
            Assert.That(rezultat, Is.True);

            _mockRepo.Verify(r => r.Dodaj(It.IsAny<Biljka>()), Times.Once);

            _mockDogadjaji.Verify(l => l.Zabelezi(
                    It.Is<string>(s => s.Contains("Zasađena nova biljka")),
                    TipEvidencije.INFO,
                    It.IsAny<Guid?>()),
                Times.Once);
        }
    }
}