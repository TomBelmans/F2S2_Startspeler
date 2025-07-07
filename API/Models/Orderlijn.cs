namespace API.Models
{
    public class Orderlijn
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Bestelling")]
        public int BestellingId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int TotaalAantal { get; set; }
        [JsonIgnore]
        public Bestelling? Bestelling { get; set; } = default!;
        [JsonIgnore]
        public Product? Product { get; set; } = default!;
    }
}