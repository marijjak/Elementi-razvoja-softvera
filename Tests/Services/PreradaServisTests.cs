using Domain.Repozitorijumi;
using Domain.Servisi;
using Moq;
using NUnit.Framework;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    [TestFixture]
    public class PreradaServisTests
    {
        private Mock<IBiljkeServis> _biljkeServisMock = null;
        private Mock<IPerfumeRepository> _parfemRepoMock = null;
        private Mock<IBiljkeRepozitorijum> _biljkeRepoMock = null;
        private PreradaServis _servis = null; 

        [SetUp]
        public void Setup()
        {
            _biljkeServisMock = new Mock<IBiljkeServis>();
            _parfemRepoMock = new Mock<IPerfumeRepository>();
            _biljkeRepoMock = new Mock<IBiljkeRepozitorijum>();

            _servis = new PreradaServis(
                _biljkeServisMock.Object,
                _parfemRepoMock.Object,
                _biljkeRepoMock.Object
            );
        }

        [Test]
        public void NekiTestPrerade_TrebaDaZabeleziDogadjaj()
        {
            
            Assert.Pass();
        }
    }
}
