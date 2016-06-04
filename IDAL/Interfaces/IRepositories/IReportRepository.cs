using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IRepositories
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<Report> GetLastEmployeeReport(Guid? employeeId);
        Task<List<Report>> GetReportsByEmployeeId(Guid? employeeId);
    }
}
