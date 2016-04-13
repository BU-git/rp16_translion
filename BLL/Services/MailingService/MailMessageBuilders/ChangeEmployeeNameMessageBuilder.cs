using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class ChangeEmployeeNameMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "De naam van uw werknemer is veranderd naar {0}";
        private readonly string _subject = "Employee's name changed";

        public ChangeEmployeeNameMessageBuilder(string employerName, string employeeName)
        {
            Body = String.Format(_body, employerName, employeeName);
            Subject = _subject;
        }
    }
}
