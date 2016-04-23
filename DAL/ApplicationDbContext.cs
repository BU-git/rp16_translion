using System.Data.Entity;
using System.Data.Entity.SqlServer;
using DAL.Configurations;
using IDAL.Models;

namespace DAL
{
    internal class ApplicationDbContext : DbContext
    {
        internal ApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            // ROLA - This is a hack to ensure that Entity Framework SQL Provider is copied across to the output folder.
            // As it is installed in the GAC, Copy Local does not work. It is required for probing.
            // Fixed "Provider not loaded" error
            Database.SetInitializer(new DBInitializer());
            var ensureDLLIsCopied = SqlProviderServices.Instance;
        }

        internal IDbSet<User> Users { get; set; }
        internal IDbSet<Role> Roles { get; set; }
        internal IDbSet<ExternalLogin> Logins { get; set; }
        internal IDbSet<Employer> Employers { get; set; }
        internal IDbSet<Admin> Admins { get; set; }
        internal IDbSet<Advisor> Advisors { get; set; }
        internal IDbSet<Page> Pages { get; set; }
        internal IDbSet<Question> Questions { get; set; }
        internal IDbSet<Answer> Answers { get; set; }
           
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClaimConfiguration());
            modelBuilder.Configurations.Add(new ExternalLoginConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new EmployerConfiguration());
            modelBuilder.Configurations.Add(new AdminConfiguration());
            modelBuilder.Configurations.Add(new AdvisorConfiguration());
            modelBuilder.Configurations.Add(new PageConfiguration());
            modelBuilder.Configurations.Add(new QuestionConfiguration());
            modelBuilder.Configurations.Add(new AnswerConfiguration());
        }
    }
}