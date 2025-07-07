using System.Text.Json.Serialization;

namespace Companion.Models
{
    public class Bestelling
    {
        public int Id { get; set; }
        public string GebruikerId { get; set; } = default!;
        public string? KlantNaam { get; set; } = default!;
        public string TafelNummer { get; set; } = default!;
        public decimal TotaalPrijs { get; set; }
        public DateTime Datum { get; set; }
        public string? Opmerking { get; set; } = default!;
        public bool IsBetaald { get; set; }

        [JsonIgnore]
        public Gebruiker Gebruiker { get; set; } = default!;
        [JsonIgnore]
        public List<Orderlijn> Orderlijnen { get; set; } = default!;

        // Geeft een Ja/Nee string terug indien de bestelling al-dan-niet bestaald is
        [JsonIgnore]
        public string Betaald => IsBetaald ? "Ja" : "Nee";
    }
}
