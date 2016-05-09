using System.Data.Entity.ModelConfiguration;
using IDAL.Models;

namespace DAL.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("User");

            #region Fields configuration

            HasKey(x => x.UserId)
                .Property(x => x.UserId)
                .HasColumnName("UserId")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(x => x.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(x => x.SecurityStamp)
                .HasColumnName("SecurityStamp")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(x => x.UserName)
                .HasColumnName("UserName")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            #endregion

            #region Relationship configuration

            HasOptional(x => x.Admin)
                .WithRequired(z => z.User);

            HasOptional(x => x.Advisor)
                .WithRequired(z => z.User);

            HasOptional(x => x.Employer)
                .WithRequired(z => z.User)
                .WillCascadeOnDelete(true);

            HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .Map(x =>
                {
                    x.ToTable("UserRole");
                    x.MapLeftKey("UserId");
                    x.MapRightKey("RoleId");
                });

            HasMany(x => x.Claims)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);

            HasMany(x => x.Logins)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);

            //HasMany(x => x.Alerts)
            //    .WithRequired(x => x.User);

            #endregion
        }
    }
}