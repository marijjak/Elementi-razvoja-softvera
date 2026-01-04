using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.BazaPodataka;
using System.Text.Json;

namespace Database.BazaPodataka
{
    public class JsonBazaPodataka : IBazaPodataka
    {
        private const string Putanja = "baza.json";

        public TabeleBazaPodataka Tabele { get; set; }

        public JsonBazaPodataka()
        {
            if (File.Exists(Putanja))
            {
                var json = File.ReadAllText(Putanja);
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
            File.WriteAllText(Putanja, json);
            return true;
        }
    }
}
