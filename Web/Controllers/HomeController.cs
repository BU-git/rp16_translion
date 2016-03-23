using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.ViewModels;

using BLL.Services.MailingService;
namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser, Guid> _userManager;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public HomeController(IUserStore<IdentityUser, Guid> userStore)
        {
            _userManager = new UserManager<IdentityUser, Guid>(userStore);
            _userManager.EmailService = new MailingService();
            
        }
        // GET: Home
        public ActionResult Index()
        {
            MailingService srv = new MailingService();
            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registration(AccountEmployerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.LoginName };
                var result = await _userManager.CreateAsync(user, model.UserPassword);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    await _userManager.SendEmailAsync(user.Id, "asdasdasdasd", "asdasdasdasd");
                    await SignInAsync(user, false);
                    

                    return RedirectToAction("Succeed");
                }
            }
            return View(model);
            
        }

        public ActionResult ForgotUsername()
        {
            return View();
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }


        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
        }
    }
}