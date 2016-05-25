using IDAL.Models;
using System;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    /// <summary>
    /// Message that system will send to admin
    /// </summary>
    public sealed class ReportCompltdAdminMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Hello. Employee {0} from {1} completed report. Please, check result in attachment.";
        private readonly string _subject = "New report";

        public ReportCompltdAdminMessageBuilder(Employee employee)
        {
            Body = String.Format(_body, $"{employee.FirstName} {employee.Prefix} {employee.LastName}", employee.Employer.CompanyName);
            Subject = _subject;
        }
    }
}
