namespace Companion.Models
{
    public class Inschrijving
    {
        public int Id { get; set; }
        public int AantalDeelnemers { get; set; }
        public DateTime Datum { get; set; }
        public string? Opmerking { get; set; } = default!;
        public int GebuikerId { get; set; }
        public Gebruiker Gebruiker { get; set; } = default!;
        public int EvenementId { get; set; }
        public Evenement Evenement { get; set; } = default!;
    }
}
