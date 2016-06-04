using System;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class RemindReportMessageBuilder : MailMessageBuilder
    {
        private readonly string _subject = "Reminder";
        private readonly string _body = "{0},\n\n Since last report for employee {1} have been created 20 days ago, a new report should be created.\n\n";

        public RemindReportMessageBuilder(string employerName, string employeeName)
        {
            Subject = _subject;
            Body = String.Format(_body, employerName, employeeName);
        }
    }
}
