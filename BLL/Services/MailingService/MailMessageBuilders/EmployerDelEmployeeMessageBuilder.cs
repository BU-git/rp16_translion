using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class EmployerDelEmployeeMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Hello. Employer {0} deleted employee {1}.";
        private readonly string _subject = "Employee deletion";

        public EmployerDelEmployeeMessageBuilder(string employerName, string employeeName)
        {
            Body = String.Format(_body, employerName, employeeName);
            Subject = _subject;
        }
    }
}
