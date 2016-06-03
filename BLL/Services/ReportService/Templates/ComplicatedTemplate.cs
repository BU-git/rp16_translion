using System.Text;
using BLL.Services.ReportService.Abstract;
using BLL.Services.ReportService.Factories;
using IDAL.Models;
using System.Linq;

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
            var designer = desFactory.GetDesigner(_name);

            foreach (var answer in PrepareAnswers(_question))
                designer.Draw(answer);
        }

        private string[] PrepareAnswers(Question question)
        {
            int splitBy = 5; //default - complicated type 1;

            if (question.TypeAnswer.Contains("3"))
                splitBy = 2;
            else if (question.TypeAnswer.Contains("2"))
                splitBy = 4;

            var answers = new string[question.Answers.Count / splitBy];
            var qAnswers = question.Answers.ToArray();
            StringBuilder answer;

            for (int i = 0; i < answers.Length; i++)
            {
                answer = new StringBuilder();

                for (int j = i * splitBy; j < (i + 1) * splitBy; j++)
                    answer.Append(qAnswers[j].Name + "   ");

                //answers
                answers[i] = answer.ToString();
            }

            return answers;
        }
    }
}
