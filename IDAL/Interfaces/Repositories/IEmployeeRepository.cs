using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee FindById(Guid id);
        List<Employee> GetAllEmployees(User user);
        Task<List<Employee>> GetAllEmployeesAsync(User user);
        Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken, User user);
    }
}