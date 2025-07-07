namespace Kassa.Data.Repository
{
    public class ProducttypeRepository : BaseRepository, IProducttypeRepository
    {
        public IEnumerable<Producttype> GetProducttypes()
        {
            string sql = @"SELECT * FROM producttype";

            using (IDbConnection db = new MySqlConnection(ConnectionString))
            {
                var producttypes = db.Query<Producttype>(sql);
                return producttypes;
            }
        }
    }
}
