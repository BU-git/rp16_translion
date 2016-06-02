using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.AlertService;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.PersonageService;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Advisor")]
    public class AdvisorController : BaseController
    {
        public AdvisorController(
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

        public async Task<ActionResult> Index()
        {
            var employers = await employerManager.GetAll();
            return View(employers);
        }

        public async Task<ActionResult> ViewAlerts()
        {
            var user = await adminManager.GetBaseUserByName(User.Identity.Name);
            var alerts = await alertManager.GetAdvisorAlerts(user.UserId);
            var alertList = new List<AdminAlertPanelViewModel>();

            foreach (var alert in alerts)
            {
                var currentAlert = await MapAlertToTableView(alert);
                alertList.Add(currentAlert);
            }
            return View("AdvisorAlertPanel", alertList);
        }

        protected async Task<AdminAlertPanelViewModel> MapAlertToTableView(Alert alert)
        {
            var EmployeeName = "n.v.t";
            var alertToShow = alertManager.GetAlert(alert.AlertId);

            if (alertToShow.EmployeeId != null)
            {
                var emp = await alertManager.FindEmployeeAsync(alert);
                EmployeeName = emp.LastName + " " + emp.FirstName;
            }

            Employer employer = await alertManager.FindEmployerAsync(alert);

            var AlertData = new AdminAlertPanelViewModel
            {
                alert = alert,
                EmployerName = employer.LastName + " " + employer.FirstName,
                Company = employer.CompanyName,
                EmployeeName = EmployeeName,
                AlertType = alert.AlertType.ToString()
            };

            return AlertData;
        }
    }
}