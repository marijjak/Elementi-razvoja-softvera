using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.PomocneMetode;
using Moq;
using NUnit.Framework;
using Services;
using System;

namespace Tests.Services
{
    [TestFixture]
    public class SkladisteServisTests
    {
        private Mock<ISkladisteRepozitorijum> _mockRepo;
        private SkladisteServis _servis;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ISkladisteRepozitorijum>();
            _servis = new SkladisteServis(_mockRepo.Object);
        }

        [Test]
        public void DodajAmbalazuUSkladiste_ImaMesta_VracaTrueISacuva()
        {
            // Arrange
            var skladiste = new Skladiste
            {
                Id = Guid.NewGuid(),
                MaxBrojAmbalaza = 10,
                TrenutniBrojAmbalaza = 5
            };

            _mockRepo
                .Setup(r => r.NadjiPoId(skladiste.Id))
                .Returns(skladiste);

            // Act
            var rezultat = _servis.DodajAmbalazuUSkladiste(skladiste.Id, 3);

            // Assert
            Assert.That(rezultat, Is.True);
            Assert.That(skladiste.TrenutniBrojAmbalaza, Is.EqualTo(8));

            _mockRepo.Verify(r => r.Sacuvaj(), Times.Once);
        }

        [Test]
        public void DodajAmbalazuUSkladiste_NemaMesta_VracaFalseINeCuva()
        {
            // Arrange
            var skladiste = new Skladiste
            {
                Id = Guid.NewGuid(),
                MaxBrojAmbalaza = 10,
                TrenutniBrojAmbalaza = 9
            };

            _mockRepo
                .Setup(r => r.NadjiPoId(skladiste.Id))
                .Returns(skladiste);

            // Act
            var rezultat = _servis.DodajAmbalazuUSkladiste(skladiste.Id, 2);

            // Assert
            Assert.That(rezultat, Is.False);
            Assert.That(skladiste.TrenutniBrojAmbalaza, Is.EqualTo(9));

            _mockRepo.Verify(r => r.Sacuvaj(), Times.Never);
        }
    }
}
