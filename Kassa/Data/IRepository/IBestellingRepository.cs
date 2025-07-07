namespace Kassa.Data.IRepository
{
    public interface IBestellingRepository
    {
        List<Bestelling> GetBestellingenMetProducten();
        Bestelling GetBestellingById(int Id);
        bool UpdateBestelling(Bestelling bestelling);
        bool VerwijderBestelling(int Id);
    }
}
