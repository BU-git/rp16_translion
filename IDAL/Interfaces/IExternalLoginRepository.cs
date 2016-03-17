using System.Threading;
using System.Threading.Tasks;
using Domain.Models;

namespace IDAL.Interfaces
{
    public interface IExternalLoginRepository : IRepository<ExternalLogin>
    {
        #region Methods

        ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey);
        Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey);

        Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider,
            string providerKey);

        #endregion
    }
}