using System.Text;
using BLL.Services.ReportService.Abstract;
using BLL.Services.ReportService.Factories;
using IDAL.Models;

namespace BLL.Services.ReportService.Templates
{
    /// <summary>
    /// Complicated question template
    /// </summary>
    internal sealed class ComplicatedTemplate : QuestionBaseTemplate
    {
        public ComplicatedTemplate(Question question) : base("Complicated", question)
        {
        }

        public override void AddParagraph(DesignerFactory desFactory)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var answer in _question.Answers)
                builder.Append($"{answer.Name}    ");

            desFactory.GetDesigner(_name).Draw(builder.ToString());
        }
    }
}
