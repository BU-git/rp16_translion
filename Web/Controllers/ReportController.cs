using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BLL.Services.TestService;
using BLL.Services.TestService.Interfaces;
using IDAL.Interfaces;
using IDAL.Models;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly ITestService _testService;

        public ReportController(IUnitOfWork uow)
        {
            _testService = new TestManager(uow);    
        }

        [HttpGet]
        public ViewResult CreateReport()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> AddPages(Page[] pages)
        {
            if (!ModelState.IsValid || pages == null || pages.Length == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not all pages are valid");

            await _testService.DeleteAllPages(); //deleting old template

            await _testService.AddPages(pages);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPages(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
            {
                return RedirectToAction("Index", "Home");
            }

            var pages = await _testService.GetAllPages() 
                ?? new List<Page>();

            ViewBag.EmployeeId = id;
            return View(pages.OrderBy(p => p.Order).ToList());
        }

        [HttpPost]
        public async Task<ActionResult> SaveReport(FormCollection formCollection)
        {
            var pages = new List<Page>();

            int pageId, questionId;
            int? questionType, answerId; // for complicated question

            string employeeId = null;

            foreach (var key in formCollection.AllKeys)
            {
                if(key == "EmployeeId")
                {
                    employeeId = formCollection[key];
                    continue;
                }

                _testService.ParseAnswerName(key, out pageId, out questionId, out questionType, out answerId);

                var page = pages.FirstOrDefault(p => p.Id == pageId);
                if (page == null)
                {
                    var dbPage = await _testService.GetPageById(pageId);

                    page = new Page
                    {
                        Id = dbPage.Id,
                        Name = dbPage.Name,
                        Order = dbPage.Order,
                        Questions = new List<Question>()
                    };
                }
                else
                {
                    var oldPage = page;
                    pages.Remove(oldPage);
                }
                
                var question = page.Questions.FirstOrDefault(q => q.Id == questionId);
                if (question == null)
                {
                    var dbQuestion = await _testService.GetQuestion(questionId);
                    question = new Question
                    {
                        Id = dbQuestion.Id,
                        QuestionName = dbQuestion.QuestionName,
                        TypeAnswer = dbQuestion.TypeAnswer,
                        PageId = dbQuestion.PageId,
                        Answers = new List<Answer>()
                    };
                }
                else
                {
                    var oldQuestion = question;
                    page.Questions.Remove(oldQuestion);
                }

                foreach (var value in formCollection[key].Split(','))
                {
                    var answer = new Answer
                    {
                        Name = value,
                        QuestionId = question.Id
                    };
                    question.Answers.Add(answer);
                }
                
                page.Questions.Add(question);
                pages.Add(page);
            }

            var objectToJson = new { EmployeeId = employeeId, Pages = pages };
            string json = JsonConvert.SerializeObject(objectToJson);

            return View();
        }
    }
}