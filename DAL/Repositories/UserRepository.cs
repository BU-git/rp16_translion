using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces.Repositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        internal UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public User FindByEmail(string email)
        {
            return Set.FirstOrDefault(u => u.Email == email);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return Set.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<User> FindByEmailAsync(CancellationToken cancellationToken, string email)
        {
            return Set.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task AddEmployerAsync(Employer employer, object userId)
        {
            var user = await Set.FindAsync(userId);
            user.Employer = employer;
            Update(user);
        }

        public async Task AddEmployerAsync(Employer employer, string userName)
        {
            var user = await FindByUserNameAsync(userName);
            user.Employer = employer;
            Update(user);
        }

        public async Task AddAdminAsync(Admin admin, object userId)
        {
            var user = await Set.FindAsync(userId);
            user.Admin = admin;
            Update(user);
        }

        public async Task AddAdminAsync(Admin admin, string userName)
        {
            var user = await FindByUserNameAsync(userName);
            user.Admin = admin;
            Update(user);
        }

        public async Task AddAdvisorAsync(Advisor advisor, object userId)
        {
            var user = await Set.FindAsync(userId);
            user.Advisor = advisor;
            Update(user);
        }

        public async Task AddAdvisorAsync(Advisor advisor, string userName)
        {
            var user = await FindByUserNameAsync(userName);
            user.Advisor = advisor;
            Update(user);
        }

        public User FindByUserName(string username)
        {
            return  Set.FirstOrDefault(x => x.UserName == username);
        }

        public Task<User> FindByUserNameAsync(string username)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }
    }
}