using System;
using System.Threading;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.Types;

namespace BLL.Services.MailingService
{
    /// <summary>
    ///     Adds and removes mails to queue
    /// </summary>
    public static class QueueListener
    {
        public static void Start(IMailingService mailingService, ILogger logger)
        {
            m_sender = mailingService;
            m_sender.IgnoreQueue(); //doesn't add message to queue
            m_logger = logger;
            m_backgrndWorker = new Thread(BackgrndAction) {IsBackground = true}; //setting thread action
            m_maxSendTries = 3;
            StartBackgrndProcessing(); //starts listener
        }

        public static void Stop()
        {
            m_backgrndWorker.Abort();
            m_logger?.Log($"[{DateTime.Now.ToLongTimeString()}] Listener stopped.");
            m_logger?.Dispose();
        }

        private static void StartBackgrndProcessing()
        {
            m_backgrndWorker.Start();
            m_logger?.Log($"[{DateTime.Now.ToLongTimeString()}] Listener started.");
        }

        /// <summary>
        ///     Action that executes in thread
        /// </summary>
        private static void BackgrndAction()
        {
            try
            {
                while (true)
                {
                    var message = MailQueue.Queue.GetMessage(); //message to resend

                    if (message != null)
                    {
                        if (message.TimeToRemove > DateTime.Now && message.SendingAttempts < m_maxSendTries)
                            //checks if it can send message
                        {
                            var sendResult = m_sender.SendMail(message.Message);

                            if (sendResult.Status == MessageStatus.Error && ++message.SendingAttempts <= m_maxSendTries)
                                //another sending fail
                                MailQueue.Queue.AddMessage(message);
                            else if (sendResult.Status == MessageStatus.Sent)
                                m_logger?.Log(
                                    $"[{DateTime.Now.ToLongTimeString()}] Successfully sent a message to {message.GetAllRecievers()}.");
                        }
                        else
                            m_logger?.Log(
                                $"[{DateTime.Now.ToLongTimeString()}] Sending of message to {message.GetAllRecievers()} failed.");
                    }
                }
            }
            catch (ThreadAbortException)
            {
                m_logger?.Log($"[{DateTime.Now.ToLongTimeString()}] Thread stopped.");
            }
        }

        #region private

        private static IMailingService m_sender;

        private static ILogger m_logger;

        private static int m_maxSendTries;

        private static Thread m_backgrndWorker;

        #endregion
    }
}