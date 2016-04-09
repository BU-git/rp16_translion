using System;
using System.Net.Mail;
using System.Text;

namespace BLL.Services.MailingService.Types
{
    /// <summary>
    ///     Standard .Net MailMessage wrapper
    /// </summary>
    internal sealed class QueuedMessage
    {
        public QueuedMessage(MailMessage message, TimeSpan timeDelta)
        {
            TimeToRemove = DateTime.Now.Add(timeDelta);
            Message = message;
        }

        public MailMessage Message { get; }

        public DateTime TimeToRemove { get; }

        public int SendingAttempts { get; set; }

        /// <summary>
        ///     Returns recievers string
        /// </summary>
        /// <example>
        ///     aaa@gmail.com, azz@gmail.com
        /// </example>
        public string GetAllRecievers(char separator = ',')
        {
            var sBuilder = new StringBuilder();

            foreach (var address in Message.To)
                sBuilder.Append(address.Address + separator);

            var built = sBuilder.ToString();

            return built.Remove(built.Length - 1, 1);
        }
    }
}