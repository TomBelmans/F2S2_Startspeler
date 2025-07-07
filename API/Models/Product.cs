namespace API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ProductType")]
        public int ProductTypeId { get; set; }

        [StringLength(256)]
        public string? Naam { get; set; } = default!;

        public decimal Prijs { get; set; }

        public byte[]? Afbeelding { get; set; } = default!;

        [StringLength(2000)]
        public string? Beschrijving { get; set; }

        public short? Aantal { get; set; }

        public ProductType? ProductType { get; set; } = default!;

        [JsonIgnore]
        public ICollection<Orderlijn>? Orderlijnen { get; set; } = default!;
    }
}
