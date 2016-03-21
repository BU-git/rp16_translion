using System;
using System.Net.Mail;
using System.Text;


namespace BLL.Services.MailingService.Types
{
    /// <summary>
    /// Standard .Net MailMessage wrapper
    /// </summary>
    internal sealed class QueuedMessage
    {
        readonly DateTime m_timeToRemove; //time when message will be remove(or later)

        MailMessage m_mailMessage; //wrapped message

        Int32 m_messageSendingAttempts; //message sending attempts

        public QueuedMessage(MailMessage message, TimeSpan timeDelta)
        {
            m_timeToRemove = DateTime.Now.Add(timeDelta);
            m_mailMessage = message;
        }

        public MailMessage Message { get { return m_mailMessage; } }

        public DateTime TimeToRemove { get { return m_timeToRemove; } }

        public Int32 SendingAttempts
        {
            get { return m_messageSendingAttempts; }
            set { m_messageSendingAttempts = value; }
        }

        /// <summary>
        /// Returns recievers string
        /// </summary>
        /// <example>
        /// aaa@gmail.com, azz@gmail.com
        /// </example>
        public String GetAllRecievers(Char separator = ',')
        {
            StringBuilder sBuilder = new StringBuilder();

            foreach (MailAddress address in m_mailMessage.To)
                sBuilder.Append(address.Address + separator);

            String built = sBuilder.ToString();

            return built.Remove(built.Length - 1, 1);
        }
    }
}
