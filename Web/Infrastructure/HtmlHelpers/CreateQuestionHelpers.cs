using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using IDAL.Models;

namespace Web.Infrastructure.HtmlHelpers
{
    public static class CreateQuestionHelpers
    {
        public static MvcHtmlString CreateQuestion(this HtmlHelper html, Question question)
            => html.Partial(String.Format("_{0}", question.TypeAnswer), question);
    }
}