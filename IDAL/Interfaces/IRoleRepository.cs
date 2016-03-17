using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        #region Methods

        Role FindByName(string roleName);
        Task<Role> FindByNameAsync(string roleName);
        Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName);

        #endregion
    }
}