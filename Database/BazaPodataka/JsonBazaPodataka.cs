using Domain.BazaPodataka;
using Domain.Konstante;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Database.BazaPodataka
{
    public class JsonBazaPodataka : IBazaPodataka
    {
        private const string Putanja = "baza.json";

        public TabeleBazaPodataka Tabele { get; set; }
        public List<FiskalniRacun> FiskalniRacuni { get; set; } = new List<FiskalniRacun>();

        public JsonBazaPodataka()
        {
            if (File.Exists(KONSTANTE.PutanjaBaze))
            {
           
                var json = File.ReadAllText(KONSTANTE.PutanjaBaze);
                Tabele = JsonSerializer.Deserialize<TabeleBazaPodataka>(json)
                         ?? new TabeleBazaPodataka();
            }
            else
            {
                Tabele = new TabeleBazaPodataka();
                SacuvajPromene();
            }
        }

        public bool SacuvajPromene()
        {
            var opcije = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(Tabele, opcije);
            File.WriteAllText(KONSTANTE.PutanjaBaze, json);
            return true;
        }
    }
}
