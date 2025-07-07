namespace Companion.Models
{
    public partial class Product : ObservableObject
    {
        public int id { get; set; }
        public int producttypeId { get; set; }
        public string naam { get; set; } = default!;
        public decimal prijs { get; set; }
        public byte[] afbeelding { get; set; } = default!;
        public ImageSource AfbeeldingSource
        {
            get
            {
                if (afbeelding != null && afbeelding.Length > 0)
                {
                    return ImageSource.FromStream(() => new MemoryStream(afbeelding));
                }
                return null;
            }
        }
        public string beschrijving { get; set; } = default!;

        private int Aantal;
        public int aantal
        {
            get => Aantal;
            set
            {
                SetProperty(ref Aantal, value);
                OnPropertyChanged(nameof(beschikbaarheid)); // Notify that beschikbaarheid has changed
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PrijsPerProduct))]
        public int aantalInWinkelmand;

        public Producttype Producttype { get; set; } = default!;

        public string beschikbaarheid
        {
            get
            {
                return aantal == 0 || aantal == null ? "Uitverkocht" : "Beschikbaar: " + aantal;
            }
        }

        public decimal PrijsPerProduct => aantalInWinkelmand * prijs;
    }
}
