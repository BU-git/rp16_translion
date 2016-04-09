using IDAL.Models;

namespace IDAL.Interfaces.Repositories
{
    public interface IEmployerRepository : IRepository<Employer>
    {
        void AddEmployee(Employee employee, User user);
    }
}