using System;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    /// <summary>
    /// Employee recovery
    /// </summary>
    public sealed class RecoverEmployeeMailMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Hello, {0}. An employee {1} successfully recovered.";
        private readonly string _subject = "Employee recovery";

        public RecoverEmployeeMailMessageBuilder(string employeeName, string employerName)
        {
            Body = String.Format(_body, employerName, employeeName);
            Subject = _subject;
        }
    }
}
