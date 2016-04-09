using System;

namespace BLL.Services.MailingService.Interfaces
{
    /// <summary>
    ///     Logs info about some state
    /// </summary>
    public interface ILogger : IDisposable
    {
        void Log(string message);
    }
}