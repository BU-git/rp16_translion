using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.Managers
{
    public interface IAlertManager
    {
        #region Get all members

        //List<Alert> GetAll();
        //Task<List<Alert>> GetAllAsync();
        //Task<List<Alert>> GetAllAsync(CancellationToken cancellationToken);

        #endregion

        #region Get concrete type alert

        //List<Alert> Get(AlertType alertType);
        //Task<Alert> GetAsync(AlertType alertType);
        //Task<Alert> GetAsync(CancellationToken cancellationToken, AlertType alertType);

        #endregion

        #region Get with concrete status alerts
        List<Alert> GetNew();
        Task<List<Alert>> GetNewAsync();
        Task<List<Alert>> GetNewAsync(CancellationToken cancellationToken);
        #endregion

        #region Find employee

        Employee FindEmployee(Alert alert);

        #endregion

        #region Create

        void Create(Alert alert);
        //Task<int> CreateAsync(Employer employer, Employee employee, AlertType alertType);
        //Task<int> CreateAsync(CancellationToken cancellationToken, Employer employer, Employee employee, AlertType alertType);

        #endregion

        #region Comment

        //void Comment(Alert alert, User user);
        //Task<int> CommentAsync(Alert alert, User user);
        //Task<int> CommentAsync(CancellationToken cancellationToken, Alert alert, User user);

        #endregion

        #region Approve

        void Approve(Alert alert);
        //Task<int> ApproveAsync(Alert alert, User user);
        //Task<int> ApproveAsync(CancellationToken cancellationToken, Alert alert, User user);

        #endregion

        #region Clean all

        //void Clean();
        //Task<int> CleanAsync();
        //Task<int> CleanAsync(CancellationToken cancellationToken);

        #endregion

        #region Get Alert

        Alert GetAlert(Guid? alertid);
        Task<Alert> GetAlertAsync(Guid? alertid);
        Task<Alert> GetAsyncAsync(CancellationToken cancellationToken, Guid? alertid);

        #endregion
    }
}
