using System;
using System.Linq;
using BLL.Services.ReportService.Abstract;
using BLL.Services.ReportService.Factories;
using IDAL.Models;

namespace BLL.Services.ReportService.Templates
{
    /// <summary>
    /// Comment answer template
    /// </summary>
    internal sealed class CommentTemplate : QuestionBaseTemplate
    {
        public CommentTemplate(Question question) : base("Comment", question)
        {
        }

        public override void AddParagraph(DesignerFactory desFactory)
        {
            var answer = _question.Answers.FirstOrDefault();

            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            desFactory.GetDesigner(_name).Draw(answer.Name);
        }
    }
}
