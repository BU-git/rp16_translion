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
    public class AdvisorController : AdminController
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
        public new async Task<ActionResult> ViewAlerts()
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


    }
}