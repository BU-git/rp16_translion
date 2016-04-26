using System.Data.Entity.ModelConfiguration;
using IDAL.Models;

namespace DAL.Configurations
{
    internal class AnswerConfiguration : EntityTypeConfiguration<Answer>
    {
        public AnswerConfiguration()
        {
            HasKey(a => a.Id);

            Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(300);
        }
    }
}
