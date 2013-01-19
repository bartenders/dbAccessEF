using System.Data.Entity.ModelConfiguration;
using dal.DomainClasses;

namespace dal.Mappings
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FistName)
                .HasMaxLength(50);

            this.Property(t => t.LastName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Customer");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FistName).HasColumnName("FistName");
            this.Property(t => t.LastName).HasColumnName("LastName");
        }
    }
}
