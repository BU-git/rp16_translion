using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.Repositories
{
    public interface IAlertRepository: IRepository<Alert>
    {
        List<Alert> GetNewAlerts();
        Task<List<Alert>> GetNewAlertsAsync();
        Task<List<Alert>> GetNewAlertsAsync(CancellationToken cancellationToken);
        Alert FindAlertById(Guid? id);
        //void AddAlert(Alert alert);
    }
}
