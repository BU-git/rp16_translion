using System.Data.Entity.ModelConfiguration;
using IDAL.Models;

namespace DAL.Configurations
{
    internal class QuestionConfiguration : EntityTypeConfiguration<Question>
    {
        public QuestionConfiguration()
        {
            HasKey(q => q.Id);

            Property(q => q.AnswersCount)
                .IsOptional();

            Property(q => q.QuestionName)
                .HasMaxLength(300)
                .IsRequired();

            Property(q => q.TypeAnswer)
                .IsRequired()
                .HasMaxLength(20);

            HasMany(q => q.Answers)
                .WithRequired(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .WillCascadeOnDelete();
        }
    }
}
