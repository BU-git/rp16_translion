using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.Types;
using Microsoft.AspNet.Identity;

namespace BLL.Services.MailingService
{
    /// <summary>
    ///     Service that sends emails to users
    /// </summary>
    public sealed class MailingService : IMailingService
    {
        private readonly string m_from; //sender address
        private readonly MailQueue m_queue; //mailing queue

        private readonly SmtpClient m_sender; //email sender
        private bool m_disposed;

        private bool m_queueIgnored;

        public MailingService(string from, string password, string host)
            : this(new NetworkCredential(from, password), host)
        {
        }

        public MailingService(NetworkCredential credentials, string host) : this(credentials.UserName)
        {
            m_sender = new SmtpClient();
            m_sender.Credentials = credentials;
            m_sender.DeliveryMethod = SmtpDeliveryMethod.Network;
            m_sender.Host = host;
            m_sender.EnableSsl = true;
            m_sender.Timeout = 10000;
        }

        private MailingService(string from)
        {
            m_from = from;
            m_queue = MailQueue.Queue;
        }

        //can cause exception throwing if config has invalid settings
        //added only for identity support
        public MailingService()
            : this(
                ConfigurationManager.AppSettings["mailFrom"], ConfigurationManager.AppSettings["mailPass"],
                ConfigurationManager.AppSettings["mailHost"])
        {
        }

        public SendStatus SendMail(string body, string subject, params string[] to)
        {
            var createdMessage = CreateMessage(subject, body, null, to); //create a new message

            return SendMail(createdMessage);
        }

        public SendStatus SendMail(string body, string subject, string to) => SendMail(body, subject, new[] {to});

        public Task<SendStatus> SendMailAsync(string body, string subject, params string[] to)
        {
            var createdMessage = CreateMessage(subject, body, null, to);

            return SendMailAsync(createdMessage);
        }

        public Task<SendStatus> SendMailAsync(string body, string subject, string to)
            => SendMailAsync(body, subject, new[] {to});

        /// <summary>
        ///     Sends mail to recievers
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>
        ///     Sending operation result.
        ///     NOTE!!! Result is successful even if message has added to message queue
        /// </returns>
        public SendStatus SendMail(MailMessage message)
        {
            if (CheckMessageValidity(message))
            {
                try
                {
                    m_sender.Send(message);
                    return new SendStatus {HasError = false, Status = MessageStatus.Sent};
                }
                catch (SmtpException ex) //delivering exception
                {
                    if (!m_queueIgnored) //if not ignore queue adding
                    {
                        m_queue.AddMessage(message); //adding message to queue
                        return new SendStatus {HasError = false, Status = MessageStatus.InQueue};
                    }

                    return new SendStatus {HasError = true, Status = MessageStatus.Error, ErrorMessage = ex.Message};
                }
                catch (Exception ex)
                {
                    return new SendStatus {HasError = true, Status = MessageStatus.Error, ErrorMessage = ex.Message};
                }
            }
            //message is invalid
            return new SendStatus
            {
                ErrorMessage = "Message is invalid, check recievers,sender or message data",
                HasError = true,
                Status = MessageStatus.Error
            };
        }

        /// <summary>
        ///     Sends email to recievers async
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>
        ///     Sending operation result.
        ///     NOTE!!! Result is successful even if message has added to message queue
        /// </returns>
        public async Task<SendStatus> SendMailAsync(MailMessage message)
        {
            if (CheckMessageValidity(message))
            {
                try
                {
                    await m_sender.SendMailAsync(message);
                    return new SendStatus {HasError = false, Status = MessageStatus.Sent};
                }
                catch (SmtpException ex) //delivering exception
                {
                    if (!m_queueIgnored) //if not ignore queue adding
                    {
                        m_queue.AddMessage(message); //adding message to queue
                        return new SendStatus {HasError = false, Status = MessageStatus.InQueue};
                    }

                    return new SendStatus {HasError = true, Status = MessageStatus.Error, ErrorMessage = ex.Message};
                }
                catch (Exception ex)
                {
                    return new SendStatus {HasError = true, Status = MessageStatus.Error, ErrorMessage = ex.Message};
                }
            }
            //message is invalid
            return new SendStatus
            {
                ErrorMessage = "Message is invalid, check recievers,sender or message data",
                HasError = true,
                Status = MessageStatus.Error
            };
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_sender.Dispose();
                m_disposed = true;
            }
        }


        public void IgnoreQueue()
        {
            m_queueIgnored = true;
        }

        #region Identity

        /// <summary>
        ///     Allows identity message sending
        /// </summary>
        Task IIdentityMessageService.SendAsync(IdentityMessage message)
        {
            var created = message != null
                ? CreateMessage(message.Subject, message.Body, null, message.Destination)
                : null;

            return SendMailAsync(created);
        }
        #endregion

        #region MessageCreators

        /// <summary>
        ///     Creates a new MailMessage with subject, body and addresses
        /// </summary>
        /// <param name="subject">Message's subject</param>
        /// <param name="body">Message's body</param>
        /// <param name="addresses">Recievers addresses</param>
        /// <param name="attachment">Attachments</param>
        /// <returns>New MailMessage</returns>
        private MailMessage CreateMessage(string subject, string body, IEnumerable<byte> attachment,
            params string[] addresses)
        {
            try
            {
                var message = new MailMessage();
                //setting message values
                message.Body = body;
                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;

                message.From = new MailAddress(m_from);


                message.IsBodyHtml = true;

                //adding addresses to mail message
                if (addresses != null)
                {
                    foreach (var address in addresses)
                        message.To.Add(new MailAddress(address));
                }
                
                return message;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        #endregion

        #region Validation

        /// <summary>
        ///     Checks message for valid content
        /// </summary>
        /// <param name="message">Message to check</param>
        /// <returns>Checking result</returns>
        private bool CheckMessageValidity(MailMessage message)
        {
            return message != null && !string.IsNullOrEmpty(message.Body) && message.From != null &&
                   !string.IsNullOrEmpty(message.From.Address)
                   && CheckRecieversCollection(message) && !string.IsNullOrEmpty(message.Subject);
        }

        /// <summary>
        ///     Checks recievers collection for validity
        /// </summary>
        /// <param name="message">Mail message to check</param>
        /// <returns>Validation result</returns>
        private bool CheckRecieversCollection(MailMessage message)
        {
            if (message.To == null || message.To.Count == 0)
                return false;

            foreach (var address in message.To)
            {
                if (string.IsNullOrEmpty(address.Address)) //foreach address in addresses -> check address validity
                    return false;
            }

            return true;
        }

        #endregion
    }
}