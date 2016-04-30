using System;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        #region Methods

        Task<User> FindByUserName(string username);
        Task<User> FindByUserName(CancellationToken cancellationToken, string username);

        Task<User> FindByEmail(string email);
        Task<User> FindByEmail(CancellationToken cancellationToken, string email);

        void AddEmployer(Employer employer);

        void AddAdmin(Admin admin);

        void AddAdvisor(Advisor advisor);

        #endregion
    }
}