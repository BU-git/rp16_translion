using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.AlertService;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.MailMessageBuilders;
using BLL.Services.PersonageService;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : BaseController
    {
        public EmployerController(
            UserManager<IdentityUser, Guid> userManager,
            PersonManager<Admin> adminManager,
            PersonManager<Advisor> advisorManager,
            PersonManager<Employer> employerManager,
            AlertManager alertManager,
            IMailingService mailingService) : base(
                userManager,
                adminManager,
                advisorManager,
                employerManager,
                alertManager,
                mailingService)
        {
        }

        [HttpGet]
        public new async Task<ActionResult> Index()
        {
            var user = await employerManager.GetBaseUserByGuid(User.Identity.GetUserId());

            if (user.Employer != null)
            {
                List<Employee> employees =
                    user.Employer.Employees.Where(e => !e.IsDeleted && e.IsApprove).ToList<Employee>();
                return View(employees);
            }
            return RedirectToAction("Logout");
        }

        [HttpGet]
        public override async Task<ViewResult> Settings()
        {
            var user = await employerManager.GetBaseUserByGuid(User.Identity.GetUserId());

            if (user != null)
            {
                var employer = await employerManager.Get(user.UserId);

                if (employer != null)
                {
                    return View(new EmployerSettingsViewModel
                    {
                        CompanyName = employer.CompanyName,
                        FirstName = employer.FirstName,
                        Prefix = employer.Prefix,
                        LastName = employer.LastName,
                        TelephoneNumber = employer.TelephoneNumber,
                        EmailAdress = user.Email,
                        PostalCode = employer.PostalCode,
                        Adress = employer.Adress,
                        City = employer.City,
                        UserName = user.UserName
                    });
                }
            }
            return View("Index");
        }

        [HttpGet]
        public override async Task<ActionResult> AddEmployee(Guid? userId = null)
        {
            if (userId == null || userId == Guid.Empty)
            {
                var user = await employerManager.GetBaseUserByGuid(User.Identity.GetUserId());
                ViewBag.EmployerId = user.UserId;
                return View();
            }
            ViewBag.EmployerId = userId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "AntiForgeryError")]
        public async override Task<ActionResult> AddEmployee(AddEmployeeViewModel employeeViewModel, string id)
        {
            if (!ModelState.IsValid)
                return View(employeeViewModel);

            var user = await employerManager.GetBaseUserByName(User.Identity.Name);

            if (user == null)
                return RedirectToAction("Logout");


            var employee = new Employee
            {
                EmployerId = user.UserId,
                EmployeeId = Guid.NewGuid(),
                LastName = employeeViewModel.LastName,
                FirstName = employeeViewModel.FirstName,
                Prefix = employeeViewModel.Prefix,
                IsApprove = false
            };


            await employerManager.CreateEmployee(employee);

            var alert = new Alert();
            {
                alert.AlertId = Guid.NewGuid();
                alert.EmployerId = user.UserId;
                alert.EmployeeId = employee.EmployeeId;
                alert.AlertType = AlertType.Employee_Add;
                alert.AlertIsDeleted = false;
                alert.AlertCreateTS = DateTime.Now;
                alert.AlertUpdateTS = DateTime.Now;
                alert.UserId = user.UserId;
            };
            await alertManager.CreateAsync(alert);

            var mailMessageData = new CreateEmployeeMailMessageBuilder(User.Identity.Name,
                $"{employeeViewModel.FirstName} {employeeViewModel.Prefix} {employeeViewModel.LastName}");

            await mailingService.SendMailAsync(mailMessageData.Body, mailMessageData.Subject,
                await GetAllAdminsEmailsAsync());

            return View("AddEmployeeSuccess");
        }

        [NonAction]
        private async Task<string[]> GetAllAdminsEmailsAsync()
        {
            var admins = await adminManager.GetAll();

            if (admins.Count == 0)
                return null;

            return admins
                .Select(adm => adm.User.Email)
                .ToArray();
        }
    }
}