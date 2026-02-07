using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class PreradaServisTests
    {
        private Mock<IPerfumeRepository> _parfemRepoMock = null!;
        private Mock<IBiljkeServis> _biljkeServisMock = null!;
        private Mock<IBiljkeRepozitorijum> _biljkeRepoMock = null!;
        private Mock<ILoggerServis> _loggerServisMock = null!; 
        private PreradaServis _servis = null!;

        [SetUp]
        public void Setup()
        {
            _parfemRepoMock = new Mock<IPerfumeRepository>();
            _biljkeServisMock = new Mock<IBiljkeServis>();
            _biljkeRepoMock = new Mock<IBiljkeRepozitorijum>();
            _loggerServisMock = new Mock<ILoggerServis>(); 

            
            _servis = new PreradaServis(
                _biljkeServisMock.Object,
                _parfemRepoMock.Object,
                _biljkeRepoMock.Object,
                _loggerServisMock.Object
            );
        }

        [Test]
        public void NapraviParfem_ValidniPodaci_IspravnaKalkulacijaKolicine()
        {
            
            string naziv = "TestParfem";
            int brojBocica = 10;
            int zapremina = 150;
            string tip = "Eau de Parfum";

            var biljke = new List<Biljka>();
            for (int i = 1; i <= 200; i++)
            {
                biljke.Add(new Biljka
                {
                    Id = Guid.NewGuid(),
                    Stanje = StanjeBiljke.Ubrana,
                    JacinaArome = 3.0
                });
            }

            _biljkeServisMock.Setup(s => s.SveBiljke()).Returns(biljke);
            _biljkeServisMock.Setup(s => s.DodajBiljku(It.IsAny<Biljka>())).Returns(true);
            _parfemRepoMock.Setup(r => r.Dodaj(It.IsAny<Parfem>()))
                           .Returns((Parfem p) => p);

           
            Parfem parfem;
            bool rezultat = _servis.NapraviParfem(naziv, brojBocica, zapremina, tip, out parfem);

            
            ClassicAssert.IsTrue(rezultat);
            ClassicAssert.IsNotNull(parfem);
            ClassicAssert.AreEqual(naziv, parfem.Naziv);
            ClassicAssert.AreEqual(tip, parfem.TipParfema);
            ClassicAssert.AreEqual(brojBocica, parfem.KolicinaNaStanju);
            ClassicAssert.AreEqual(zapremina, parfem.ZapreminaBociceMl);
            ClassicAssert.AreEqual(brojBocica * zapremina, parfem.UkupnaKolicinaMl);

            _parfemRepoMock.Verify(r => r.Dodaj(It.IsAny<Parfem>()), Times.Once);
        }
    }
}