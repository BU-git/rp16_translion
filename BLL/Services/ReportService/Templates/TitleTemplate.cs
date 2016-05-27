using BLL.Services.ReportService.Abstract;
using BLL.Services.ReportService.Factories;

namespace BLL.Services.ReportService.Templates
{
    /// <summary>
    /// Creates title of a document
    /// </summary>
    internal sealed class TitleTemplate : ElementTemplate
    {
        private const string TITLE_DOC = "Trans Lion medewerker rapport";

        public TitleTemplate() : base("Title")
        {
        }

        public override void AddParagraph(DesignerFactory desFactory)
            => desFactory.GetDesigner(_name).Draw(TITLE_DOC);
    }
}
