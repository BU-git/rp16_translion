using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public abstract class BaseManager
    {
        public BaseManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork _unitOfWork { get; }
        public abstract bool DeleteEmployee(Employee employee);

        #region GetAllEmployees

        public List<Employee> GetAllEmployees(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("User is null. Wrong parameter");
            }
            if (user.Roles.Any(x => x.Name == "Employer"))
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

        public async void CreateEmployeeAsync(Employee employee, User user)
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
            await _unitOfWork.SaveChangesAsync();
        }

        public async void CreateEmployeeAsync(CancellationToken cancellationToken, Employee employee, User user)
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
            await _unitOfWork.SaveChangesAsync(cancellationToken);
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

        public async void UpdateEmployeeAsync(Employee employee)
        {
            if (employee.EmployeeId == null)
            {
                throw new ArgumentException("employeeId is null. Wrong parameter");
            }
            _unitOfWork.EmployeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync();
        }

        public async void UpdateEmployeeAsync(CancellationToken cancellationToken, Employee employee)
        {
            if (employee.EmployeeId == null)
            {
                throw new ArgumentException("employeeId is null. Wrong parameter");
            }
            _unitOfWork.EmployeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}