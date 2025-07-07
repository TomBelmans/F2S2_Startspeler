namespace Kassa.Data.IRepository
{
    public interface IGebruikerRepository
    {
        // Gebruiker login
        public Gebruiker GebruikerLogin(string email);
        // Lijst van alle gebruikers
        public IEnumerable<Gebruiker> GetGebruikers();
        // Lijst van alle rollen
        public IEnumerable<Aspnetroles> GetAlleRollen();
        // Gebruiker verwijderen
        public bool VerwijderenGebruiker(string id);
        // Gebruiker wijzigen
        public bool WijzigenGebruiker(Gebruiker gebruiker);
    }
}
