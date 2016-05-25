using System;
using BLL.Services.ReportService.Abstract;
using BLL.Services.ReportService.Factories;
using IDAL.Models;

namespace BLL.Services.ReportService.Templates
{
    /// <summary>
    /// Page template
    /// </summary>
    internal sealed class PageTemplate : ElementTemplate
    {
        private readonly Page _page;

        public PageTemplate(Page page) : base("Page")
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            _page = page;
        }


        public override void AddParagraph(DesignerFactory desFactory)
            => desFactory.GetDesigner(_name).Draw(_page.Name);
    }
}
