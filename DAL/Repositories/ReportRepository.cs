using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using IDAL.Interfaces.IRepositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class ReportRepository : Repository<Report>, IReportRepository
    {
        internal ReportRepository(ApplicationDbContext context)
            : base(context)
        {
           
        }

        public async Task<Report> GetLastEmployeeReport(Guid? employeeId)
        {
            var reports = await GetReportsByEmployeeId(employeeId);
            return reports?.Aggregate((i, j) => i.CreatedDate > j.CreatedDate ? i : j);
        }

        public async Task<List<Report>> GetReportsByEmployeeId(Guid? employeeId)
        {
            var query = from reports in Set
                        where reports.Employee.EmployeeId == employeeId
                        orderby reports.CreatedDate descending 
                        select reports;

            return await query.ToListAsync();
        }
    }
}
