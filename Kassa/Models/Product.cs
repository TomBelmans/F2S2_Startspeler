namespace Kassa.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int ProducttypeId { get; set; }
        public string Naam { get; set; } = default!;
        public decimal Prijs { get; set; }
        public byte[]? Afbeelding { get; set; } = default!;
        public ImageSource AfbeeldingSource
        {
            get
            {
                if (Afbeelding != null && Afbeelding.Length > 0)
                {
                    return ImageSource.FromStream(() => new MemoryStream(Afbeelding));
                }
                return null;
            }
            set { }
        }
        public string Beschrijving { get; set; } = default!;
        public int Aantal { get; set; }
        public Producttype Producttype { get; set; } = default!;
    }
}