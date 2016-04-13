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
        
        Task AddEmployerAsync(Employer employer, object userId);
        Task AddEmployerAsync(Employer employer, string userName);

        Task AddAdminAsync(Admin admin, object userId);
        Task AddAdminAsync(Admin admin, string userName);

        Task AddAdvisorAsync(Advisor advisor, object userId);
        Task AddAdvisorAsync(Advisor advisor, string userName);

        #endregion
    }
}