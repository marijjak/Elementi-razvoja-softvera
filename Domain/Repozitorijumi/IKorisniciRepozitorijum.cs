using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IKorisniciRepozitorijum
    {
        public Korisnik DodajKorisnika(Korisnik korisnik);
        public Korisnik PronadjiKorisnikaPoKorisnickomImenu(string korisnickoIme);
        public IEnumerable<Korisnik> SviKorisnici();
    }
}
