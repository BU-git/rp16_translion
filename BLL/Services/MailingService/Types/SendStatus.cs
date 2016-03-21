using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MailingService.Types
{
    [Serializable]
    public enum MessageStatus
    {
        None = 0,
        Sent = 1,
        InQueue = 2,
        Error = 3
    }

    /// <summary>
    /// Message sending operation status
    /// </summary>
    public class SendStatus
    {
        public SendStatus()
        {
            Status = MessageStatus.None;
        }

        internal MessageStatus Status { get; set; }

        public Boolean HasError { get; internal set; }

        public String ErrorMessage { get; internal set; }
    }
}
