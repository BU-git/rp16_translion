using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IRepositories
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<List<Report>> GetReportsByEmployeeId(Guid? employeeId);
    }
}
