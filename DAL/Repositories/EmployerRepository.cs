using Domain.Models;
using IDAL.Interfaces;

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