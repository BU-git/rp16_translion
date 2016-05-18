using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.Types;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;

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
            m_sender = new SmtpClient
            {
                Credentials = credentials,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = host,
                EnableSsl = true,
                Timeout = 10000
            };
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
            var createdMessage = CreateMessage(subject, body, to); //create a new message

            return SendMail(createdMessage);
        }

        public SendStatus SendMail(string body, string subject, string to) 
            => SendMail(body, subject, new[] {to});

        public SendStatus SendMail(string body, string subject, byte[] attachment,
            string fileName, params string[] to)
        {
            var createdMessage =
                CreateMessageWithAttachment(subject, body, attachment, fileName, to);

            return SendMail(createdMessage);
        }

        public SendStatus SendMail(string body, string subject, byte[] attachment,
            string fileName, string to)
        {
            return SendMail(body, subject, attachment, fileName, new[] { to });
        }

        public Task<SendStatus> SendMailAsync(string body, string subject, params string[] to)
        {
            var createdMessage = CreateMessage(subject, body, to);

            return SendMailAsync(createdMessage);
        }

        public Task<SendStatus> SendMailAsync(string body, string subject, string to)
            => SendMailAsync(body, subject, new[] {to});


        public Task<SendStatus> SendMailAsync(string body, string subject, byte[] attachment,
            string fileName, params string[] to)
        {
            var createdMessage =
                CreateMessageWithAttachment(subject, body, attachment, fileName, to);

            return SendMailAsync(createdMessage);
        }

        public Task<SendStatus> SendMailAsync(string body, string subject, byte[] attachment,
            string fileName, string to)
        {
            return SendMailAsync(body, subject, attachment, fileName, new[] {to});
        }
            
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
            if (!CheckMessageValidity(message))
            {
                return new SendStatus
                {
                    ErrorMessage = "Message is invalid, check recievers, sender or message data",
                    HasError = true,
                    Status = MessageStatus.Error
                };
            }

            message.From = new MailAddress(m_from); //setting valid sender

            try
            {
                m_sender.Send(message);
                return new SendStatus {HasError = false, Status = MessageStatus.Sent};
            }
            catch (SmtpException ex) //delivering exception
            {
                if (m_queueIgnored)
                    return new SendStatus {HasError = true, Status = MessageStatus.Error, ErrorMessage = ex.Message};

                m_queue.AddMessage(message); //adding message to queue

                return new SendStatus {HasError = false, Status = MessageStatus.InQueue};
            }
            catch (Exception ex)
            {
                return new SendStatus {HasError = true, Status = MessageStatus.Error, ErrorMessage = ex.Message};
            }
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
            if (!CheckMessageValidity(message))
            {
                return new SendStatus
                {
                    ErrorMessage = "Message is invalid, check recievers, sender or message data",
                    HasError = true,
                    Status = MessageStatus.Error
                };
            }

            message.From = new MailAddress(m_from); //setting valid sender   

            try
            {
                await m_sender.SendMailAsync(message);
                return new SendStatus {HasError = false, Status = MessageStatus.Sent};
            }
            catch (SmtpException ex) //delivering exception
            {
                if (m_queueIgnored)
                    return new SendStatus {HasError = true, Status = MessageStatus.Error, ErrorMessage = ex.Message};

                m_queue.AddMessage(message); //adding message to queue

                return new SendStatus {HasError = false, Status = MessageStatus.InQueue};
            }
            catch (Exception ex)
            {
                return new SendStatus {HasError = true, Status = MessageStatus.Error, ErrorMessage = ex.Message};
            }
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
        /// <returns>New MailMessage</returns>
        private MailMessage CreateMessage(string subject, string body, params string[] addresses)
        {
            var message = new MailMessage
            {
                Body = body,
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };


            if (!CheckRecieversCollection(addresses))
                return null;

            foreach (var address in addresses)
                message.To.Add(new MailAddress(address));

            return message;
        }

        /// <summary>
        ///     Creates a new MailMessage with subject, body, addresses and attachment
        /// </summary>
        /// <param name="subject">Message's subject</param>
        /// <param name="body">Message's body</param>
        /// <param name="fileName">File name of attachment</param>
        /// <param name="addresses">Recievers addresses</param>
        /// <param name="attachment">Attachments</param>
        /// <returns>New MailMessage</returns>
        private MailMessage CreateMessageWithAttachment(string subject, string body,
            byte[] attachment, string fileName, params string[] addresses)
        {
            var message = CreateMessage(subject, body, addresses);

            return message == null ? null : AddAttachment(attachment, fileName, message);
        }
 
        /// <summary>
        /// Adds attachment to message.
        /// </summary>
        /// <param name="attachment">File's bytes</param>
        /// <param name="fileName">Name of file</param>
        /// <param name="message">Message where will be attachments</param>
        /// <returns>Message with attachment</returns>
        private MailMessage AddAttachment(byte[] attachment, string fileName, MailMessage message)
        {
            if (attachment == null || attachment.Length == 0 || String.IsNullOrWhiteSpace(fileName))
            {
                return message;
            }

            var mAttachment = new Attachment(new MemoryStream(attachment), fileName);
            message.Attachments.Add(mAttachment);
            return message;
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
            return message != null 
                   && !string.IsNullOrWhiteSpace(message.Body) 
                   && !string.IsNullOrWhiteSpace(message.Subject);
        }

        /// <summary>
        ///     Checks recievers collection for validity
        /// </summary>
        /// <param name="recievers">Collection of addresses</param>
        /// <returns>Validation result</returns>
        private bool CheckRecieversCollection(IEnumerable<string> recievers)
        {
            if (recievers == null)
                return false;

            //checks if it is valid string and has email format
            return 
                !recievers.Any(r => String.IsNullOrWhiteSpace(r) || !Regex.IsMatch(r, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"));
        }
        #endregion
    }
}