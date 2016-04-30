using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces.IRepositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        internal UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public Task<User> FindByEmail(string email)
        {
            return Set.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<User> FindByEmail(CancellationToken cancellationToken, string email)
        {
            return Set.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public Task<User> FindByUserName(string username)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public Task<User> FindByUserName(CancellationToken cancellationToken, string username)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }

        public async void AddEmployer(Employer employer)
        {
            User user = await FindById(employer.EmployerId);
            user.Employer = employer;
            Update(user);
        }

        public async void AddAdmin(Admin admin)
        {
            User user = await FindById(admin.AdminId);
            user.Admin = admin;
            Update(user);
        }

        public async void AddAdvisor(Advisor advisor)
        {
            User user = await FindById(advisor.AdvisorId);
            user.Advisor = advisor;
            Update(user);
        }


        public async void AddEmployer(Employer employer, object userId)
        {
            var user = await Set.FindAsync(userId);
            user.Employer = employer;
            Update(user);
        }

        public async void AddAdmin(Admin admin, object userId)
        {
            var user = await Set.FindAsync(userId);
            user.Admin = admin;
            Update(user);
        }

        public async void AddAdvisor(Advisor advisor, object userId)
        {
            var user = await Set.FindAsync(userId);
            user.Advisor = advisor;
            Update(user);
        }
    }
}