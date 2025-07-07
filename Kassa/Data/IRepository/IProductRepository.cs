namespace Kassa.Data.IRepository
{
    public interface IProductRepository
    {
        List<Product> GetProducts();
        Product GetProductById(int productId);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(int id);
        public List<Product> FilterProductByFrisdrank();
        public List<Product> FilterProductBySnack();
        public List<Product> FilterProductByAlcoholisch();
        public List<Product> FilterProductByWarmeDrank();
        public List<Product> FilterProductenPrijsOplopend();
        public List<Product> FilterProductenPrijsAflopend();
    }
}
