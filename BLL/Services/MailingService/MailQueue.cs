using System;
using System.Net.Mail;
using System.Collections.Concurrent;

using BLL.Services.MailingService.Types;

namespace BLL.Services.MailingService
{

    /// <summary>
    /// Mailing queue
    /// </summary>
    internal sealed class MailQueue
    {
        #region private
        static readonly Lazy<MailQueue> m_obj =
            new Lazy<MailQueue>(() => new MailQueue()); //singleton obj

        ConcurrentQueue<QueuedMessage> m_queue; //concurrent queue - main queue

        TimeSpan m_deletionDelta; //delta time of deletion
        #endregion

        //hidden constructor of singleton obj
        private MailQueue()
        {
            m_queue = new ConcurrentQueue<QueuedMessage>();
            m_deletionDelta = TimeSpan.FromMinutes(3.0);
        }

        internal static MailQueue Queue { get { return m_obj.Value; } } //returns this class instance

        //stops queue (calls dispose of all disposable objects)
        internal static void Stop()
        {
            //TODO: Can dispose logger
        }


        /// <summary>
        /// Adds a message to queue
        /// </summary>
        internal void AddMessage(MailMessage message) => m_queue.Enqueue(FromMailMessage(message));
        /// <summary>
        /// Add queued message to queue one more time
        /// </summary>
        internal void AddMessage(QueuedMessage message) => m_queue.Enqueue(message);

        /// <summary>
        /// Dequeue value from queue. Null if can't get message.
        /// </summary>
        /// <returns>Mail message from queue</returns>
        internal QueuedMessage GetMessage()
        {
            QueuedMessage message;

            return m_queue.TryDequeue(out message) ? message : null; //dequeues message from queue or returns null
        }

        /// <summary>
        /// Casts .net mail message to QueuedMessageWrapper
        /// </summary>
        private QueuedMessage FromMailMessage(MailMessage messageToCast) => new QueuedMessage(messageToCast, m_deletionDelta);
    }
}
