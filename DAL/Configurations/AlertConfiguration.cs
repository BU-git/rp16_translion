using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using IDAL.Models;

namespace DAL.Configurations
{
    class AlertConfiguration: EntityTypeConfiguration<Alert>
    {
        public AlertConfiguration()
        {
            ToTable("Alert");

            #region Fields configuration

            HasKey(x => x.AlertId)
                .Property(x => x.AlertId)
                .HasColumnName("AlertId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(x => x.AlertComment)
                .HasColumnName("AlertComment")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(x => x.AlertIsDeleted)
                .HasColumnName("IsDeleted")
                .HasColumnType("bit")
                .IsRequired();

            Property(x => x.AlertCreateTS)
                .HasColumnName("AlertCreateTS")
                .HasColumnType("datetime2")
                .IsRequired();

            Property(x => x.AlertUpdateTS)
                .HasColumnName("AlertUpdateTS")
                .HasColumnType("datetime2")
                .IsRequired();

            #endregion

            #region Relationship configuration

            //HasRequired(x => x.Employer)
            //    .WithMany(x => x.Alerts)
            //    .HasForeignKey(x => x.AlertEmployerId);

            //HasMany(x => x.Employees)
            //    .WithMany(x => x.Alerts);


            #endregion
        }
    }
}