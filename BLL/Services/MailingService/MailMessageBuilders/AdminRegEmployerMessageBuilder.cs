using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class AdminRegEmployerMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Your account was succefully created!\n\nUsername: {0}\nPassword: {1}\n\n You can change them after sign in.";
        private readonly string _subject = "Account was create";

        public AdminRegEmployerMessageBuilder(string username, string password)
        {
            Body = String.Format(_body, username, password);
            Subject = _subject;
        }
    }
}
