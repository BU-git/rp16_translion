using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.MailingService;
using Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser, Guid> _userManager;

        public HomeController(IUserStore<IdentityUser, Guid> userStore)
        {
            _userManager = new UserManager<IdentityUser, Guid>(userStore);
            _userManager.EmailService = new MailingService();
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        // GET: Home
        
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.LoginName);
                if (user != null)
                {
                    await SignInAsync(user, true);
                    return RedirectToAction("Result");
                }
            }
            return View(model);
        }
        
        [AllowAnonymous]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(AccountEmployerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employer = AccountEmployerViewModelToEmloyer(model);
                var user = new IdentityUser {UserName = model.LoginName, Roles = Roles.Employer, Employer = employer};
                var result = await _userManager.CreateAsync(user, model.UserPassword);

                if (result.Succeeded)
                {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //await _userManager.SendEmailAsync(user.Id, "asdasdasdasd", "asdasdasdasd");
                    await SignInAsync(user, true);

                    return RedirectToAction("Result");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Result()
        {
            return View("Index");
        }

        private Employer AccountEmployerViewModelToEmloyer(AccountEmployerViewModel model)
        {
            var employer = new Employer();
            employer.Adress = model.Adress;
            employer.City = model.City;
            employer.CompanyName = model.CompanyName;
            employer.EmailAdress = model.EmailAdress;
            employer.FirstName = model.FirstName;
            employer.LastName = model.LastName;
            employer.Prefix = model.Prefix;
            employer.PostalCode = model.PostalCode;
            employer.TelephoneNumber = model.TelephoneNumber;
            return employer;
        }

        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties {IsPersistent = isPersistent}, identity);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}