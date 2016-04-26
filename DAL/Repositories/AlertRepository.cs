using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces.Repositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class AlertRepository: Repository<Alert>, IAlertRepository
    {
        internal AlertRepository(ApplicationDbContext context) :
            base(context)
        {
            
        }
        public  List<Alert> GetNewAlerts()
        {
            return Set.Where(a => !a.AlertIsDeleted).Include(x => x.Employees).ToList();
        }
        
        public async Task<List<Alert>> GetNewAlertsAsync()
        {
            return await Set.Where(a => !a.AlertIsDeleted).Include(x => x.Employees).ToListAsync();
        }

        public async Task<List<Alert>> GetNewAlertsAsync(CancellationToken cancellationToken)
        {
            return await Set.Where(a => !a.AlertIsDeleted).Include(x => x.Employees).ToListAsync(cancellationToken);
        }

        //public void AddAlert(Alert alert)
        //{
        //    Set.Add(alert);
        //}

        public Alert FindAlertById(Guid? id)
        {
            return Set.Include(a=>a.Employees).FirstOrDefault(a=>a.AlertId==id);
        }

    }
}
