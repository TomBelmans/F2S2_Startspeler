namespace Kassa.Models
{
    public class Bestelling
    {
        public int Id { get; set; }
        public string GebruikerId { get; set; } = default!;
        public string KlantNaam { get; set; } = default!;
        public string TafelNummer { get; set; } = default!;
        public decimal TotaalPrijs { get; set; }
        public DateTime Datum { get; set; }
        public bool IsBetaald { get; set; }
        public bool BestellingVerwerkt { get; set; }
        public string Opmerking { get; set; } = default!;
        public Gebruiker Gebruiker { get; set; } = default!;
        public List<Orderlijn> Orderlijnen { get; set; } = new List<Orderlijn>();
        public int AantalBestellingen { get; set; }
        public string Verwerkt
        {
            get
            {
                return BestellingVerwerkt ? "Ja" : "Nee";
            }
        }

        public string Betaald
        {
            get
            {
                return IsBetaald ? "Ja" : "Nee";
            }
        }
        public decimal BerekenTotaalPrijs()
        {
            return Orderlijnen.Sum(ol => ol.Product.Prijs * ol.TotaalAantal);
        }

        public decimal TotaalPrijsAfrekening => BerekenTotaalPrijs();
    }
}
