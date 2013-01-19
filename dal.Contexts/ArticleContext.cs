using System.Data.Entity;
using dal.DomainClasses;
using dal.Mappings;

namespace dal.Contexts
{
    public class ArticleContext : DbContext
    {
        static ArticleContext()
        {
            Database.SetInitializer<ArticleContext>(null);
        }

		public ArticleContext()
            : base("Name=EnterpriseDatabase")
		{
		}

        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ArticleMap());
        }
    }
}
