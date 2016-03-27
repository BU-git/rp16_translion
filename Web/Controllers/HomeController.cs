using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.MailMessageBuilders;
using Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Web.ViewModels;

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
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.LoginName);
                var isLoginSuccessful = await _userManager.CheckPasswordAsync(user, model.UserPassword);
                if (isLoginSuccessful)
                {
                    await SignInAsync(user, true);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(RegistrationEmployerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employer = RegistrationEmployerViewModel(model);
                var user = new IdentityUser
                {
                    UserName = model.LoginName,
                    Roles = Roles.Employer,
                    Employer = employer,
                    Email = model.EmailAdress
                };
                var result = await _userManager.CreateAsync(user, model.UserPassword);

                if (result.Succeeded)
                {
                    await SendEmail(user.Id, new RegistrationMailMessageBuilder(model.LoginName));
                    await SignInAsync(user, true);

                    return View("AccountConfirmation");
                }
            }
            return View(model);
        }

        //password remindering methods
        [HttpGet]
        [AllowAnonymous]
        public ViewResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof (HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel passwForgot)
        {
            var errorSummary = string.Empty; //summary error message

            if (!ModelState.IsValid)
                errorSummary = "Uw gebruikersnaam is incorrect, controleer dit aub.";
            else
            {
                var user = await _userManager.FindByNameAsync(passwForgot.UserName); //searching user by name

                if (user == null) //user not found
                    errorSummary = "Uw gebruikersnaam is incorrect, controleer dit aub.";
                else
                {
                    var passResetToken = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                    //generating password reset token

                    if (string.IsNullOrEmpty(passResetToken)) //generation of token failed
                        errorSummary = "Server Error. Try later";
                    else
                    {
                        var callbackUrl = Url.Action("PasswordRecovery", "Home",
                            new {userId = user.Id, token = passResetToken}, Request.Url.Scheme); //sets recovery url

                        await SendEmail(user.Id, new ForgotPasswordMailMessageBuilder(callbackUrl));

                        return View("ForgotPasswordEnd"); //succeeded
                    }
                }
            }

            //error occured
            ModelState.AddModelError("", errorSummary);
            return View();
        }


        //username changing methods
        [HttpGet]
        [AllowAnonymous]
        public ViewResult ForgotUserName()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof (HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ViewResult> ForgotUserName(ForgotUsernameViewModel unameForgot)
        {
            if (!ModelState.IsValid)
                ModelState.AddModelError(nameof(unameForgot.Email), "Uw emailadres is incorrect, controleer dit aub");
            else
            {
                var user = await _userManager.FindByEmailAsync(unameForgot.Email); //searching user by name

                if (user == null) //user not found
                    ModelState.AddModelError(nameof(unameForgot.Email), "Uw emailadres is incorrect, controleer dit aub");
                else
                {
                    await SendEmail(user.Id, new ForgotUsernameMailMessageBuilder(user.UserName));
                    return View("ForgotUserNameEnd"); //succeeded
                }
            }
            return View();
        }


        //password recovery (after remindering and revieving of email with token)
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordRecovery(Guid userId, string token)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "URL for resetting password is invalid");
                return RedirectToAction("ForgotPassword");
            }

            return View(new PasswordRecoveryViewModel {Token = token, Id = userId});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof (HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> PasswordRecovery(PasswordRecoveryViewModel passwRecovery)
        {
            var errorMessage = string.Empty; //error message
            if (string.CompareOrdinal(passwRecovery.Password, passwRecovery.ConfirmationalPassword) != 0)
                //comapres passwords if not match
                errorMessage = "Wachtwoord kom niet overeen";
            else //if match
            {
                if (!ModelState.IsValid && !string.IsNullOrWhiteSpace(passwRecovery.Password) &&
                    !string.IsNullOrWhiteSpace(passwRecovery.ConfirmationalPassword))
                    //if not valid data and passwords not null or white space, etc..
                    errorMessage = "URL for passwort reset is invalid. Try to generate new or check url.";
                else //if data valid
                {
                    var result =
                        await
                            _userManager.ResetPasswordAsync(passwRecovery.Id, passwRecovery.Token,
                                passwRecovery.Password); //reseting password

                    if (result.Succeeded)
                        return RedirectToAction("Index"); //redirecting to index if succeded
                    errorMessage =
                        "URL for passwort reset is invalid. Try to generate new or check url.";
                    //password reset 
                }
            }
            //error occured
            ModelState.AddModelError("", errorMessage);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> CheckUserName(string userName)
        {
            if (ModelState.IsValid && await _userManager.FindByNameAsync(userName) != null)
                return Json(true, JsonRequestBehavior.AllowGet);

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> CheckEmail(string email)
        {
            if (ModelState.IsValid && await _userManager.FindByEmailAsync(email) != null)
                return Json(true, JsonRequestBehavior.AllowGet);

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        private Employer RegistrationEmployerViewModel(RegistrationEmployerViewModel model)
        {
            var employer = new Employer
            {
                Adress = model.Adress,
                City = model.City,
                CompanyName = model.CompanyName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Prefix = model.Prefix,
                PostalCode = model.PostalCode,
                TelephoneNumber = model.TelephoneNumber
            };

            return employer;
        }

        private async Task SendEmail(Guid userId, MailMessageBuilder mailMessageBuilder)
        {
            await _userManager.SendEmailAsync(userId, mailMessageBuilder.Subject, mailMessageBuilder.Body);
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
            return RedirectToAction("Index", "Home");
        }
    }
}