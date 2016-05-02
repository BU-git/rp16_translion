using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using IDAL.Models;

namespace Web.Infrastructure.HtmlHelpers
{
    public static class CreatePreviewHelper
    {
        public static MvcHtmlString CreatePreview(this HtmlHelper html, Question question)
            => html.Partial(String.Format("_QuestionPreview"), question);
    }
}