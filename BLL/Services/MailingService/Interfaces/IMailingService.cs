using System;
using System.Net.Mail;
using System.Threading.Tasks;
using BLL.Services.MailingService.Types;
using Microsoft.AspNet.Identity;

namespace BLL.Services.MailingService.Interfaces
{
    public interface IMailingService : IIdentityMessageService, IDisposable
    {
        /// <summary>
        ///     Send email to someone
        /// </summary>
        /// <param name="message"></param>
        SendStatus SendMail(MailMessage message);

        /// <summary>
        ///     Send mail to someone
        /// </summary>
        /// <param name="to">Person's email address</param>
        /// <param name="body">Mail's body</param>
        /// <param name="subject">Mail's subject</param>
        SendStatus SendMail(string body, string subject, string to);

        /// <summary>
        ///     Send emails to many recievers
        /// </summary>
        /// <param name="to">Recievers emails</param>
        /// <param name="body">Mail's body</param>
        /// <param name="subject">Mail's subject</param>
        SendStatus SendMail(string body, string subject, params string[] to);

        /// <summary>
        /// Sends mail with attachment
        /// </summary>
        SendStatus SendMail(string body, string subject, byte[] attachment,
            string fileName, params string[] to);

        /// <summary>
        /// Sends mail with attachment
        /// </summary>
        SendStatus SendMail(string body, string subject, byte[] attachment,
            string fileName, string to);

        /// <summary>
        ///     Async analogue of <see cref="SendMail(MailMessage)" />
        /// </summary>
        /// <returns>Async operation's task object</returns>
        Task<SendStatus> SendMailAsync(MailMessage message);

        /// <summary>
        ///     Async SendMail <see cref="SendMail(String, String, String)" /> analogue
        /// </summary>
        /// <returns>Async operation's task object</returns>
        Task<SendStatus> SendMailAsync(string body, string subject, string to);

        /// <summary>
        ///     Async SendMail <see cref="SendMail(String, String, String[])" /> analogue.
        /// </summary>
        /// <returns>Async operation's task object</returns>
        Task<SendStatus> SendMailAsync(string body, string subject, params string[] to);


        Task<SendStatus> SendMailAsync(string body, string subject, byte[] attachment,
            string fileName, params string[] to);

        Task<SendStatus> SendMailAsync(string body, string subject, byte[] attachment,
            string fileName, string to);

        /// <summary>
        ///     Doesn't add message to queue
        /// </summary>
        void IgnoreQueue();
    }
}