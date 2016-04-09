using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces.Repositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        internal EmployeeRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public Employee FindById(Guid id)
        {
            return Set.FirstOrDefault(x => x.EmployeeId == id);
        }

        public List<Employee> GetAllEmployees(User user)
        {
            return (from employee in Set
                where employee.EmployerId == user.UserId
                select employee).ToList();
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(User user)
        {
            return await (from employee in Set
                where employee.EmployerId == user.UserId
                select employee).ToListAsync();
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken, User user)
        {
            return await (from employee in Set
                where employee.EmployerId == user.UserId
                select employee).ToListAsync(cancellationToken);
        }
    }
}