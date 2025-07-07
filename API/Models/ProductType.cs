namespace API.Models
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }

        [StringLength(256)]
        public string Naam { get; set; } = default!;

        [JsonIgnore]
        public ICollection<Product> Producten { get; set; } = default!;
    }
}
