using Domain.Servisi;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    public class PreradaServisTests
    {
        private Mock<IDogadjajiServis> _mockLogger = null!;
        // Ovde dodaj ostale mock-ove koji trebaju tvom PreradaServisu

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<IDogadjajiServis>();
            // Inicijalizuj servis ovde
        }

        [Test]
        public void NekiTestPrerade_TrebaDaZabeleziDogadjaj()
        {
            // Assert - Proveri da li se i ovde poziva logger
            Assert.Pass(); // Privremeni pass dok ne povežeš logiku
        }
    }
}
