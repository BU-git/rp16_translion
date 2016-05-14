using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IRepositories
{
    public interface IAlertRepository : IRepository<Alert>
    {
        Task<List<Alert>> GetNewAlerts();
        Task<List<Alert>> GetNewAlerts(CancellationToken cancellationToken);
        Task<Alert> FindAlertById(Guid alertId);
        Task<List<Alert>> GetAdvisorAlerts(Guid userId);
        Task<List<Alert>> GetAdvisorAlerts(CancellationToken cancellationToken, Guid userId);
    }
}