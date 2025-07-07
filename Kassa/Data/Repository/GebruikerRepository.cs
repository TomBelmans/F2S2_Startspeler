namespace Kassa.Data.Repository
{
    public class GebruikerRepository : BaseRepository, IGebruikerRepository
    {
        // Gebruiker login
        public Gebruiker GebruikerLogin(string email)
        {
            string sql = @"SELECT g.*, aur.*, ar.*
            FROM gebruiker AS g
           LEFT JOIN aspnetuserroles AS aur ON aur.UserId = g.Id
           LEFT JOIN aspnetroles AS ar ON aur.RoleId = ar.Id
           WHERE g.Email = @Email";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var gebruikers = db.Query<Gebruiker, Aspnetroles, Gebruiker>(
                    sql,
                    (gebruiker, rol) =>
                    {
                        gebruiker.Rol = rol;
                        return gebruiker;
                    },
                    new { Email = email },
                    splitOn: "UserId,RoleId");

                return gebruikers.FirstOrDefault();
            }
        }

        // Lijst van alle gebruikers
        public IEnumerable<Gebruiker> GetGebruikers()
        {
            string sql = @"SELECT g.*, aur.*, ar.*
                 FROM gebruiker AS g
                 JOIN aspnetuserroles AS aur ON aur.UserId = g.Id
                 JOIN aspnetroles AS ar ON aur.RoleId = ar.Id
                 ORDER BY g.Achternaam DESC";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var gebruikers = db.Query<Gebruiker, Aspnetroles, Gebruiker>(
                    sql,
                    (gebruiker, rol) =>
                    {
                        gebruiker.Rol = rol;
                        return gebruiker;
                    },
                    splitOn: "UserId,RoleId");

                return gebruikers.ToList();
            }
        }

        // Lijst van alle rollen
        public IEnumerable<Aspnetroles> GetAlleRollen()
        {
            string sql = @"SELECT * FROM aspnetroles";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                return db.Query<Aspnetroles>(sql).ToList();
            }
        }

        // Gebruiker verwijderen
        public bool VerwijderenGebruiker(string id)
        {
            string sql = @"DELETE FROM gebruiker WHERE id = @id";

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, new { id });

            return affectedRows == 1;
        }

        // Gebruiker wijzigen
        public bool WijzigenGebruiker(Gebruiker gebruiker)
        {
            string sql = @"UPDATE gebruiker as g 
                           JOIN aspnetuserroles as aur on aur.UserId = g.Id
                           SET g.username = @username, g.voornaam = @voornaam, g.achternaam = @achternaam, g. email = @email, aur.RoleId = @rolId
                           WHERE g.Id = @Id";

            var parameters = new
            {
                rolId = gebruiker.Rol.Id,
                username = gebruiker.UserName,
                voornaam = gebruiker.Voornaam,
                achternaam = gebruiker.Achternaam,
                email = gebruiker.Email,
                id = gebruiker.Id
            };

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);

            return affectedRows == 2;
        }

    }
}