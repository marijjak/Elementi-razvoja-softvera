using Domain.Enumeracije;

namespace Domain.Servisi
{
    public interface ILoggerServis
    {
        public bool EvidentirajDogadjaj(TipEvidencije tip, string poruka);
    }
}
