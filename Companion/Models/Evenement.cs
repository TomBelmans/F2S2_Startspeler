namespace Companion.Models
{
    public class Evenement
    {
        public int id { get; set; }
        public string naam { get; set; } = default!;
        public DateTime startDatum { get; set; }
        public DateTime eindDatum { get; set; }
        public decimal prijs { get; set; }
        public string? omschrijving { get; set; } = default!;
        public string? winnaar { get; set; } = default!;
        public int? aantalDeelnemers { get; set; }
        public int totaalAantalDeelnemers { get; set; }
        public int communityTypeId { get; set; }
        public Communitytype CommunityType { get; set; } = default!;
    }
}
