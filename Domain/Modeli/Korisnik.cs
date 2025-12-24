using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class Korisnik
    {
        public long Id { get; set; } = 0;
        public string KorisnickoIme { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
        public string ImePrezime { get; set; } = string.Empty;
        public TipKorisnika Uloga { get; set; }

        public Korisnik() { }

        public Korisnik(string korisnickoIme, string lozinka, string imePrezime, TipKorisnika tipKorisnika)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            ImePrezime = imePrezime;
            Uloga = tipKorisnika;
        }
    }
}
