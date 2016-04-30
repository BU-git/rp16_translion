using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IRepositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        #region Methods

        Task<Role> FindByName(string roleName);
        Task<Role> FindByName(CancellationToken cancellationToken, string roleName);

        #endregion
    }
}