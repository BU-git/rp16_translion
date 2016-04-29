using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDAL;
using IDAL.Interfaces;
using IDAL.Interfaces.IManagers;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public abstract class PersonManager<TEntity> : IPersonageManager<TEntity> where TEntity : class
    {
        public PersonManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; }
        public abstract Task<WorkResult> DeleteEmployee(Employee employee);
        
        #region Get User

        public async Task<User> GetBaseUserByName(string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            return await UnitOfWork.UserRepository.FindByUserName(userName);
        }

        public async Task<User> GetBaseUserByName(CancellationToken cancellationToken, string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            return await UnitOfWork.UserRepository.FindByUserName(cancellationToken, userName);
        }

        public async Task<User> GetBaseUserByGuid(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException($"User's id can't be {userId}");

            return await UnitOfWork.UserRepository.FindById(userId);
        }

        public async Task<User> GetBaseUserByGuid(CancellationToken cancellationToken, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException($"User's id can't be {userId}");

            return await UnitOfWork.UserRepository.FindById(cancellationToken, userId);
        }

        public async Task<User> GetBaseUserByGuid(string userId)
        {
            Guid userGuid;

            if (!Guid.TryParse(userId, out userGuid))
                throw new ArgumentException($"User's id can't be {userId}");

            return await GetBaseUserByGuid(userGuid);
        }

        public async Task<User> GetBaseUserByGuid(CancellationToken cancellationToken, string userId)
        {
            Guid userGuid;

            if (!Guid.TryParse(userId, out userGuid))
                throw new ArgumentException($"User's id can't be {userId}");

            return await GetBaseUserByGuid(cancellationToken, userGuid);
        }

        #endregion

        #region GetAllEmployees
        
        public async Task<List<Employee>> GetAllEmployees(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentException("User is null. Wrong parameter");
            }
            User user = await GetBaseUserByGuid(userId);
            return await UnitOfWork.EmployeeRepository.GetAllEmployees(user.UserId);
        }

        public async Task<List<Employee>> GetAllEmployees(CancellationToken cancellationToken, string userId)
        {
            if (userId == null)
            {
                throw new ArgumentException("User is null. Wrong parameter");
            }
            User user = await GetBaseUserByGuid(cancellationToken, userId);
            return await UnitOfWork.EmployeeRepository.GetAllEmployees(cancellationToken, user.UserId);
        }

        public async Task<List<Employee>> GetAllEmployees(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
            {
                throw new ArgumentException("User is null. Wrong parameter");
            }
            return await UnitOfWork.EmployeeRepository.GetAllEmployees(userId);
        }

        public async Task<List<Employee>> GetAllEmployees(CancellationToken cancellationToken, Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
            {
                throw new ArgumentException("User is null. Wrong parameter");
            }
            return await UnitOfWork.EmployeeRepository.GetAllEmployees(cancellationToken, userId);
        }
        #endregion

        #region GetEmployee

        public async Task<Employee> GetEmployee(Guid employeeId)
        {
            if (employeeId == null || employeeId == Guid.Empty)
            {
                throw new ArgumentException("employeeId is null. Wrong parameter");
            }
            return await UnitOfWork.EmployeeRepository.FindById(employeeId);
        }

        public async Task<Employee> GetEmployee(CancellationToken cancellationToken, Guid employeeId)
        {
            if (employeeId == null || employeeId == Guid.Empty)
            {
                throw new ArgumentException("employeeId is null. Wrong parameter");
            }
            return await UnitOfWork.EmployeeRepository.FindById(cancellationToken, employeeId);
        }

        #endregion

        #region CreateEmployee
        
        public async Task<WorkResult> CreateEmployee(Employee employee)
        {
            if (employee == null)
            {
                return WorkResult.Failed("employee or user is null. Wrong parameter");
            }
            UnitOfWork.EmployerRepository.AddEmployee(employee);
            await UnitOfWork.SaveChanges();
            
            return WorkResult.Success();
        }
        
        public async Task<WorkResult> CreateEmployee(CancellationToken cancellationToken, Employee employee)
        {
            if (employee == null)
            {
                return WorkResult.Failed("employee or user is null. Wrong parameter");
            }
            UnitOfWork.EmployerRepository.AddEmployee(employee);
            await UnitOfWork.SaveChanges(cancellationToken);
            return WorkResult.Success();
        }

        #endregion

        #region UpdateEmployee
        
        public async Task<WorkResult> UpdateEmployee(Employee employee)
        {
            if (employee == null)
            {
                return WorkResult.Failed("EmployeeId is null. Wrong parameter");
            }
            UnitOfWork.EmployeeRepository.Update(employee);
            await UnitOfWork.SaveChanges();
            return WorkResult.Success();
        }
        public async Task<WorkResult> UpdateEmployee(CancellationToken cancellationToken, Employee employee)
        {
            if (employee == null)
            {
                WorkResult.Failed("employeeId is null. Wrong parameter");
            }
            UnitOfWork.EmployeeRepository.Update(employee);
            await UnitOfWork.SaveChanges(cancellationToken);
            return WorkResult.Success();
        }
        
        #endregion
        
        public abstract Task<List<TEntity>> GetAll();
        public abstract Task<List<TEntity>> GetAll(CancellationToken cancellationToken);

        public abstract Task<TEntity> Get(Guid userId);
        public abstract Task<TEntity> Get(CancellationToken cancellationToken, Guid userId);
        public abstract Task<TEntity> Get(string userName);
        public abstract Task<TEntity> Get(CancellationToken cancellationToken, string userName);

        public abstract Task<WorkResult> Create(TEntity entity);
        public abstract Task<WorkResult> Create(CancellationToken cancellationToken, TEntity entity);

        public abstract Task<WorkResult> Update(TEntity entity);
        public abstract Task<WorkResult> Update(CancellationToken cancellationToken, TEntity entity);

        public abstract Task<WorkResult> Delete(Guid userId);
        public abstract Task<WorkResult> Delete(CancellationToken cancellationToken, Guid userId);
        public abstract Task<WorkResult> Delete(string userName);
        public abstract Task<WorkResult> Delete(CancellationToken cancellationToken, string userName);
    }
}