using System.Data.Entity;

namespace _002_Operations
{
    public class ProductDB : DbContext
    {
        public ProductDB()
            : base("name=ProductDB")
        {
        }
        public virtual DbSet<Product> Products { get; set; }
    }
}