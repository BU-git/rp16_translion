﻿using BLL.Services.ReportService.Abstract;
using System;
using System.Linq;
using IDAL.Models;
using BLL.Services.ReportService.Factories;

namespace BLL.Services.ReportService.Templates
{
    internal sealed class SingleLineTemplate : QuestionBaseTemplate
    {
        public SingleLineTemplate(Question question) : base("SingleLine", question)
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
