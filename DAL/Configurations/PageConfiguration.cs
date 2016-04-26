using System.Data.Entity.ModelConfiguration;
using IDAL.Models;

namespace DAL.Configurations
{
    internal class PageConfiguration : EntityTypeConfiguration<Page>
    {
        public PageConfiguration()
        {
            HasKey(p => p.Id);

            Property(p => p.Name)
                .HasMaxLength(20)
                .IsRequired();

            HasMany(p => p.Questions)
                .WithRequired(q => q.Page)
                .HasForeignKey(q => q.PageId)
                .WillCascadeOnDelete();

            Property(p => p.Order)
                .IsRequired();
        }
    }
}
