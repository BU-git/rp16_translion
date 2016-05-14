using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.AlertService;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.PersonageService;
using IDAL;
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

        #region Advisors section
        
        [HttpGet]
        public async Task<ActionResult> AdvisorInfo(Guid? id)
        {
            var user = await GetUserIfAdvisorAsync(id);

            if (user != null)
                return View(user.Advisor);

            return RedirectToAction("AdvisorsList");
        }

        [HttpGet]
        public async Task<ActionResult> AdvisorsList()
        {
            var advisorList = await advisorManager.GetAll();
            return View(advisorList);
        }

        [HttpGet]
        public async Task<JsonResult> CheckAdvisorName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(await adminManager.GetBaseUserByName(userName) == null,
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteAdvisor(Guid? id)
        {
            var user = await GetUserIfAdvisorAsync(id);

            WorkResult result = await advisorManager.Delete(user.UserId);
            if (user != null && result.Succeeded)
            {
                return View("Settings");
            }

            return RedirectToAction("AdvisorsList");
        }

        #endregion

        #region Alerts section

        public async Task<ActionResult> ViewAlerts()
        {
            var alerts = await alertManager.GetNew();
            var alertList = new List<AdminAlertPanelViewModel>();

            foreach (var alert in alerts)
            {
                var currentAlert = await MapAlertToTableView(alert);
                alertList.Add(currentAlert);
            }
            return View("AdminAlertPanel", alertList);
        }

        public async Task<ActionResult> ApproveAlert(Guid? alertId)
        {
            if (alertId != null)
            {
            var alertToApprove = alertManager.GetAlert((Guid)alertId);
            await alertManager.Approve(alertToApprove);
                
            }

            var alerts = await alertManager.GetNew();
            var alertList = new List<AdminAlertPanelViewModel>();
            foreach (var alert in alerts)
            {
                var currentAlert = await MapAlertToTableView(alert);
                alertList.Add(currentAlert);
            }
            return View("AdminAlertPanel", alertList);
        }

        #endregion

        #region Helpers region

        [NonAction]
        private async Task<User> GetUserIfAdvisorAsync(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return null;

            var user = await adminManager.GetBaseUserByGuid(id.ToString());

            return user?.Advisor != null ? user : null;
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
                AlertType = alert.AlertType.ToString(),
                Comment = alert.AlertComment
            };

            return AlertData;
        }

        #endregion

        #region Add advisor

        [HttpGet]
        public  ViewResult AddAdvisor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAdvisor(CreateAdvisorViewModel advisorInfo)
        {
            if (!ModelState.IsValid)
                return View(advisorInfo);

            User user = await adminManager.GetBaseUserByName(advisorInfo.Username);

            if (user != null)
            {
                ModelState.AddModelError(nameof(advisorInfo.Username), USERNAME_IS_IN_USE_ERROR);
                return View(advisorInfo);
            }
            
            IdentityUser identityUser = new IdentityUser
            {
                Id = Guid.NewGuid(),
                UserName = advisorInfo.Username
            };
            IdentityResult creationRes = await userManager.CreateAsync(
                identityUser,
                advisorInfo.Password);
            
            if (creationRes.Succeeded)
            {
                Advisor advisor = new Advisor()
                {
                    Name = advisorInfo.Name,
                    AdvisorId = identityUser.Id
                };

                await advisorManager.Create(advisor);

                var roleResult = await userManager.AddToRoleAsync(identityUser.Id, ADVISOR_ROLE);

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

                WorkResult result = await advisorManager.Update(user.Advisor);
                if (result.Succeeded)
                {
                    return View("Settings");
                }
            }

            ModelState.AddModelError("", SERVER_ERROR);
            return View(advInfo);
        }

        #endregion
    }
}