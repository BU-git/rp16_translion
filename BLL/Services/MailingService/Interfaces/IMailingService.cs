using System;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.AspNet.Identity;

using BLL.Services.MailingService.Types;

namespace BLL.Services.MailingService.Interfaces
{
    public interface IMailingService : IIdentityMessageService, IDisposable
    {
        /// <summary>
        /// Send email to someone
        /// </summary>
        /// <param name="message"></param>
        SendStatus SendMail(MailMessage message);

        /// <summary>
        /// Send mail to someone
        /// </summary>
        /// <param name="to">Person's email address</param>
        /// <param name="body">Mail's body</param>
        /// <param name="subject">Mail's subject</param>
        SendStatus SendMail(String body, String subject, String to);

        /// <summary>
        /// Send emails to many recievers
        /// </summary>
        /// <param name="to">Recievers emails</param>
        /// <param name="body">Mail's body</param>
        /// <param name="subject">Mail's subject</param>
        SendStatus SendMail(String body, String subject, params String[] to);

        /// <summary>
        /// Async analogue of <see cref="SendMail(MailMessage)"/>
        /// </summary>
        /// <returns>Async operation's task object</returns>
        Task<SendStatus> SendMailAsync(MailMessage message);

        /// <summary>
        /// Async SendMail <see cref="SendMail(String, String, String)"/> analogue 
        /// </summary>
        /// <returns>Async operation's task object</returns>
        Task<SendStatus> SendMailAsync(String body, String subject, String to);

        /// <summary>
        /// Async SendMail <see cref="SendMail(String[])"/> analogue.
        /// </summary>
        /// <returns>Async operation's task object</returns>
        Task<SendStatus> SendMailAsync(String body, String subject, params String[] to);

        /// <summary>
        /// Doesn't add message to queue
        /// </summary>
        void IgnoreQueue();
    }
}
