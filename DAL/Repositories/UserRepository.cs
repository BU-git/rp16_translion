using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces;
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

        public User FindByUserName(string username)
        {
            return Set.FirstOrDefault(x => x.UserName == username);
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