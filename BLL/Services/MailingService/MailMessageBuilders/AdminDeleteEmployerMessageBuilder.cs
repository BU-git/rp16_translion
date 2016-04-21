using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MailingService.MailMessageBuilders
{
    public class AdminDeleteEmployerMessageBuilder : MailMessageBuilder
    {
        private readonly string _body = "Your account have been deleted";
        private readonly string _subject = "Account was deleted by Trans Lion";

        public AdminDeleteEmployerMessageBuilder()
        {
            Body = _body;
            Subject = _subject;
        }
    }
}
