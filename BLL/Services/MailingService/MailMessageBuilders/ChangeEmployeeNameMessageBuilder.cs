using System;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class ChangeEmployeeNameMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "De naam van uw werknemer is veranderd naar {0}";
        private readonly string _subject = "Employee's name changed";
        
        public ChangeEmployeeNameMessageBuilder(string employeeName)
        {
            Body = String.Format(_body, employeeName);
            Subject = _subject;
        }
    }
}


