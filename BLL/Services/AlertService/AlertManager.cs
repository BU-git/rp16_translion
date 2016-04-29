using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces;
using IDAL.Interfaces.IManagers;
using IDAL.Models;

namespace BLL.Services.AlertService
{
    public class AlertManager: IAlertManager
    {
        public AlertManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork _unitOfWork { get; }

        #region Get with concrete status alerts

        public async Task<List<Alert>> GetNew()
        {
            return await _unitOfWork.AlertRepository.GetNewAlerts();
        }

        public async Task<List<Alert>> GetNew(CancellationToken cancellationToken)
        {
            return await _unitOfWork.AlertRepository.GetNewAlerts(cancellationToken);
        }
        #endregion

        #region Find employee

        public Employee FindEmployee(Alert alert)
        {
            Employee employee = alert.Employees.FirstOrDefault(e => e.EmployerId == alert.AlertEmployerId);
            return employee;
        }

        #endregion

        #region Create

        public void Create(Alert alert)
        {
            if (alert == null)
            {
                throw new ArgumentException("alert is null. Wrong parameter");
            }

            _unitOfWork.AlertRepository.Add(alert);
            _unitOfWork.SaveChanges();
        }

        public async Task<int> CreateAsync(Alert alert)
        {
            if (alert == null)
            {
                throw new ArgumentException("alert is null. Wrong parameter");
            }

            _unitOfWork.AlertRepository.Add(alert);
            return await _unitOfWork.SaveChanges();
        }

        public async Task<int> CreateAsync(CancellationToken cancellationToken, Alert alert)
        {
            if (alert == null)
            {
                throw new ArgumentException("alert is null. Wrong parameter");
            }

            _unitOfWork.AlertRepository.Add(alert);
            return await _unitOfWork.SaveChanges(cancellationToken);

        }
        #endregion

        #region Comment

        //public void Comment(Alert alert, User user)
        //{

        //}

        //public async Task<int> CommentAsync(Alert alert, User user)
        //{

        //}
        //public async Task<int> CommentAsync(CancellationToken cancellationToken, Alert alert, User user)
        //{

        //}

        #endregion

        #region Approve

        public void Approve(Alert alert)
        {
            if (alert.AlertType==AlertType.Employee_Add)
            {
                Employee employee = this.FindEmployee(alert);
                employee.IsApprove = true;
                _unitOfWork.EmployeeRepository.Update(employee);
            }
            alert.AlertIsDeleted = true;
            _unitOfWork.AlertRepository.Update(alert);
            _unitOfWork.SaveChanges();
        }

        //public async Task<int> ApproveAsync(Alert alert, User user)
        //{

        //}

        //public async Task<int> ApproveAsync(CancellationToken cancellationToken, Alert alert, User user)
        //{

        //}

        #endregion

        #region Clean all

        //public void Clean()
        //{

        //}

        //public async Task<int> CleanAsync()
        //{

        //}

        //public async Task<int> CleanAsync(CancellationToken cancellationToken)
        //{

        //}

        #endregion

        #region GetAlert
        public Alert GetAlert(Guid alertid)
        {
            if (alertid == null)
            {
                throw new ArgumentException("alert is null. Wrong parameters");
            }
            return  _unitOfWork.AlertRepository.FindAlertById(alertid).Result;
        }

        public  async Task<Alert> GetAlertAsync(Guid alertid)
        {
            if (alertid == null)
            {
                throw new ArgumentException("alert is null. Wrong parameters");
            }
            return await _unitOfWork.AlertRepository.FindById(alertid);
        }

        public async Task<Alert> GetAsyncAsync(CancellationToken cancellationToken, Guid alertid)
        {
            if (alertid == null)
            {
                throw new ArgumentException("alert is null. Wrong parameters");
            }
            return await _unitOfWork.AlertRepository.FindById(cancellationToken,alertid);
        }
#endregion

    }
}
