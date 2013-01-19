using System.Data.Entity;
using dal.DomainClasses;
using dal.Mappings;

namespace dal.Contexts
{
    public class CustomerContext : DbContext
    {
        static CustomerContext()
        {
            Database.SetInitializer<CustomerContext>(null);
        }

		public CustomerContext()
			: base("Name=EnterpriseDatabase")
		{
		}

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomerMap());
        }
    }
}
