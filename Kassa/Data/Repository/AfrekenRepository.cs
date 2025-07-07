namespace Kassa.Data.Repository
{
    public class AfrekenRepository : BaseRepository, IAfrekenRepository
    {
        public List<Bestelling> GetBestellingen()
        {
            string sql = @"SELECT b.*, g.*, ol.*, p.*
                   FROM Bestelling b
                   JOIN Gebruiker g ON b.GebruikerId = g.Id
                   JOIN Orderlijn ol ON b.Id = ol.BestellingId
                   JOIN Product p ON ol.ProductId = p.Id
                   WHERE b.IsBetaald = 0
                   ORDER BY b.Datum DESC";

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
                ).Distinct().ToList();

                foreach (var bestelling in bestellingen)
                {
                    bestelling.TotaalPrijs = bestelling.BerekenTotaalPrijs();
                }

                // Groepeer bestellingen per klant per dag
                var groupedBestellingen = bestellingen
                    .GroupBy(b => new { b.GebruikerId, b.KlantNaam, Datum = b.Datum.Date })
                    .Select(g => new Bestelling
                    {
                        GebruikerId = g.Key.GebruikerId,
                        Gebruiker = new Gebruiker { Id = g.Key.GebruikerId, Voornaam = g.First().Gebruiker.Voornaam, Achternaam = g.First().Gebruiker.Achternaam },
                        KlantNaam = g.Key.KlantNaam,
                        Datum = g.Key.Datum,
                        Orderlijnen = g.SelectMany(b => b.Orderlijnen)
                                      .GroupBy(ol => new { ol.ProductId, ol.Product.Naam, ol.Product.Prijs })
                                      .Select(olGroup => new Orderlijn
                                      {
                                          ProductId = olGroup.Key.ProductId,
                                          Product = new Product
                                          {
                                              Id = olGroup.Key.ProductId,
                                              Naam = olGroup.Key.Naam,
                                              Prijs = olGroup.Key.Prijs
                                          },
                                          TotaalAantal = olGroup.Sum(ol => ol.TotaalAantal)
                                      }).ToList(),
                        AantalBestellingen = g.Count() // Aantal bestellingen per klant per dag
                    }).ToList();

                foreach (var bestelling in groupedBestellingen)
                {
                    bestelling.TotaalPrijs = bestelling.BerekenTotaalPrijs();
                }

                return groupedBestellingen;
            }
        }

        public List<Bestelling> GetBestellingenOpNaam(string achternaam)
        {
            string sql = @"SELECT b.*, g.*, ol.*, p.*
                   FROM Bestelling b
                   JOIN Gebruiker g ON b.GebruikerId = g.Id
                   JOIN Orderlijn ol ON b.Id = ol.BestellingId
                   JOIN Product p ON ol.ProductId = p.Id
                   WHERE b.IsBetaald = 0 and Achternaam LIKE @Achternaam
                   ORDER BY b.Datum DESC";

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
                    new { Achternaam = "%" + achternaam + "%" },
                    splitOn: "Id,Id,Id,Id"
                ).Distinct().ToList();

                foreach (var bestelling in bestellingen)
                {
                    bestelling.TotaalPrijs = bestelling.BerekenTotaalPrijs();
                }

                // Groepeer bestellingen per klant per dag
                var groupedBestellingen = bestellingen
                    .GroupBy(b => new { b.GebruikerId, b.KlantNaam, Datum = b.Datum.Date })
                    .Select(g => new Bestelling
                    {
                        GebruikerId = g.Key.GebruikerId,
                        Gebruiker = new Gebruiker { Id = g.Key.GebruikerId, Voornaam = g.First().Gebruiker.Voornaam, Achternaam = g.First().Gebruiker.Achternaam },
                        KlantNaam = g.Key.KlantNaam,
                        Datum = g.Key.Datum,
                        Orderlijnen = g.SelectMany(b => b.Orderlijnen)
                                      .GroupBy(ol => new { ol.ProductId, ol.Product.Naam, ol.Product.Prijs })
                                      .Select(olGroup => new Orderlijn
                                      {
                                          ProductId = olGroup.Key.ProductId,
                                          Product = new Product
                                          {
                                              Id = olGroup.Key.ProductId,
                                              Naam = olGroup.Key.Naam,
                                              Prijs = olGroup.Key.Prijs
                                          },
                                          TotaalAantal = olGroup.Sum(ol => ol.TotaalAantal)
                                      }).ToList(),
                        AantalBestellingen = g.Count() // Aantal bestellingen per klant per dag
                    }).ToList();

                foreach (var bestelling in groupedBestellingen)
                {
                    bestelling.TotaalPrijs = bestelling.BerekenTotaalPrijs();
                }

                return groupedBestellingen;
            }
        }


        public void UpdateBestellingStatus(int bestellingId, bool isBetaald)
        {
            string sql = "UPDATE Bestelling SET IsBetaald = @IsBetaald WHERE Id = @Id";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                db.Execute(sql, new { IsBetaald = isBetaald, Id = bestellingId });
            }
        }
    }
}
