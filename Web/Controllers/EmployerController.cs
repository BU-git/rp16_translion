using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.AlertService;
using BLL.Services.MailingService.Interfaces;
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
        public async Task<ActionResult> Index()
        {
            var user = await employerManager.GetBaseUserByGuid(User.Identity.GetUserId());

            if (user?.Employer != null)
                return View(user.Employer.Employees.Where(empl => !empl.IsDeleted));

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
    }
}