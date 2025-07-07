namespace Kassa.Data.IRepository
{
    public interface IAfrekenRepository
    {
        public List<Bestelling> GetBestellingen();
        public void UpdateBestellingStatus(int bestellingId, bool isBetaald);
        public List<Bestelling> GetBestellingenOpNaam(string achternaam);
    }
}
