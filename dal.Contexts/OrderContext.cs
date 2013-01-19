using System.Data.Entity;
using dal.DomainClasses;
using dal.Mappings;

namespace dal.Contexts
{
    public class OrderContext : DbContext
    {
        static OrderContext()
        {
            Database.SetInitializer<OrderContext>(null);
        }

		public OrderContext()
            : base("Name=EnterpriseDatabase")
		{
		}

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
        }
    }
}
