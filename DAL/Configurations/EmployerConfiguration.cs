using System.Data.Entity.ModelConfiguration;
using IDAL.Models;

namespace DAL.Configurations
{
    public class EmployerConfiguration : EntityTypeConfiguration<Employer>
    {
        public EmployerConfiguration()
        {
            ToTable("Employers");

            HasKey(x => x.EmployerId);

            HasMany(x => x.Employees)
                .WithRequired(x => x.Employer)
                .HasForeignKey(x => x.EmployerId);

            //HasMany(x => x.Alerts)
            //    .WithRequired(x => x.Employer)
            //    .HasForeignKey(x => x.AlertEmployerId);
        }
    }
}