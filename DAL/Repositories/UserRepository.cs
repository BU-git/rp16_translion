using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using IDAL.Interfaces;

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
            //TODO Need to be implemented
            throw new NotImplementedException();
        }

        public Task<User> FindByEmailAsync(string email)
        {
            //TODO Need to be implemented
            throw new NotImplementedException();
        }

        public Task<User> FindByEmailAsync(CancellationToken cancellationToken, string email)
        {
            //TODO Need to be implemented
            throw new NotImplementedException();
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