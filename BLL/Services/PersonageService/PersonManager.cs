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
        

        public abstract Task<WorkResult> DeleteEmployee(Employee employee);

        public abstract Task<WorkResult> Delete(TEntity entity);
        public abstract Task<WorkResult> Delete(CancellationToken cancellationToken, TEntity entity);

        #region Get User

        public async Task<User> GetBaseUserByName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                return await UnitOfWork.UserRepository.FindByUserName(userName);
            }
            return null;
        }

        public async Task<User> GetBaseUserByName(CancellationToken cancellationToken, string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                return await UnitOfWork.UserRepository.FindByUserName(cancellationToken, userName);
            }
            return null;
        }

        public async Task<User> GetBaseUserByGuid(Guid userId)
        {
            if (userId != Guid.Empty)
            {
                return await UnitOfWork.UserRepository.FindById(userId);
            }
            return null;
        }

        public async Task<User> GetBaseUserByGuid(CancellationToken cancellationToken, Guid userId)
        {
            if (userId != Guid.Empty)
            {
                return await UnitOfWork.UserRepository.FindById(cancellationToken, userId);
            }
            return null;
        }

        public async Task<User> GetBaseUserByGuid(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Guid userGuid;

                if (!Guid.TryParse(userId, out userGuid))
                {
                    return null;
                }
                return await GetBaseUserByGuid(userGuid);
            }
            return null;
        }

        public async Task<User> GetBaseUserByGuid(CancellationToken cancellationToken, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Guid userGuid;

                if (!Guid.TryParse(userId, out userGuid))
                {
                    return null;
                }
                return await GetBaseUserByGuid(cancellationToken, userGuid);
            }
            return null;
        }

        #endregion

        #region GetAllEmployees

        public async Task<List<Employee>> GetAllEmployees(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                User user = await GetBaseUserByGuid(userId);
                return await UnitOfWork.EmployeeRepository.GetAllEmployees(user.UserId);
            }
            return null;
        }

        public async Task<List<Employee>> GetAllEmployees(CancellationToken cancellationToken, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                User user = await GetBaseUserByGuid(cancellationToken, userId);
                return await UnitOfWork.EmployeeRepository.GetAllEmployees(cancellationToken, user.UserId);
            }
            return null;
        }

        public async Task<List<Employee>> GetAllEmployees(Guid userId)
        {
            if (userId != Guid.Empty)
            {
                return await UnitOfWork.EmployeeRepository.GetAllEmployees(userId);
            }
            return null;
        }

        public async Task<List<Employee>> GetAllEmployees(CancellationToken cancellationToken, Guid userId)
        {
            if (userId != Guid.Empty)
            {
                return await UnitOfWork.EmployeeRepository.GetAllEmployees(cancellationToken, userId);
            }
            return null;
        }

        #endregion

        #region GetEmployee

        public async Task<Employee> GetEmployee(Guid employeeId)
        {
            if (employeeId != Guid.Empty)
            {
                return await UnitOfWork.EmployeeRepository.FindById(employeeId);
            }
            return null;
        }

        public async Task<Employee> GetEmployee(CancellationToken cancellationToken, Guid employeeId)
        {
            if (employeeId != Guid.Empty)
            {
                return await UnitOfWork.EmployeeRepository.FindById(cancellationToken, employeeId);
            }
            return null;
        }

        #endregion

        #region CreateEmployee

        public async Task<WorkResult> CreateEmployee(Employee employee)
        {
            if (employee != null)
            {
                UnitOfWork.EmployerRepository.AddEmployee(employee);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned result 0");
            }
            return WorkResult.Failed("Wrong param.Entity is null");
        }

        public async Task<WorkResult> CreateEmployee(CancellationToken cancellationToken, Employee employee)
        {
            if (employee != null)
            {
                UnitOfWork.EmployerRepository.AddEmployee(employee);
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned result 0");
            }
            return WorkResult.Failed("Wrong param.Entity is null");
        }

        #endregion

        #region UpdateEmployee

        public async Task<WorkResult> UpdateEmployee(Employee employee)
        {
            if (employee != null)
            {
                UnitOfWork.EmployeeRepository.Update(employee);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned result 0");
            }
            return WorkResult.Failed("Wrong param.Entity is null");
        }

        public async Task<WorkResult> UpdateEmployee(CancellationToken cancellationToken, Employee employee)
        {
            if (employee != null)
            {
                UnitOfWork.EmployeeRepository.Update(employee);
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned result 0");
            }
            return WorkResult.Failed("Wrong param.Entity is null");
        }

        #endregion

        #region Get user's roles by id
        public async Task<List<Role>> GetUserRolesById(string userId)
        {
            var user = await GetBaseUserByGuid(userId);

            return user?.Roles.ToList() ?? new List<Role>();
        }

        public async Task<List<Role>> GetUserRolesById(CancellationToken cancellationToken, string userId)
        {
            var user = await GetBaseUserByGuid(cancellationToken, userId);

            return user?.Roles.ToList() ?? new List<Role>();
        }
        #endregion
    }
}