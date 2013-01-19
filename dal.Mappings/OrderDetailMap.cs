using System.Data.Entity.ModelConfiguration;
using dal.DomainClasses;

namespace dal.Mappings
{
    public class OrderDetailMap : EntityTypeConfiguration<OrderDetail>
    {
        public OrderDetailMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("OrderDetail");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.OrderId).HasColumnName("OrderId");
            Property(t => t.ArticleId).HasColumnName("ArticleId");
            Property(t => t.Count).HasColumnName("Count");

            // Relationships
            HasRequired(t => t.Article)
                .WithMany(t => t.OrderDetails)
                .HasForeignKey(d => d.ArticleId);
            HasRequired(t => t.Order)
                .WithMany(t => t.OrderDetails)
                .HasForeignKey(d => d.OrderId);

        }
    }
}
