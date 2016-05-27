using System;
using IDAL.Models;

namespace BLL.Services.ReportService.Abstract
{
    /// <summary>
    /// Base class for each question-based template
    /// </summary>
    public abstract class QuestionBaseTemplate : ElementTemplate
    {
        protected readonly Question _question;

        protected QuestionBaseTemplate(string name, Question question) : base(name)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            _question = question;
        }
    }
}
