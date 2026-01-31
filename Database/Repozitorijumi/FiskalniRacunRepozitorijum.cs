using Domain.Modeli;
using Domain.Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repozitorijumi
{
    using Domain.BazaPodataka;
    using Domain.Enumeracije;
    using Domain.Modeli;
    using Domain.Repozitorijumi;
    using System;
    using System.Collections.Generic;
    using System.IO;

    namespace Database.Repozitorijumi
    {
        public class FiskalniRacunRepozitorijum : IFiskalniRacunRepozitorijum
        {
            private readonly IBazaPodataka _baza;

            private string _putanjaDoFajla =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "racuni.txt");

            public FiskalniRacunRepozitorijum(IBazaPodataka baza)
            {
                _baza = baza;
            }

            public bool Dodaj(FiskalniRacun racun)
            {
                try
                {
                    string stavkeTekst = string.Join(",", racun.Stavke.Select(s => $"{s.Key}:{s.Value}"));

                    string linija =
                        $"{racun.Id};" +
                        $"{racun.DatumIzdavanja:dd.MM.yyyy HH:mm};" + // Dodaj i sate
                        $"{racun.ImeProdavca};" + // DODAJ OVO
                        $"{racun.TipProdaje};" +
                        $"{racun.NacinPlacanja};" +
                        $"{racun.UkupanIznos};" +
                        $"{stavkeTekst}"; 

                    File.AppendAllLines(_putanjaDoFajla, new[] { linija });
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public IEnumerable<FiskalniRacun> GetSviRacuni()
            {
                if (!File.Exists(_putanjaDoFajla))
                {
                    return new List<FiskalniRacun>();
                }

                var linije = File.ReadAllLines(_putanjaDoFajla);
                var racuni = new List<FiskalniRacun>();

                foreach (var linija in linije)
                {
                    if (string.IsNullOrWhiteSpace(linija)) continue;

                    var delovi = linija.Split(';');

                    racuni.Add(new FiskalniRacun
                    {
                        Id = Guid.Parse(delovi[0]),
                        DatumIzdavanja = DateTime.ParseExact(delovi[1], "dd.MM.yyyy HH:mm", null),
                        ImeProdavca = delovi[2],
                        TipProdaje = Enum.Parse<TipProdaje>(delovi[3]),
                        NacinPlacanja = Enum.Parse<NacinPlacanja>(delovi[4]),
                        UkupanIznos = decimal.Parse(delovi[5])
                    });
                }


                return racuni;
            }
        }
    }


}
