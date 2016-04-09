using System.Data.Entity.ModelConfiguration;
using IDAL.Models;

namespace DAL.Configurations
{
    public class AdvisorConfiguration : EntityTypeConfiguration<Advisor>
    {
        public AdvisorConfiguration()
        {
            ToTable("Advisor");

            HasKey(x => x.AdvisorId);
        }
    }
}