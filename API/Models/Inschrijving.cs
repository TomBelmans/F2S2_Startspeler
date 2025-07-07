namespace API.Models
{
    public class Inschrijving
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Gebruiker")]
        public string GebruikerId { get; set; } = default!;

        [ForeignKey("Evenement")]
        public int EvenementId { get; set; }

        [StringLength(maximumLength: 255, MinimumLength = 2)]
        public byte AantalDeelnemers { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Datum { get; set; }

        [StringLength(2000)]
        public string? Opmerking { get; set; }

        public Gebruiker Gebruiker { get; set; } = default!;
        public Evenement Evenement { get; set; } = default!;
    }
}
