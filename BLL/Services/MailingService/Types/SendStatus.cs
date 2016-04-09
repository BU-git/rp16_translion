using System;

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
    ///     Message sending operation status
    /// </summary>
    public class SendStatus
    {
        public SendStatus()
        {
            Status = MessageStatus.None;
        }

        internal MessageStatus Status { get; set; }

        public bool HasError { get; internal set; }

        public string ErrorMessage { get; internal set; }
    }
}