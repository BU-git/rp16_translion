using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class DeleteEmployeeMailMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Hello. Employee {0} has been deleted";
        private readonly string _subject = "Employee deletion";

        public DeleteEmployeeMailMessageBuilder(string employeeName)
        {
            Body = String.Format(_body, employeeName);
            Subject = _subject;
        }
    }
}
