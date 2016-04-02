using System.Data.Entity.ModelConfiguration;
using IDAL.Models;

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