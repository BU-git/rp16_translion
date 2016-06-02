using BLL.Services.ReportService.Factories;
using BLL.Services.ReportService.Templates;
using IDAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.ReportService.Abstract
{
    /// <summary>
    /// Get's document as byte array
    /// </summary>
    public abstract class Document
    {
        #region members
        /// <summary>
        /// List of pages that document will write to file
        /// </summary>
        private readonly List<Page> _pages;

        /// <summary>
        /// Employee which passed a test
        /// </summary>
        private readonly Employee _employeeInfo;


        /// <summary>
        /// Queue of templates in the document
        /// </summary>
        private Queue<ElementTemplate> _templates;
        #endregion

        protected Document(List<Page> pages, Employee employee, string extension)
        {
            if (pages == null)
            {
                throw new ArgumentNullException(nameof(pages));
            }
            if (pages.Count == 0)
            {
                throw new ArgumentException("Pages count is 0");
            }
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
                
            Name = $"{employee.FirstName} {employee.Prefix} {employee.LastName} {DateTime.UtcNow.ToShortDateString()} raport{extension}";
            _pages = pages;
            _employeeInfo = employee;
            _templates = new Queue<ElementTemplate>();
        }

        /// <summary>
        /// Document name
        /// </summary>
        public string Name { get; private set; }

        #region Specials
        /// <summary>
        /// Adds templates to queue
        /// </summary>
        protected void ProcessDocument()
        {
            Question lastQuestion = null;
            _templates.Enqueue(new TitleTemplate());
            _templates.Enqueue(new EmployeeInfoTemplate(_employeeInfo));
            foreach (var page in _pages)
            {
                _templates.Enqueue(new PageTemplate(page));
                foreach (var question in page.Questions)
                {
                    if (lastQuestion == null || lastQuestion.QuestionName != question.QuestionName)
                        _templates.Enqueue(new QuestionTitleTemplate(question));
                    
                    _templates.Enqueue(SelectTemplate(question));
                    lastQuestion = question;
                }
            }
        }

        /// <summary>
        /// Gets element template using question's answer type
        /// </summary>
        /// <param name="question">Question</param>
        /// <returns>
        /// Choosen element template
        /// </returns>
        protected ElementTemplate SelectTemplate(Question question)
        {
            if (question.TypeAnswer.Contains("Comment"))
                return new CommentTemplate(question);
            else if (question.TypeAnswer.Contains("Comp"))
                return new ComplicatedTemplate(question);
            else if (question.TypeAnswer.Contains("Date"))
                return new DatePickerTemplate(question);
            else if (question.TypeAnswer.Contains("Single"))
                return new SingleLineTemplate(question);
            else
                return new SelectableTemplate(question);
        }

        /// <summary>
        /// Dequeues all templates and fills document
        /// </summary>
        internal void WriteToDoc(DesignerFactory factory)
        {
            while (_templates.Count > 0)
                _templates.Dequeue().AddParagraph(factory);
        }
        #endregion

        #region abstract 
        public abstract byte[] GetDocument();
        #endregion
    }
}
