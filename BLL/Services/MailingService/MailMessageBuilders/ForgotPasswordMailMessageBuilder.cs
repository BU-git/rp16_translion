using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class ForgotPasswordMailMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "To recover your password <a href=\"{0}\" target=\"_blank\">click here</a>";
        private readonly string _subject = "Password recovery";

        public ForgotPasswordMailMessageBuilder(string callbackUrl)
        {
            Body = string.Format(_body, callbackUrl);
            Subject = _subject;
        }
    }
}
