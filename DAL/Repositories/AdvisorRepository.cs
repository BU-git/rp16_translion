using IDAL.Interfaces.Repositories;
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