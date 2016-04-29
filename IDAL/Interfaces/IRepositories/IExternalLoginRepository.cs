using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IRepositories
{
    public interface IExternalLoginRepository : IRepository<ExternalLogin>
    {
        #region Methods

        Task<ExternalLogin> GetByProviderAndKey(string loginProvider, string providerKey);

        Task<ExternalLogin> GetByProviderAndKey(CancellationToken cancellationToken, string loginProvider,
            string providerKey);

        #endregion
    }
}