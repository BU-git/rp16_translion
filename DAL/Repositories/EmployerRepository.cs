using IDAL.Interfaces.Repositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class EmployerRepository : Repository<Employer>, IEmployerRepository
    {
        internal EmployerRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public void AddEmployee(Employee employee, User user)
        {
            user.Employer.Employees.Add(employee);
        }
    }
}