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
            return await Set.Where(a => !a.AlertIsDeleted).ToListAsync();
        }

        public async Task<List<Alert>> GetNewAlerts(CancellationToken cancellationToken)
        {
            return await Set.Where(a => !a.AlertIsDeleted).ToListAsync(cancellationToken);
        }


        public async Task<List<Alert>> GetAdvisorAlerts(Guid userId)
        {
            List<Alert> adminAlerts = await Set.Where(a => a.User.Admin != null).ToListAsync();
            List<Alert> advisorAlerts = await Set.Where(a => a.UserId == userId).ToListAsync();
            advisorAlerts.AddRange(adminAlerts);
            return advisorAlerts;
        }

        public async Task<List<Alert>> GetAdvisorAlerts(CancellationToken cancellationToken, Guid userId)
        {
            List<Alert> adminAlerts = await Set.Where(a => a.User.Admin != null).ToListAsync(cancellationToken);
            List<Alert> advisorAlerts = await Set.Where(a => a.UserId == userId).ToListAsync(cancellationToken);
            advisorAlerts.AddRange(adminAlerts);
            return advisorAlerts;
        }

        public Task<Alert> FindAlertById(Guid id)
        {
            return Set.FirstOrDefaultAsync(a => a.AlertId == id);
        }
    }
}