using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.MailMessageBuilders;
using IDAL.Interfaces;
using Quartz;

namespace BLL.Services.ReminderService
{
    public class Reminder : IJob
    {
        private const int maxExpiration = 30; // in days

        public async void Execute(IJobExecutionContext context)
        {
            var unitOfWork = (IUnitOfWork)context.Scheduler.Context.Get("unitOfWork");
            var mailingService = (IMailingService)context.Scheduler.Context.Get("mailingService");

            var employees = await unitOfWork.EmployeeRepository.GetAllEmployees();

            foreach (var employee in employees)
            {
                var employer = await unitOfWork.EmployerRepository.FindById(employee.EmployerId);
                string employerName = $"{employer.FirstName} {employer.Prefix} {employer.LastName}";
                string employeeName = $"{employee.FirstName} {employee.Prefix} {employee.LastName}";

                var report = await unitOfWork.ReportRepository.GetLastEmployeeReport(employee.EmployeeId);

                if (report != null)
                {
                    int expiration = (DateTime.Now - report.CreatedDate).Days;
                    if (expiration > maxExpiration)
                    {
                        var message = new RemindReportMessageBuilder(employerName, employeeName);
                        await mailingService.SendMailAsync(message.Body, message.Subject, employer.User.Email);
                    }
                }
            }
        }
    }
}
