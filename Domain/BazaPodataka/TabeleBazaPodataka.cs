using Domain.Modeli;

namespace Domain.BazaPodataka
{
    public class TabeleBazaPodataka
    {
        public List<Korisnik> Korisnici { get; set; } = [];

        public List<Biljka> Biljke { get; set; } = new List<Biljka>();

        //  Add other database tables as needed

        public TabeleBazaPodataka() { }
    }
}
