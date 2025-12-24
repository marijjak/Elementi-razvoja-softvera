using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IAutentifikacijaServis
    {
        public (bool, Korisnik) Prijava(string korisnickoIme, string lozinka);
    }
}
