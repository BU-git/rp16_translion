using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IManagers
{
    public interface IAlertManager
    {
        #region Get all members

        //List<Alert> GetAll();
        //Task<List<Alert>> GetAllAsync();
        //Task<List<Alert>> GetAllAsync(CancellationToken cancellationToken);

        #endregion

        #region Get with concrete status alerts
        Task<List<Alert>> GetNew();
        Task<List<Alert>> GetNew(CancellationToken cancellationToken);
        #endregion

        #region Find employee

        Task<Employee> FindEmployeeAsync(Alert alert);

        #endregion

        #region Find employer

        Task<Employer> FindEmployerAsync(Alert alert);

        #endregion

        #region Create

        void Create(Alert alert);
        Task<int> CreateAsync(Alert alert);
        Task<int> CreateAsync(CancellationToken cancellationToken, Alert alert);

        #endregion

        #region Comment

        //void Comment(Alert alert, User user);
        //Task<int> CommentAsync(Alert alert, User user);
        //Task<int> CommentAsync(CancellationToken cancellationToken, Alert alert, User user);

        #endregion

        #region Approve

        Task<int> Approve(Alert alert);
        //Task<int> ApproveAsync(Alert alert, User user);
        //Task<int> ApproveAsync(CancellationToken cancellationToken, Alert alert, User user);

        #endregion

        #region Clean all

        //void Clean();
        //Task<int> CleanAsync();
        //Task<int> CleanAsync(CancellationToken cancellationToken);

        #endregion

        #region Get Alert

        Alert GetAlert(Guid alertid);
        Task<Alert> GetAlertAsync(Guid alertid);
        Task<Alert> GetAlertAsync(CancellationToken cancellationToken, Guid alertid);
        Task<List<Alert>> GetAdvisorAlerts(Guid userId);
        Task<List<Alert>> GetAdvisorAlerts(CancellationToken cancellationToken, Guid userId);

        #endregion
    }
}
