using Domain.Modeli;

namespace Domain.BazaPodataka
{
    public class TabeleBazaPodataka
    {
        public List<Korisnik> Korisnici { get; set; } = [];

        public List<Biljka> Biljke { get; set; } =[];

        public List<Dogadjaj> Dogadjaji { get; set; } = new();


        //  Add other database tables as needed

        public TabeleBazaPodataka() { }
    }
}
