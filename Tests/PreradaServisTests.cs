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
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class PreradaServisTests
    {
        private Mock<IPerfumeRepository> _parfemRepoMock = null;
        private Mock<IDogadjajiServis> _dogadjajiMock = null;
        private Mock<IBiljkeServis> _biljkeServisMock = null!;
        private PreradaServis _servis = null;


        [SetUp]
        public void Setup()
        {
            // Kreiramo "lažne" verzije zavisnosti
            _parfemRepoMock = new Mock<IPerfumeRepository>();
            _dogadjajiMock = new Mock<IDogadjajiServis>();
            _biljkeServisMock = new Mock<IBiljkeServis>();

            // Inicijalizujemo servis sa tim lažnim podacima

            _servis = new PreradaServis(_biljkeServisMock.Object, _parfemRepoMock.Object);
        }

        public void NapraviParfem_ValidniPodaci_IspravnaKalkulacijaKolicine()
        {
            // Arrange - Postavljanje ulaznih podataka
            string naziv = "TestParfem";
            int brojBocica = 10;
            int zapremina = 150; 
            string tip = "Eau de Parfum";

            _biljkeServisMock.Setup(s => s.SveBiljke()).Returns(new List<Biljka> {
    new Biljka { Stanje = StanjeBiljke.Ubrana, JacinaArome = 3.0 }
});

            // Act - Izvršavanje metode koju testiramo
            var rezultat = _servis.NapraviParfem(naziv, brojBocica, zapremina, tip);

            // Assert - Provera rezultata
            ClassicAssert.That(rezultat, Is.Not.Null);
            ClassicAssert.That(rezultat.Naziv, Is.EqualTo(naziv));

            // KLJUČNA PROVERA: Da li je mililitraža ispravno izračunata (broj * zapremina)
            ClassicAssert.AreEqual(brojBocica, rezultat.KolicinaNaStanju);
            ClassicAssert.AreEqual(zapremina, rezultat.ZapreminaBociceMl);

            // Provera da li je repozitorijum pozvan da sačuva parfem
            _parfemRepoMock.Verify(r => r.Dodaj(It.IsAny<Parfem>()), Times.Once);
        }
    }
}
