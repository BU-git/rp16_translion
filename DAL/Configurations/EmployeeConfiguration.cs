using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.Models;

namespace DAL.Configurations
{
    class EmployeeConfiguration : EntityTypeConfiguration<Employee>
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
