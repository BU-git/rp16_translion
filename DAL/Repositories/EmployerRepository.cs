using IDAL.Interfaces;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class EmployerRepository : Repository<Employer>, IEmployerRepository
    {
        internal EmployerRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}