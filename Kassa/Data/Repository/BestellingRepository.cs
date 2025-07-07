namespace Kassa.Data.Repository
{
    public class BestellingRepository : BaseRepository, IBestellingRepository
    {
        public List<Bestelling> GetBestellingenMetProducten()
        {
            string sql = @"
                         SELECT B.*, U.*, OL.*, P.*
                         FROM Bestelling B
                         JOIN Gebruiker U ON B.GebruikerId = U.Id
                         JOIN Orderlijn OL ON B.Id = OL.BestellingId
                         JOIN Product P ON OL.ProductId = P.Id
                         ORDER BY B.Datum DESC";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var bestellingDictionary = new Dictionary<int, Bestelling>();

                var bestellingen = db.Query<Bestelling, Gebruiker, Orderlijn, Product, Bestelling>(
                    sql,
                    (bestelling, gebruiker, orderlijn, product) =>
                    {
                        if (!bestellingDictionary.TryGetValue(bestelling.Id, out var bestellingEntry))
                        {
                            bestellingEntry = bestelling;
                            bestellingEntry.Gebruiker = gebruiker;
                            bestellingEntry.Orderlijnen = new List<Orderlijn>();
                            bestellingDictionary.Add(bestellingEntry.Id, bestellingEntry);
                        }

                        if (orderlijn != null)
                        {
                            orderlijn.Product = product;
                            bestellingEntry.Orderlijnen.Add(orderlijn);
                        }

                        return bestellingEntry;
                    },
                    splitOn: "Id,Id,Id,Id"
                )
                .Distinct()
                .ToList();

                return bestellingen;
            }
        }
        public Bestelling GetBestellingById(int Id)
        {
            string sql = @"
        SELECT B.*, G.*, OL.*, P.*
        FROM Bestelling B
        JOIN Gebruiker G ON B.GebruikerId = G.Id
        JOIN Orderlijn OL ON B.id = OL.bestellingId
        JOIN Product P ON OL.productId = P.id
        WHERE B.Id = @Id";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var bestellingen = db.Query<Bestelling, Gebruiker, Orderlijn, Product, Bestelling>(
            sql,
            (bestelling, gebruiker, orderlijn, product) =>
            {
                // Voeg de orderlijn toe aan de bestelling en koppel het product
                bestelling.Gebruiker = gebruiker;
                orderlijn.Product = product;
                bestelling.Orderlijnen.Add(orderlijn);
                return bestelling;
            },
            new { Id },
            splitOn: "Id,Id,Id")
            .ToList();

                var result = bestellingen
                    .GroupBy(b => b.Id)
                    .Select(g =>
                    {
                        var groupedBestelling = g.First();
                        groupedBestelling.Orderlijnen = g.SelectMany(b => b.Orderlijnen).ToList();
                        return groupedBestelling;
                    })
                    .SingleOrDefault();

                return result;
            }
        }

        public bool UpdateBestelling(Bestelling bestelling)
        {
            string sql = @"
                UPDATE Bestelling
                SET TafelNummer = @TafelNummer, KlantNaam = @KlantNaam, Datum = @Datum, TotaalPrijs = @Prijs, IsBetaald = @Betaald, BestellingVerwerkt = @Verwerkt
                WHERE Id = @Id";

            var parameters = new
            {
                tafelnummer = bestelling.TafelNummer,
                klantnaam = bestelling.KlantNaam,
                datum = bestelling.Datum,
                prijs = bestelling.TotaalPrijs,
                betaald = bestelling.IsBetaald,
                verwerkt = bestelling.BestellingVerwerkt,
                id = bestelling.Id,
            };

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);
            return affectedRows == 1;
        }
        public bool VerwijderBestelling(int id)
        {
            string sql = "DELETE FROM Bestelling WHERE Id = @Id";

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, new { Id = id });
            return affectedRows >= 1;
        }
    }
}

