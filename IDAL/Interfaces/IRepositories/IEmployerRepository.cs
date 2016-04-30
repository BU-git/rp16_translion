using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IRepositories
{
    public interface IEmployerRepository : IRepository<Employer>
    {
        void AddEmployee(Employee employee);
    }
}