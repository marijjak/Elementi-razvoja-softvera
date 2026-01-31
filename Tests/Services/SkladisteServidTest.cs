using Domain.Enumeracije;
using Domain.Modeli;
using Domain.PomocneMetode;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Moq;
using NUnit.Framework;
using Services;
using System;
using System.Threading.Tasks; 

namespace Tests.Services
{
    [TestFixture]
    public class SkladisteServisTests
    {
        private Mock<ISkladisteRepozitorijum> _mockRepo = null;
        private SkladisteServis _servis = null;
        private Mock<IAmbalazaRepozitorijum> _mockAmbalazaRepo = null; 
        private Mock<IDogadjajiServis> _mockDogadjaji = null;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ISkladisteRepozitorijum>();
            _servis = new SkladisteServis(_mockRepo.Object);
            _mockAmbalazaRepo = new Mock<IAmbalazaRepozitorijum>();
            _mockDogadjaji = new Mock<IDogadjajiServis>();
        }

       

        [Test]
        public void Prodaja_SmanjujeBrojAmbalazaUSkladistu()
        {
            var magacin = new MagacinskiCentarServis(_mockAmbalazaRepo.Object, _mockDogadjaji.Object);

           
        }

        [Test]
        public async Task Test_PosaljiPaketAsync_AzuriraStatus()
        {
           
            var ambalazaId = Guid.NewGuid();
            var ambalaza = new Ambalaza { Id = ambalazaId, Status = StatusAmbalaze.Spakovana };

            _mockAmbalazaRepo.Setup(r => r.NadjiPoId(ambalazaId)).Returns(ambalaza);
            var servis = new MagacinskiCentarServis(_mockAmbalazaRepo.Object, _mockDogadjaji.Object);

            
            var rezultat = await servis.PosaljiPaketAsync(ambalazaId);

          
            Assert.That(rezultat, Is.True);
        }
    }
}