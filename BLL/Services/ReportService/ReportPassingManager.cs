using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.Interfaces;
using IDAL.Models;

namespace BLL.Services.ReportService
{
    public class ReportPassingManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportPassingManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<WorkResult> AddReport(Guid? employeeId)
        {
            if (employeeId == null)
            {
                return WorkResult.Failed("Wrong param. Employee id is null");
            }

            try
            {
                var employee = await _unitOfWork.EmployeeRepository.FindById(employeeId);
                if (employee == null)
                {
                    return WorkResult.Failed("Employee wasn't found. Possible uncorrect id");
                }

                var report = new Report
                {
                    ReportId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Employee = employee
                };

                _unitOfWork.ReportRepository.Add(report);
                int result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned result 0");
            }
            catch (Exception e)
            {
                return WorkResult.Failed(e.Message);
            }
        }

        public async Task<List<Report>> GetReportByEmployeeId(Guid? employeeId)
        {
            if (employeeId == null)
            {
                return null;
            }

            return await _unitOfWork.ReportRepository.GetReportsByEmployeeId(employeeId);
        }
    }
}
