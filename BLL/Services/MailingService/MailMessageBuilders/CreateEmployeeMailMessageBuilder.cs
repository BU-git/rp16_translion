using System;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class CreateEmployeeMailMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Hello. Employer {0} created a new employee {1}. Please, confirm this action.";
        private readonly string _subject = "Employee creation";

        public CreateEmployeeMailMessageBuilder(string employerName, string employeeName)
        {
            Body = String.Format(_body, employerName, employeeName);
            Subject = _subject;
        }
    }
}
