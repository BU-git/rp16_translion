using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces.IRepositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class ExternalLoginRepository : Repository<ExternalLogin>, IExternalLoginRepository
    {
        internal ExternalLoginRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public Task<ExternalLogin> GetByProviderAndKey(string loginProvider, string providerKey)
        {
            return Set.FirstOrDefaultAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
        }

        public Task<ExternalLogin> GetByProviderAndKey(CancellationToken cancellationToken, string loginProvider,
            string providerKey)
        {
            return Set.FirstOrDefaultAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey,
                cancellationToken);
        }
    }
}