using BLL.Services.ReportService.Abstract;
using System.Collections.Generic;

namespace BLL.Services.ReportService.Factories
{
    /// <summary>
    /// Base class for designer factories
    /// </summary>
    public abstract class DesignerFactory
    {
        protected readonly Dictionary<string, Designer> _designers;

        protected DesignerFactory()
        {
            _designers = new Dictionary<string, Designer>();
        }

        public Designer GetDesigner(string name)
            => _designers[name];
    }
}
