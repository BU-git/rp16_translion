using System;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class AdminAddEmployeeMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "De gewenste werknemer, met de naam {0} is toegevoegd aan uw account";
        private readonly string _subject = "Nieuwe account werknemer";

        public AdminAddEmployeeMessageBuilder(string employeeName)
        {
            Body = String.Format(_body, employeeName);
            Subject = _subject;
        }
    }
}
