using System.Data.Entity;

namespace _001_1_to_1
{
    public class OneToOneDbEntity : DbContext
    {
        public OneToOneDbEntity()
            : base("name=OneToOneDbEntity")
        {
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

    }
}