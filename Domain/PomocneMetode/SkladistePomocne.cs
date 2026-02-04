using Domain.Modeli;

namespace Domain.PomocneMetode
{
    public static class SkladistePomocne
    {
        public static bool ImaMesta(Skladiste skladiste, int kolicina)
        {
            if (skladiste == null || kolicina <= 0)
            {
                return false;
            }

            return skladiste.TrenutniBrojAmbalaza + kolicina <= skladiste.MaxBrojAmbalaza;
        }

        public static bool DodajAmbalazu(Skladiste skladiste, int kolicina)
        {
            if (!ImaMesta(skladiste, kolicina))
            {
                return false;
            }

            skladiste.TrenutniBrojAmbalaza += kolicina;
            return true;
        }
    }
}