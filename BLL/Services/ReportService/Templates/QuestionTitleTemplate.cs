using BLL.Services.ReportService.Abstract;
using BLL.Services.ReportService.Factories;
using IDAL.Models;

namespace BLL.Services.ReportService.Templates
{
    /// <summary>
    /// Question's title template
    /// </summary>
    internal sealed class QuestionTitleTemplate : QuestionBaseTemplate
    {
        public QuestionTitleTemplate(Question question) : base("QuestionTitle", question)
        {
        }

        public override void AddParagraph(DesignerFactory desFactory)
            => desFactory.GetDesigner(_name).Draw(_question.QuestionName);
    }
}
