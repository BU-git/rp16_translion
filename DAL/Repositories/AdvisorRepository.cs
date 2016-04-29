using IDAL.Interfaces.IRepositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class AdvisorRepository : Repository<Advisor>, IAdvisorRepository
    {
        internal AdvisorRepository(ApplicationDbContext context) :
            base(context)
        {
        }
    }
}