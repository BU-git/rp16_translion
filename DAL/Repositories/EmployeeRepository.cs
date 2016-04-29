using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces.IRepositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        internal EmployeeRepository(ApplicationDbContext context)
            : base(context)
        {
        }
        public Employee FindById(Guid employeeId)
        {
            return Set.FirstOrDefault(x => x.EmployeeId == employeeId);
        }

        public async Task<List<Employee>> GetAllEmployees(Guid employerId)
        {

            return await (from employee in Set
                where employee.Employer.EmployerId == employerId
                select employee).ToListAsync();
        }


        public async Task<List<Employee>> GetAllEmployees(CancellationToken cancellationToken, Guid employerId)
        {
            return await (from employee in Set
                where employee.Employer.EmployerId == employerId
                select employee).ToListAsync(cancellationToken);
        }
    }
}