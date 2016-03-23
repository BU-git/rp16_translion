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

using BLL.Services.MailingService.Interfaces;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser, Guid> _userManager;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public HomeController(IUserStore<IdentityUser, Guid> userStore, IMailingService emailService)
        {
            _userManager = new UserManager<IdentityUser, Guid>(userStore);
            _userManager.EmailService = emailService;
            
        }
        // GET: Home
        public ActionResult Index()
        {
            //MailingService srv = new MailingService();
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


        //password remindering methods
        [HttpGet]
        [AllowAnonymous]
        public ViewResult RemindUserPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> RemindUserPassword(String userName)
        {
            String errorSummary = String.Empty; //summary error message

            if (!ModelState.IsValid)
                errorSummary = "Some message on dutch about error with user's name!";
            else
            {
                IdentityUser user = await _userManager.FindByNameAsync(userName); //searching user by name

                if (user == null) //user not found
                    errorSummary = "Can't find user. Try later or register.";
                else
                {

                    String passResetToken = await _userManager.GeneratePasswordResetTokenAsync(user.Id); //generating password reset token

                    if (!String.IsNullOrEmpty(passResetToken)) //generation of token failed
                        errorSummary = "Server Error. Try later";
                    else
                    {
                        String callbackUrl = Url.Action("PasswordRecovery", "Home", new { userId = user.Id, token = passResetToken }, Request.Url.Scheme); //sets recovery url
                        await _userManager.SendEmailAsync(user.Id, "Password recovery", $"To recover your password <a href=\"{callbackUrl}\" target=\"_blank\">click here</a>");
                        return RedirectToAction("RemindUserPasswordSuccess"); //succeeded
                    }
                }
            }

            //error occured
            ModelState.AddModelError("", errorSummary);
            return View();            
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult RemindUserPasswordSuccess()
        {
            return View();
        }


        //username changing methods
        [HttpGet]
        [AllowAnonymous]
        public ViewResult RemindUserName()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> RemindUserName(String email)
        {
            String errorSummary = String.Empty; //summary error message

            if (!ModelState.IsValid)
                errorSummary = "Some message on dutch about error with user's email!";
            else
            {
                IdentityUser user = await _userManager.FindByEmailAsync(email); //searching user by name

                if (user == null) //user not found
                    errorSummary = "Can't find user. Try later or register.";
                else
                {
                    await _userManager.SendEmailAsync(user.Id, "User name remindering", $"Hello, dear user. Your username is {user.UserName}");
                    return RedirectToAction("RemindUserNameSuccess"); //succeeded
                }
            }

            ModelState.AddModelError("", errorSummary);
            return View();
        }
 
        [HttpGet]
        [AllowAnonymous]
        public ViewResult RemindUserNameSuccess()
        {
            return View();
        }

        //password recovery (after remindering and revieving of email with token)
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordRecovery(Guid id, String token)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "URL for reset is invalid.");
                return RedirectToAction("RemindUserPassword");
            }
            return View(new { Id = id, Token = token });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> PasswordRecovery(String pass, String confPass, Guid id, String token)
        {
            String errorMessage = String.Empty; //error message

            if (String.CompareOrdinal(pass, confPass) != 0)  //comapres passwords if not match
                errorMessage = "Password and confirmation password are not match";
            else //if match
            {
                if (!ModelState.IsValid && !String.IsNullOrWhiteSpace(pass) && !String.IsNullOrWhiteSpace(confPass)) //if not valid data and passwords not null or white space, etc..
                    errorMessage = "Error with reseting url. Try to generate new token for confirmation.";
                else //if data valid
                {
                    var result = await _userManager.ResetPasswordAsync(id, token, pass); //reseting password

                    if (result.Succeeded) 
                        return RedirectToAction("Index"); //redirecting to index if succeded
                    else
                        errorMessage = "Error with reseting password, please, try later. Or generate new token for confirmation."; //password reset 
                }
            }
            //error occured
            ModelState.AddModelError("", errorMessage);
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