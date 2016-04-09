using System.Data.Entity.ModelConfiguration;
using IDAL.Models;

namespace DAL.Configurations
{
    internal class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            ToTable("Employees");

            Property(x => x.EmployerId)
                .HasColumnName("EmployerId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            HasRequired(x => x.Employer)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.EmployerId);
        }
    }
}