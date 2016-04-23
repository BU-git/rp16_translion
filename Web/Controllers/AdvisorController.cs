using System;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.PersonageService;
using IDAL.Models;
using Microsoft.AspNet.Identity;

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
            IMailingService mailingService) : base(
                userManager,
                adminManager,
                advisorManager,
                employerManager,
                mailingService
                )
        {
        }
    }
}