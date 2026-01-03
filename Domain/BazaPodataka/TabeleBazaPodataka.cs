using Domain.Modeli;

namespace Domain.BazaPodataka
{
    public class TabeleBazaPodataka
    {
        public List<Korisnik> Korisnici { get; set; } = [];
        public List<Biljka> Biljka { get; set; } = [];

        // TODO: Add other database tables as needed

        public TabeleBazaPodataka() { }
    }
}
