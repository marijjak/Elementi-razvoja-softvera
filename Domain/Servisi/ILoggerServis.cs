using Domain.Enumeracije;

namespace Domain.Servisi
{
    public interface ILoggerServis
    {
        bool InicijalizujLogFajl();
        public bool EvidentirajDogadjaj(TipEvidencije tip, string poruka); 
    }
}
