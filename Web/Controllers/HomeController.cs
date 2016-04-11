using System;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.MailingService.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser, Guid> _userManager;

        public HomeController(UserManager<IdentityUser, Guid> userManager, IMailingService emailService)
        {
            _userManager = userManager;

            //bad solutions
            _userManager.EmailService = emailService;
            _userManager.UserTokenProvider =
                new DataProtectorTokenProvider<IdentityUser, Guid>(
                    new DpapiDataProtectionProvider("Sample").Create("EmailConfirmation"));
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // GET: Home
        public ActionResult Index()
        {
            return RedirectIfSignedIn() ?? RedirectToAction("Login", "Account");
        }

        [NonAction]
        private ActionResult RedirectIfSignedIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    RedirectToAction("Index", "Admin");
                }
                if (User.IsInRole("Advisor"))
                {
                    RedirectToAction("Index", "Advisor");
                }
                if (User.IsInRole("Employer"))
                {
                    RedirectToAction("Index", "Employer");
                }
            }
            return null;
        }
    }
}