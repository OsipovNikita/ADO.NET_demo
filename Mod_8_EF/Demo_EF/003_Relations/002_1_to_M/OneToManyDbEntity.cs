using System.Data.Entity;

namespace _002_1_to_M
{
    public class OneToManyDbEntity : DbContext
    {
        public OneToManyDbEntity()
            : base("name=OneToManyDbEntity")
        {
        }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }

    }
}