using System.Data.Entity.ModelConfiguration;
using Domain.Models;

namespace DAL.Configurations
{
    public class AdminConfiguration : EntityTypeConfiguration<Admin>
    {
        public AdminConfiguration()
        {
            ToTable("Admin");
        }
    }
}