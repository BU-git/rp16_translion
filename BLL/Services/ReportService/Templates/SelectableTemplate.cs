using BLL.Services.ReportService.Abstract;
using BLL.Services.ReportService.Factories;
using IDAL.Models;
using System.Linq;
using System.Text;

namespace BLL.Services.ReportService.Templates
{
    /// <summary>
    /// Checkboxes and radiobuttons template
    /// </summary>
    internal sealed class SelectableTemplate : QuestionBaseTemplate
    {
        public SelectableTemplate(Question question) : base("Selectable", question)
        {
        }

        public override void AddParagraph(DesignerFactory desFactory)
        {
            var designer = desFactory.GetDesigner(_name);

            foreach (var answer in _question.Answers)
                designer.Draw(answer.Name);
        }

    }
}
