namespace API.Models
{
    public class Gebruiker : IdentityUser
    {
        [StringLength(256)]
        public string Voornaam { get; set; } = default!;

        [StringLength(256)]
        public string Achternaam { get; set; } = default!;

        [JsonIgnore]
        public ICollection<Bestelling> Bestellingen { get; set; } = new List<Bestelling>();
        [JsonIgnore]
        public ICollection<Inschrijving> Inschrijvingen { get; set; } = new List<Inschrijving>();

    }
}
