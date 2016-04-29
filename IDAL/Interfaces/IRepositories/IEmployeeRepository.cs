using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IRepositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<List<Employee>> GetAllEmployees(Guid employerId);
        Task<List<Employee>> GetAllEmployees(CancellationToken cancellationToken, Guid employerId);
    }
}