namespace Kassa.Data.Repository
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public List<Product> GetProducts()
        {
            string sql = @"SELECT p.*, pt.*
                                    FROM product p
                                    INNER JOIN producttype pt ON p.producttypeid = pt.id";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var products = db.Query<Product, Producttype, Product>(
                    sql,
                    (Product, Producttype) =>
                    {
                        Product.Producttype = Producttype;
                        return Product;
                    }, splitOn: "Id");
                return products.ToList();
            }
        }

        public Product GetProductById(int productId)
        {
            string sql = @"SELECT * FROM product WHERE Id = @Id";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var product = db.Query<Product>(sql, new { Id = productId }).FirstOrDefault();
                return product;
            }
        }

        public bool CreateProduct(Product product)
        {
            string sql = @"INSERT INTO product (producttypeId, naam, prijs, afbeelding, beschrijving, aantal) VALUES (@producttypeId, @naam, @prijs, @afbeelding, @beschrijving, @aantal)";

            var paramater = new
            {
                producttypeId = product.Producttype.Id,
                naam = product.Naam,
                prijs = product.Prijs,
                afbeelding = product.Afbeelding,
                beschrijving = product.Beschrijving,
                aantal = product.Aantal
            };

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, paramater);
            return affectedRows == 1;
        }

        public bool UpdateProduct(Product product)
        {
            string sql = @"UPDATE product SET producttypeId = @producttypeId, naam = @naam, prijs = @prijs, afbeelding = @afbeelding, beschrijving = @beschrijving, aantal = @aantal WHERE id = @Id";

            var parameters = new
            {
                producttypeId = product.Producttype.Id,
                naam = product.Naam,
                prijs = product.Prijs,
                afbeelding = product.Afbeelding,
                beschrijving = product.Beschrijving,
                aantal = product.Aantal,
                id = product.Id
            };

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, parameters);
            return affectedRows == 1;
        }

        public bool DeleteProduct(int id)
        {
            string sql = @"DELETE FROM product WHERE id = @Id";

            using IDbConnection db = new MySqlConnection(ConnectionString);
            var affectedRows = db.Execute(sql, new { Id = id });
            return affectedRows >= 1;
        }

        public List<Product> FilterProductByFrisdrank()
        {
            string sql = @"SELECT p.*, pt.* 
                           FROM startspelercompanion.product as p 
                           INNER JOIN producttype pt ON p.producttypeid = pt.id 
                           WHERE pt.Naam = 'Frisdrank'";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var products = db.Query<Product, Producttype, Product>(
                    sql,
                    (Product, Producttype) =>
                    {
                        Product.Producttype = Producttype;
                        return Product;
                    }, splitOn: "Id");
                return products.ToList();
            }
        }

        public List<Product> FilterProductBySnack()
        {
            string sql = @"SELECT p.*, pt.* 
                           FROM startspelercompanion.product as p 
                           INNER JOIN producttype pt ON p.producttypeid = pt.id 
                           WHERE pt.Naam = 'Snack'";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var products = db.Query<Product, Producttype, Product>(
                    sql,
                    (Product, Producttype) =>
                    {
                        Product.Producttype = Producttype;
                        return Product;
                    }, splitOn: "Id");
                return products.ToList();
            }
        }

        public List<Product> FilterProductByAlcoholisch()
        {
            string sql = @"SELECT p.*, pt.* 
                           FROM startspelercompanion.product as p 
                           INNER JOIN producttype pt ON p.producttypeid = pt.id 
                           WHERE pt.Naam = 'Alcoholische drank'";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var products = db.Query<Product, Producttype, Product>(
                    sql,
                    (Product, Producttype) =>
                    {
                        Product.Producttype = Producttype;
                        return Product;
                    }, splitOn: "Id");
                return products.ToList();
            }
        }

        public List<Product> FilterProductByWarmeDrank()
        {
            string sql = @"SELECT p.*, pt.* 
                           FROM startspelercompanion.product as p 
                           INNER JOIN producttype pt ON p.producttypeid = pt.id 
                           WHERE pt.Naam = 'Warme drank'";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var products = db.Query<Product, Producttype, Product>(
                    sql,
                    (Product, Producttype) =>
                    {
                        Product.Producttype = Producttype;
                        return Product;
                    }, splitOn: "Id");
                return products.ToList();
            }
        }

        public List<Product> FilterProductenPrijsOplopend()
        {
            string sql = @"SELECT p.*, pt.* 
                           FROM startspelercompanion.product as p 
                           INNER JOIN producttype pt ON p.producttypeid = pt.id 
                           ORDER BY p.Prijs ASC";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var products = db.Query<Product, Producttype, Product>(
                    sql,
                    (Product, Producttype) =>
                    {
                        Product.Producttype = Producttype;
                        return Product;
                    }, splitOn: "Id");
                return products.ToList();
            }
        }

        public List<Product> FilterProductenPrijsAflopend()
        {
            string sql = @"SELECT p.*, pt.* 
                           FROM startspelercompanion.product as p 
                           INNER JOIN producttype pt ON p.producttypeid = pt.id 
                           ORDER BY p.Prijs DESC";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var products = db.Query<Product, Producttype, Product>(
                    sql,
                    (Product, Producttype) =>
                    {
                        Product.Producttype = Producttype;
                        return Product;
                    }, splitOn: "Id");
                return products.ToList();
            }
        }
    }
}
