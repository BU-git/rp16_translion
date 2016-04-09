using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.MailMessageBuilders;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Web.ViewModels;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser, Guid> _userManager;

        public AccountController(UserManager<IdentityUser, Guid> userManager, IMailingService emailService)
        {
            _userManager = userManager;
            //bad solutions
            _userManager.EmailService = emailService;
            _userManager.UserTokenProvider =
                new DataProtectorTokenProvider<IdentityUser, Guid>(
                    new DpapiDataProtectionProvider("Sample").Create("EmailConfirmation"));
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        #region CheckUserName

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> CheckUserName(string userName)
        {
            if (ModelState.IsValid && await _userManager.FindByNameAsync(userName) != null)
                return Json(true, JsonRequestBehavior.AllowGet);

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CheckEmail

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> CheckEmail(string email)
        {
            if (ModelState.IsValid && await _userManager.FindByEmailAsync(email) != null)
                return Json(true, JsonRequestBehavior.AllowGet);

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Login

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Login()
        {
            //if (User.IsInRole("Admin"))
            //{
            //var name = User.Identity.Name;
            //var user = await _userManager.FindByNameAsync(name);
            // _userManager.GetAdmin(user);
            //}
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
                    return await _userManager.IsInRoleAsync(user.Id, "Employer")
                        ? RedirectToAction("Index", "Employer", new {id = user.Id})
                        : RedirectToAction("Index", "Admin", new {id = user.Id});
                }
            }
            return View(model);
        }

        #endregion

        #region Registration

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
                    Email = model.EmailAdress
                };
                var result = await _userManager.CreateAsync(user, model.UserPassword);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user.Id, "Admin");
                    await SignInAsync(user, true);
                    await SendEmail(user.Id, new RegistrationMailMessageBuilder(model.LoginName));

                    return View("AccountConfirmation");
                }
            }
            return View(model);
        }

        #endregion

        #region ForgotPassword

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            var result = RedirectIfSignedIn();

            if (result != null)
                return result;

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof (HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel passwForgot)
        {
            string errorSummary; //summary error message

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
                        errorSummary = "Server probleem ( Probeer a.u.b. later)";
                    else
                    {
                        if (Request.Url != null)
                        {
                            var callbackUrl = Url.Action("PasswordRecovery", "Account",
                                new {userId = user.Id, token = passResetToken}, Request.Url.Scheme); //sets recovery url

                            await SendEmail(user.Id, new ForgotPasswordMailMessageBuilder(callbackUrl));
                        }

                        return View("ForgotPasswordEnd"); //succeeded
                    }
                }
            }

            //error occured
            ModelState.AddModelError("", errorSummary);
            return View();
        }

        #endregion

        #region ForgotUserName

        //username changing methods
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotUserName()
        {
            return RedirectIfSignedIn() ?? View();
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

        #endregion

        #region PasswordRecovery

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordRecovery(Guid? userId, string token)
        {
            if (!ModelState.IsValid || !userId.HasValue || string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError("", "URL for resetting password is invalid");
                return RedirectToAction("ForgotPassword");
            }

            return View(new PasswordRecoveryViewModel {Token = token, Id = userId.Value});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof (HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> PasswordRecovery(PasswordRecoveryViewModel passwRecovery)
        {
            string errorMessage; //error message
            if (string.CompareOrdinal(passwRecovery.Password, passwRecovery.ConfirmationalPassword) != 0)
                //comapres passwords if not match
                errorMessage = "Wachtwoord kom niet overeen";
            else //if match
            {
                if (!ModelState.IsValid && !string.IsNullOrWhiteSpace(passwRecovery.Password) &&
                    !string.IsNullOrWhiteSpace(passwRecovery.ConfirmationalPassword))
                    //if not valid data and passwords not null or white space, etc..
                    errorMessage = "URL for password reset is invalid. Try to generate new or check url.";
                else //if data valid
                {
                    var result =
                        await
                            _userManager.ResetPasswordAsync(passwRecovery.Id, passwRecovery.Token,
                                passwRecovery.Password); //reseting password

                    if (result.Succeeded)
                        return RedirectToAction("Login"); //redirecting to index if succeded
                    errorMessage =
                        "URL for password reset is invalid. Try to generate new or check url.";
                    //password reset 
                }
            }
            //error occured
            ModelState.AddModelError("", errorMessage);
            return View();
        }

        #endregion

        #region Helper

        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties {IsPersistent = isPersistent}, identity);
        }

        private async Task SendEmail(Guid userId, MailMessageBuilder mailMessageBuilder)
        {
            await _userManager.SendEmailAsync(userId, mailMessageBuilder.Subject, mailMessageBuilder.Body);
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

        [NonAction]
        private ActionResult RedirectIfSignedIn()
        {
            if (User.Identity.IsAuthenticated)
                return User.IsInRole("Admin")
                    ? RedirectToAction("Index", "Admin")
                    : RedirectToAction("Index", "Employer");

            return null;
        }

        #endregion
    }
}