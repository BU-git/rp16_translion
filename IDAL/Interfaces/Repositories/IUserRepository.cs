using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        #region Methods

        User FindByUserName(string username);
        Task<User> FindByUserNameAsync(string username);
        Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username);
        User FindByEmail(string email);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByEmailAsync(CancellationToken cancellationToken, string email);

        void AddEmployerAsync(Employer employer, object userId);
        void AddEmployerAsync(Employer employer, string userName);

        void AddAdminAsync(Admin admin, object userId);
        void AddAdminAsync(Admin admin, string userName);

        void AddAdvisorAsync(Advisor advisor, object userId);
        void AddAdvisorAsync(Advisor advisor, string userName);

        #endregion
    }
}