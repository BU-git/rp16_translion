using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BLL.Identity.Models;
using BLL.Services.AlertService;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.PersonageService;
using BLL.Services.TestService;
using BLL.Services.TestService.Interfaces;
using IDAL.Interfaces;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Web.ViewModels;
using BLL.Services.ReportService;
using IDAL;

namespace Web.Controllers
{
    public class ReportController : BaseController
    {
        private readonly ITestService _testService;
        private readonly ReportPassingManager _reportManager;

        public ReportController(IUnitOfWork uow, ReportPassingManager reportManager, UserManager<IdentityUser, Guid> userManager, PersonManager<Admin> adminManager, PersonManager<Advisor> advisorManager, PersonManager<Employer> employerManager, AlertManager alertManager, IMailingService mailingService) 
            : base(userManager, adminManager, advisorManager, employerManager, alertManager, mailingService)
        {
            _testService = new TestManager(uow);
            _reportManager = reportManager;
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

            var model = new ReportViewModel
            {
                Employee =  await adminManager.GetEmployee(id.Value),
                Pages = pages.OrderBy(p => p.Order).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SaveReport(FormCollection formCollection)
        {
            var pages = new List<Page>();

            int pageId, questionId;
            int? questionType, lineId, answerId; // for complicated question

            string employeeId = null;

            foreach (var key in formCollection.AllKeys)
            {
                if(key == "EmployeeId")
                {
                    employeeId = formCollection[key];
                    continue;
                }

                _testService.ParseAnswerName(key, out pageId, out questionId, out questionType, out lineId, out answerId);

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
                
                foreach (var value in formCollection.GetValues(key))
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

            Guid emplId;
            Employee employee;
            if (!Guid.TryParse(employeeId, out emplId) || (employee = await adminManager.GetEmployee(emplId)) == null)
            {
                return View("ReportResult", false);
            }

            var sender = new ReportSender(mailingService, employee, pages);

            var result = await sender.SendMailsToRecieversAsync();
            if (result == WorkResult.Success())
            {
                await _reportManager.AddReport(emplId);
            }

            return View("ReportResult", new ReportPassedViewModel
            {
                Passed = result.Succeeded,
                EmployeeId = emplId.ToString()
            });
        }
    }
}