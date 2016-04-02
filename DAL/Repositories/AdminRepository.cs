using IDAL.Interfaces;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class AdminRepository : Repository<Admin>, IAdminRepository
    {
        internal AdminRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}