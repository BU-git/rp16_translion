using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MailingService.Interfaces
{
    /// <summary>
    /// Logs info about some state
    /// </summary>
    public interface ILogger : IDisposable
    {
        void Log(String message);
    }
}
