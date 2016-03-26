using Domain.Models;
using IDAL.Interfaces;

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