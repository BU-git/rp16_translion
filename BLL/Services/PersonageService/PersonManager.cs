using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces;
using IDAL.Interfaces.Managers;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public abstract class PersonManager<TEntity> : IPersonageManager<TEntity> where TEntity : class
    {
        public PersonManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork _unitOfWork { get; }

        public abstract void DeleteEmployee(User user, Employee employee);


        #region Get user by username
        public User GetUserByName(string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            return _unitOfWork.UserRepository.FindByUserName(userName);
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            return await _unitOfWork.UserRepository.FindByUserNameAsync(userName);
        }

        public async Task<User> GetUserByNameAsync(CancellationToken cancellationToken, string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            return await _unitOfWork.UserRepository.FindByUserNameAsync(cancellationToken, userName);
        }
        #endregion

        #region Get user by id
        public User GetUserById(string userId)
        {
            Guid gUserId;

            if (!Guid.TryParse(userId, out gUserId))
                throw new ArgumentException($"User's id can't be {userId}");

            return GetUserById(gUserId);
        }

        public User GetUserById(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException($"User's id can't be {userId}");

            return _unitOfWork.UserRepository.FindById(userId);
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException($"User's id can't be {userId}");

            return await _unitOfWork.UserRepository.FindByIdAsync(userId);
        }

        public async Task<User> GetUserByIdAsync(CancellationToken cancellationToken, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException($"User's id can't be {userId}");

            return await _unitOfWork.UserRepository.FindByIdAsync(cancellationToken, userId);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            Guid gUserId;

            if (!Guid.TryParse(userId, out gUserId))
                throw new ArgumentException($"User's id can't be {userId}");

            return await GetUserByIdAsync(gUserId);
        }

        public async Task<User> GetUserByIdAsync(CancellationToken cancellationToken, string userId)
        {
            Guid gUserId;

            if (!Guid.TryParse(userId, out gUserId))
                throw new ArgumentException($"User's id can't be {userId}");

            return await GetUserByIdAsync(cancellationToken, gUserId);
        }
        #endregion

        #region GetAllEmployees

        public List<Employee> GetAllEmployees(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("User is null. Wrong parameter");
            }
            if (user.Roles.Any(x => x.Name != "Employer"))
            {
                throw new ArgumentException("Only employer can have employees. User isn't employer");
            }
            return _unitOfWork.EmployeeRepository.GetAllEmployees(user);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("User is null. Wrong parameter");
            }
            if (user.Roles.Any(x => x.Name == "Employer"))
            {
                throw new ArgumentException("Only employer can have employees. User isn't employer");
            }
            return await _unitOfWork.EmployeeRepository.GetAllEmployeesAsync(user);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken, User user)
        {
            if (user == null)
            {
                throw new ArgumentException("User is null. Wrong parameter");
            }
            if (user.Roles.Any(x => x.Name == "Employer"))
            {
                throw new ArgumentException("Only employer can have employees. User isn't employer");
            }
            return await _unitOfWork.EmployeeRepository.GetAllEmployeesAsync(cancellationToken, user);
        }

        #endregion

        #region GetEmployee

        public Employee GetEmployee(Guid employeeId)
        {
            if (employeeId == null)
            {
                throw new ArgumentException("employeeId is null. Wrong parameter");
            }
            return _unitOfWork.EmployeeRepository.FindById(employeeId);
        }

        public async Task<Employee> GetEmployeeAsync(Guid employeeId)
        {
            if (employeeId == null)
            {
                throw new ArgumentException("employeeId is null. Wrong parameter");
            }
            return await _unitOfWork.EmployeeRepository.FindByIdAsync(employeeId);
        }

        public async Task<Employee> GetEmployeeAsync(CancellationToken cancellationToken, Guid employeeId)
        {
            if (employeeId == null)
            {
                throw new ArgumentException("employeeId is null. Wrong parameter");
            }
            return await _unitOfWork.EmployeeRepository.FindByIdAsync(cancellationToken, employeeId);
        }

        #endregion

        #region CreateEmployee

        public void CreateEmployee(Employee employee, User user)
        {
            if (employee == null | user == null)
            {
                throw new ArgumentException("employee or user is null. Wrong parameter");
            }
            if (user.Roles.Any(x => x.Name == "Employer"))
            {
                throw new ArgumentException("Only employer can have employees. User isn't employer");
            }
            _unitOfWork.EmployerRepository.AddEmployee(employee, user);
            _unitOfWork.SaveChanges();
        }

        public Task<int> CreateEmployeeAsync(Employee employee, User user)
        {
            if (employee == null | user == null)
            {
                throw new ArgumentException("employee or user is null. Wrong parameter");
            }
            if (!user.Roles.Any(x => x.Name == "Employer"))
            {
                throw new ArgumentException("Only employer can have employees. User isn't employer");
            }
            _unitOfWork.EmployerRepository.AddEmployee(employee, user);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> CreateEmployeeAsync(CancellationToken cancellationToken, Employee employee, User user)
        {
            if (employee == null | user == null)
            {
                throw new ArgumentException("employee or user is null. Wrong parameter");
            }
            if (user.Roles.Any(x => x.Name == "Employer"))
            {
                throw new ArgumentException("Only employer can have employees. User isn't employer");
            }
            _unitOfWork.EmployerRepository.AddEmployee(employee, user);
            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region UpdateEmployee

        public void UpdateEmployee(Employee employee)
        {
            if (employee.EmployeeId == null)
            {
                throw new ArgumentException("employeeId is null. Wrong parameter");
            }
            _unitOfWork.EmployeeRepository.Update(employee);
            _unitOfWork.SaveChanges();
        }

        public Task<int> UpdateEmployeeAsync(Employee employee)
        {
            if (employee.EmployeeId == null)
            {
                throw new ArgumentException("employeeId is null. Wrong parameter");
            }
            _unitOfWork.EmployeeRepository.Update(employee);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> UpdateEmployeeAsync(CancellationToken cancellationToken, Employee employee)
        {
            if (employee.EmployeeId == null)
            {
                throw new ArgumentException("employeeId is null. Wrong parameter");
            }
            _unitOfWork.EmployeeRepository.Update(employee);
            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public abstract List<TEntity> GetAll();
        public abstract Task<List<TEntity>> GetAllAsync();
        public abstract Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        public abstract TEntity Get(User user);
        public abstract Task<TEntity> GetAsync(User user);
        public abstract Task<TEntity> GetAsync(CancellationToken cancellationToken, User user);
        public abstract void Create(TEntity entity, User user);
        public abstract Task<int> CreateAsync(TEntity entity, User user);
        public abstract Task<int> CreateAsync(CancellationToken cancellationToken, TEntity entity, User user);
        public abstract void Update(TEntity entity);
        public abstract Task<int> UpdateAsync(TEntity entity);
        public abstract Task<int> UpdateAsync(CancellationToken cancellationToken, TEntity entity);
        public abstract void Delete(User user);
        public abstract Task<int> DeleteAsync(User user);
        public abstract Task<int> DeleteAsync(CancellationToken cancellationToken, User user);

        #endregion
    }
}