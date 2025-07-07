namespace API.Models
{
    public class Evenement
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CommunityType")]
        public int CommunityTypeId { get; set; }

        [StringLength(256)]
        public string Naam { get; set; } = default!;

        [DataType(DataType.DateTime)]
        public DateTime StartDatum { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EindDatum { get; set; }

        public decimal Prijs { get; set; }

        [StringLength(2000)]
        public string? Omschrijving { get; set; } = default!;

        [StringLength(256)]
        public string? Winnaar { get; set; } = default!;

        public byte? AantalDeelnemers { get; set; }

        public byte TotaalAantalDeelnemers { get; set; }

        public CommunityType CommunityType { get; set; } = default!;
        
        [JsonIgnore]
        public ICollection<Inschrijving> Inschrijvingen { get; set; } = default!;
    }
}
