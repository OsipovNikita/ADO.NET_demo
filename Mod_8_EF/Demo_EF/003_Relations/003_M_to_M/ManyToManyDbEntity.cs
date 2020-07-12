using System.Data.Entity;

namespace _003_M_to_M
{
    public class ManyToManyDbEntity : DbContext
    {
        public ManyToManyDbEntity()
            : base("name=ManyToManyDbEntity")
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
    }
}