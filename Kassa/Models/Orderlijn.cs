namespace Kassa.Models
{
    public class Orderlijn
    {
        public int Id { get; set; }
        public int BestellingId { get; set; }
        public int ProductId { get; set; }
        public int TotaalAantal { get; set; }
        public Bestelling Bestelling { get; set; } = default!;
        public Product Product { get; set; } = default!;
    }
}
