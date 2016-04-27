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
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public AdminController(
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
        public async Task<ActionResult> AdvisorsList()
        {
            return View(await advisorManager.GetAllAsync());
        }

        #region Advisor's info

        [HttpGet]
        public async Task<JsonResult> CheckAdvisorName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(await adminManager.GetUserByNameAsync(userName) == null,
                JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Remove advisor

        [HttpGet]
        public async Task<ActionResult> RemoveAdvisor(Guid? id)
        {
            var user = await GetUserIfAdvisorAsync(id);

            if (user != null && await adminManager.DeleteAsync(user) > 0)
                return View("Settings");

            return RedirectToAction("AdvisorsList");
        }

        #endregion

        #region View alerts

        [HttpGet]
        public ActionResult ViewAlerts()
        {
            List<Alert> Alerts = new List<Alert>();
            Alerts = alertManager.GetNew();
            List<AdminAlertPanelViewModel> alertList = new List<AdminAlertPanelViewModel>();
            foreach (var alert in Alerts)
            {
                AdminAlertPanelViewModel a = MapAlertToTableView(alert);
                alertList.Add(a);
            }

            return View("AdminAlertPanel", alertList);
        }

        #endregion

        #region Approve alerts

        public ActionResult ApproveAlert(Guid? alertId)
        {
            Alert alertToApprove = alertManager.GetAlert(alertId);
            alertManager.Approve(alertToApprove);

            List<Alert> Alerts = new List<Alert>();
            Alerts = alertManager.GetNew();
            List<AdminAlertPanelViewModel> alertList = new List<AdminAlertPanelViewModel>();
            foreach (var alert in Alerts)
            {
                AdminAlertPanelViewModel a = MapAlertToTableView(alert);
                alertList.Add(a);
            }
            return View("AdminAlertPanel", alertList);
        }

        #endregion

        [NonAction]
        private async Task<User> GetUserIfAdvisorAsync(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return null;

            var user = await adminManager.GetUserByIdAsync(id.Value);

            return user?.Advisor != null ? user : null;
        }

        private AdminAlertPanelViewModel MapAlertToTableView(Alert alert)
        {
            string EmployeeName = "n.v.t";
            Alert alertToShow = alertManager.GetAlert(alert.AlertId);
            if (alertToShow.Employees.Count != 0)
            {
                Employee emp = alertManager.FindEmployee(alert);
                EmployeeName = emp.LastName + " " + emp.FirstName;
            }

            var AlertData = new AdminAlertPanelViewModel
            {
                alert = alert,
                EmployerName = alert.Employer.LastName + " " + alert.Employer.FirstName,
                Company = alert.Employer.CompanyName,
                EmployeeName = EmployeeName,
                AlertType = "Demo",
                Comment = alert.AlertComment
            };

            return AlertData;
        }

        #region Add advisor

        [HttpGet]
        public ViewResult AddAdvisor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAdvisor(CreateAdvisorViewModel advisorInfo)
        {
            if (!ModelState.IsValid)
                return View(advisorInfo);


            if (await adminManager.GetUserByNameAsync(advisorInfo.Username) != null)
            {
                ModelState.AddModelError(nameof(advisorInfo.Username), USERNAME_IS_IN_USE_ERROR);
                return View(advisorInfo);
            }

            var creationRes = await
                userManager.CreateAsync(new IdentityUser {Id = Guid.NewGuid(), UserName = advisorInfo.Username},
                    advisorInfo.Password);

            User user = null;

            if (creationRes.Succeeded
                && (user = await adminManager.GetUserByNameAsync(advisorInfo.Username)) != null)
            {
                await advisorManager.CreateAsync(new Advisor {Name = advisorInfo.Name}, user);

                var roleResult = await userManager.AddToRoleAsync(user.UserId, ADVISOR_ROLE);

                if (roleResult.Succeeded)
                    return RedirectToAction("Settings");
            }

            ModelState.AddModelError("", SERVER_ERROR);
            return View(advisorInfo);
        }

        #endregion

        #region Change advisor's name

        [HttpGet]
        public async Task<ActionResult> ChangeAdvisorName(Guid? id)
        {
            var user = await GetUserIfAdvisorAsync(id);

            if (user == null)
                return RedirectToAction("AdvisorsList");

            return View(new AdvisorChangeNameViewModel
            {
                Id = user.UserId,
                Name = user.Advisor.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeAdvisorName(AdvisorChangeNameViewModel advInfo)
        {
            if (!ModelState.IsValid)
                return View(advInfo);

            var user = await GetUserIfAdvisorAsync(advInfo.Id);

            if (user != null)
            {
                user.Advisor.Name = advInfo.Name;

                if (await advisorManager.UpdateAsync(user.Advisor) > 0)
                    return View("Settings");
            }

            ModelState.AddModelError("", SERVER_ERROR);
            return View(advInfo);
        }

        #endregion
    }
}