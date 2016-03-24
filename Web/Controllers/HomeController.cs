using System;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.MailingService.Interfaces;
using Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Web.ViewModels;
using Claim = Domain.Models.Claim;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser, Guid> _userManager;

        public HomeController(IUserStore<IdentityUser, Guid> userStore, IMailingService emailService)
        {
            _userManager = new UserManager<IdentityUser, Guid>(userStore);
            _userManager.EmailService = emailService;
            _userManager.UserTokenProvider =
                new DataProtectorTokenProvider<IdentityUser, Guid>(
                    new DpapiDataProtectionProvider("Sample").Create("EmailConfirmation"));
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
                bool loginIsSuccessful = await _userManager.CheckPasswordAsync(user, model.UserPassword);
                if (loginIsSuccessful == true)
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
                    await
                        _userManager.SendEmailAsync(user.Id, "Thank for your registration",
                            $"Thank, you, {model.LoginName} for registration");
                    await SignInAsync(user, true);

                    return View("AccountConfirmation");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Result()
        {
            var userName = HttpContext.User.Identity.GetUserName();
            var user = await _userManager.FindByNameAsync(userName);
            @ViewBag.Name = user.UserName; 
            return View();
        }

        // debug only
        //public ActionResult AccountConfirmation()
        //{
        //    return View();
        //}
        
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
                ModelState.AddModelError(nameof(passwForgot.UserName), "Error with username");
            else
            {
                var user = await _userManager.FindByNameAsync(passwForgot.UserName); //searching user by name

                if (user == null) //user not found
                    errorSummary = "Can't find user. Try later or register.";
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
                        await
                            _userManager.SendEmailAsync(user.Id, "Password recovery",
                                $"To recover your password <a href=\"{callbackUrl}\" target=\"_blank\">click here</a>");
                        return RedirectToAction("ForgotPasswordEnd"); //succeeded
                    }
                }
            }

            //error occured
            ModelState.AddModelError("", errorSummary);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult ForgotPasswordEnd()
        {
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
        public async Task<ActionResult> ForgotUserName(ForgotUsernameViewModel unameForgot)
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
                    await
                        _userManager.SendEmailAsync(user.Id, "User name remindering",
                            $"Hello, dear user. Your username is {user.UserName}");
                    return RedirectToAction("ForgotUserNameEnd"); //succeeded
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult ForgotUserNameEnd()
        {
            return View();
        }

        //password recovery (after remindering and revieving of email with token)
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordRecovery(Guid userId, string token)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "URL for reset is invalid");
                return RedirectToAction("ForgotPassword");
            }

            return View(new PasswordRecoveryViewModel {Token = token, Id = userId}); //bad way
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
                errorMessage = "Password and confirmation password are not match";
            else //if match
            {
                if (!ModelState.IsValid && !string.IsNullOrWhiteSpace(passwRecovery.Password) &&
                    !string.IsNullOrWhiteSpace(passwRecovery.ConfirmationalPassword))
                    //if not valid data and passwords not null or white space, etc..
                    errorMessage = "Error with reseting url. Try to generate new token for confirmation.";
                else //if data valid
                {
                    var result =
                        await
                            _userManager.ResetPasswordAsync(passwRecovery.Id, passwRecovery.Token,
                                passwRecovery.Password); //reseting password

                    if (result.Succeeded)
                        return RedirectToAction("Index"); //redirecting to index if succeded
                    errorMessage =
                        "Error with reseting password, please, try later. Or generate new token for confirmation.";
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
            var employer = new Employer();
            employer.Adress = model.Adress;
            employer.City = model.City;
            employer.CompanyName = model.CompanyName;
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
            return RedirectToAction("Index", "Home");
        }
    }
}