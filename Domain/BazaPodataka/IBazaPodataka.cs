namespace Domain.BazaPodataka
{
    public interface IBazaPodataka
    {
        public TabeleBazaPodataka Tabele { get; set; }

        public bool SacuvajPromene();
    }
}
