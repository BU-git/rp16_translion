using System.Data.Entity.ModelConfiguration;
using Domain.Models;

namespace DAL.Configurations
{
    public class EmployerConfiguration : EntityTypeConfiguration<Employer>
    {
        public EmployerConfiguration()
        {
            ToTable("Employers");
        }
    }
}