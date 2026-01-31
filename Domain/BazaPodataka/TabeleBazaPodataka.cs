using Domain.Modeli;

namespace Domain.BazaPodataka
{
    public class TabeleBazaPodataka
    {
        public List<Korisnik> Korisnici { get; set; } = [];

        public List<Biljka> Biljke { get; set; } =[];

        public List<Parfem> Parfemi { get; set; } = [];
        public List<Dogadjaj> Dogadjaji { get; set; } = new();

        public List<Ambalaza> Ambalaze { get; set; } = [];

        //  Add other database tables as needed
        public List<Skladiste> Skladista { get; set; } = new();
        public TabeleBazaPodataka() { }

        public List<FiskalniRacun> FiskalniRacuni { get; set; } = new List<FiskalniRacun>();
    }
}
