using CommunityToolkit.Mvvm.ComponentModel;

namespace Companion.Models
{
    public partial class Orderlijn : ObservableObject
    {
        public int Id { get; set; }
        public int BestellingId { get; set; }
        public int ProductId { get; set; }

        [ObservableProperty]
        private int totaalAantal;

        public Bestelling Bestelling { get; set; } = default!;
        public Product Product { get; set; } = default!;
    }
}