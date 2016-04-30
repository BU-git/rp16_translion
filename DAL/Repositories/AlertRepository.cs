using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces.IRepositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class AlertRepository : Repository<Alert>, IAlertRepository
    {
        internal AlertRepository(ApplicationDbContext context) :
            base(context)
        {
        }

        public async Task<List<Alert>> GetNewAlerts()
        {
            return await Set.Where(a => !a.AlertIsDeleted).Include(x => x.Employees).ToListAsync();
        }

        public async Task<List<Alert>> GetNewAlerts(CancellationToken cancellationToken)
        {
            return await Set.Where(a => !a.AlertIsDeleted).Include(x => x.Employees).ToListAsync(cancellationToken);
        }

        public Task<Alert> FindAlertById(Guid id)
        {
            return Set.Include(a => a.Employees).FirstOrDefaultAsync(a => a.AlertId == id);
        }
    }
}