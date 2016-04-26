using System.Data.Entity.ModelConfiguration;
using IDAL.Models;

namespace DAL.Configurations
{
    internal class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            ToTable("Employees");

            HasKey(x => x.EmployeeId);

            //Property(x => x.EmployerId)
            //    .HasColumnName("EmployerId")
            //    .HasColumnType("uniqueidentifier")
            //    .IsRequired();

            HasRequired(x => x.Employer)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.EmployerId);

            HasMany(x => x.Alerts)
                .WithMany(x => x.Employees);


            //HasMany(x => x.Employer.Alerts)
            //    .WithOptional(x => x.Employee)
            //.Map(x =>
            //{
            //    x.ToTable("AlertEmployee").MapKey("EmployeeId");

            //});

        }
    }
}