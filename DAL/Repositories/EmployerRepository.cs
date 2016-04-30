using System.Data.Entity;
using IDAL.Interfaces.IRepositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class EmployerRepository : Repository<Employer>, IEmployerRepository
    {
        internal EmployerRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async void AddEmployee(Employee employee)
        {
            Employer user = await FindById(employee.EmployerId);
            user.Employees.Add(employee);
            Update(user);
        }
    }
}