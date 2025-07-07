namespace API.Models
{
    public class Bestelling
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Gebruiker")]
        public string GebruikerId { get; set; } = default!;

        [StringLength(256)]
        public string? KlantNaam { get; set; } = default!;

        [StringLength(256)]
        public string Tafelnummer { get; set; } = default!;

        public decimal TotaalPrijs { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Datum { get; set; }

        [StringLength(2000)]
        public string? Opmerking { get; set; }

        public bool IsBetaald { get; set; }

        public bool BestellingVerwerkt { get; set; }

        [JsonIgnore]
        public Gebruiker? Gebruiker { get; set; } = default!;
        [JsonIgnore]
        public ICollection<Orderlijn>? Orderlijnen { get; set; } = default!;
    }
}