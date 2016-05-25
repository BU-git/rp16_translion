using IDAL.Models;
using System;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    /// <summary>
    /// Message that service will sent after report completion
    /// </summary>
    public class ReportCompltdEmplMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Hello, {0}. Employee {1} completed report. Please, check result in attachment.";
        private readonly string _subject = "Report complete";

        public ReportCompltdEmplMessageBuilder(Employee employee)
        {
            Body = String.Format(_body, employee.Employer.FirstName, $"{employee.FirstName} {employee.Prefix} {employee.LastName}");
            Subject = _subject;
        }
    }
}
