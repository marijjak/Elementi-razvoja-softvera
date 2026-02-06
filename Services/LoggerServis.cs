using Domain.Enumeracije;
using Domain.Servisi;
using System;
using System.IO;

namespace Services
{
    public class LoggerServis : ILoggerServis
    {
        private readonly string _putanja = "log.txt";

        public bool InicijalizujLogFajl()
        {
            try
            {
                if (!File.Exists(_putanja))
                {
                    using (FileStream fs = File.Create(_putanja))
                    {
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool EvidentirajDogadjaj(TipEvidencije tip, string poruka)
        {
            try
            {
                string linija = $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} | {tip} | {poruka}";
                File.AppendAllText(_putanja, linija + Environment.NewLine);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
