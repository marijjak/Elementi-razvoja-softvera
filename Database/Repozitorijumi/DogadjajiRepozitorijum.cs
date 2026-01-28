using Domain.Repozitorijumi;
using Domain.Modeli;
using Domain.BazaPodataka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Database.Repozitorijumi
{
    public class DogadjajiRepozitorijum : IDogadjajiRepozitorijum
    {
        // Ovo će osigurati da se fajl pojavi baš u tom net8.0 folderu koji si otvorila
        private string _putanjaDoFajla = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "loger.txt");
        public DogadjajiRepozitorijum(IBazaPodataka baza)
        {
            
        }

        public bool Dodaj(Dogadjaj dogadjaj)
        {
            try
            {
                string linija = $"[{dogadjaj.Tip}] {dogadjaj.Vreme:dd.MM.yyyy HH:mm:ss} : {dogadjaj.Opis}";

                File.AppendAllText(_putanjaDoFajla, linija + Environment.NewLine);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Dogadjaj> Sve()
        {
            if (!File.Exists(_putanjaDoFajla))
            {
                return new List<Dogadjaj>();
            }

            var sveLinije = File.ReadAllLines(_putanjaDoFajla);
            var listaDogadjaja = new List<Dogadjaj>();

            foreach (var linija in sveLinije)
            {
                listaDogadjaja.Add(new Dogadjaj
                { Opis = linija,
                  Vreme = DateTime.Now

                });
            }

            return listaDogadjaja;
        }
    }
}
