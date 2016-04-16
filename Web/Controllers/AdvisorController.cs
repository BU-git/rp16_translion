using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Identity.Models;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.MailMessageBuilders;
using BLL.Services.PersonageService;
using IDAL.Interfaces;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Advisor")]
    public class AdvisorController : Controller
    {
        private const string SERVER_ERROR = "Server probleem(Probeer a.u.b.later)";

        private readonly PersonManager<Employer> _employerManager;
        private readonly UserManager<IdentityUser, Guid> _userManager;
        private readonly PersonManager<Advisor> _advisorManager;
        private readonly PersonManager<Admin> _adminManager; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailingService _mailingService;

        public AdvisorController(IUnitOfWork uow, IUserStore<IdentityUser, Guid> store, IMailingService mailingService)
        {
            _unitOfWork = uow;
            _employerManager = new EmployerManager(uow);
            _userManager = new UserManager<IdentityUser, Guid>(store);
            _advisorManager = new AdvisorManager(uow);
            _adminManager = new AdminManager(uow);

            _userManager.UserTokenProvider =
                new DataProtectorTokenProvider<IdentityUser, Guid>(
                    new DpapiDataProtectionProvider("Sample").Create("EmailConfirmation"));

            _mailingService = mailingService;

        }
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        public ActionResult Index()
        {
            ViewBag.Employers = _employerManager.GetAll();
            return View();
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EmployerProfile(Guid id)
        {
            var user = _userManager.FindById(id);
            Employer employer = _unitOfWork.EmployerRepository.FindById(id);

            EmployerViewModel model = new EmployerViewModel
            {
                EmailAdress = user.Email,
                UserName = user.UserName,
                FirstName = employer.FirstName,
                LastName = employer.LastName,
                CompanyName = employer.CompanyName,
                Adress = employer.Adress,
                City = employer.City,
                Prefix = employer.Prefix,
                PostalCode = employer.PostalCode,
                TelephoneNumber = employer.TelephoneNumber
            };

            ViewBag.EmployerId = id;
            ViewBag.Employees = employer.Employees;
            return View(model);
        }

        [HttpGet]
        public ActionResult EditEmployer(Guid id)
        {
            var user = _userManager.FindById(id);
            Employer employer = _unitOfWork.EmployerRepository.FindById(id);

            EmployerViewModel model = new EmployerViewModel
            {
                EmailAdress = user.Email,
                UserName = user.UserName,

                FirstName = employer.FirstName,
                LastName = employer.LastName,
                CompanyName = employer.CompanyName,
                Adress = employer.Adress,
                City = employer.City,
                Prefix = employer.Prefix,
                PostalCode = employer.PostalCode,
                TelephoneNumber = employer.TelephoneNumber
            };

            ViewBag.EmployerId = id;
            return View(model);
        }



        [HttpPost]
        public ActionResult EditEmployer(EmployerViewModel model, Guid id)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindById(id);
                Employer employer = _unitOfWork.EmployerRepository.FindById(id);

                user.Email = model.EmailAdress;

                employer.Adress = model.Adress;
                employer.City = model.City;
                employer.CompanyName = model.CompanyName;
                employer.FirstName = model.FirstName;
                employer.LastName = model.LastName;
                employer.PostalCode = model.PostalCode;
                employer.Prefix = model.Prefix;
                employer.TelephoneNumber = model.TelephoneNumber;

                _userManager.Update(user);
                _employerManager.Update(employer);

                ViewBag.Employees = employer.Employees;
                return View("EmployerProfile", model);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteEmployer(Guid id)
        {
            var user = _userManager.FindById(id);
            _userManager.Delete(user);

            return RedirectToAction("Index", "Advisor");
        }

        [HttpGet]
        public ActionResult Settings()
        {
            //bad solution, go to employer repo, not advisor!!!
            var adv = _advisorManager.Get(
                new User() { UserId = Guid.Parse(User.Identity.GetUserId()) }
                );
            ViewBag.Name = adv.Name;
            return View();
        }

        [HttpGet]
        public ActionResult RegisterEmployer()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterEmployer(AdminRegisterEmployerViewModel model)
        {
            var password = Membership.GeneratePassword(12, 4);

            if (ModelState.IsValid)
            {
                var employer = MapRegisterViewModelToEmployer(model);
                var identityUser = new IdentityUser
                {
                    UserName = model.LoginName,
                    Email = model.EmailAdress,
                };

                var result = await _userManager.CreateAsync(identityUser, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(identityUser.Id, "Employer");
                    var user = await _employerManager.GetUserByIdAsync(identityUser.Id);

                    await _employerManager.CreateAsync(employer, user);

                    await SendEmail(identityUser.Id, new RegistrationMailMessageBuilder(model.LoginName));
                    return RedirectToAction("Index", "Admin");

                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> PasswordChange()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user.Id);

                if (!string.IsNullOrWhiteSpace(token))
                    return View(new EmplPassChangeViewModel { Id = user.Id, Token = token });
            }

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> PasswordChange(EmplPassChangeViewModel chPassVM)
        {
            if (ModelState.IsValid)
            {
                var oldPassValid = false;

                var user = await _userManager.FindByIdAsync(chPassVM.Id);

                if (user != null && (oldPassValid = await _userManager.CheckPasswordAsync(user, chPassVM.OldPassword)))
                {
                    var opResult =
                        await _userManager.ChangePasswordAsync(user.Id, chPassVM.OldPassword, chPassVM.Password);

                    if (opResult.Succeeded)
                        return RedirectToAction("Index");
                }
                else if (!oldPassValid)
                    ModelState.AddModelError(nameof(chPassVM.OldPassword), "Old password is invalid");
            }
            else
                ModelState.AddModelError("", "Server probleem(Probeer a.u.b.later)");

            return View("PasswordChange");
        }

        #region Change employee's name 
        [HttpGet]
        public async Task<ActionResult> ChangeEmployeeName(Guid? id)
        {
            if (id != null && id.Value != Guid.Empty)
            {
                var employee = await _employerManager.GetEmployeeAsync(id.Value);

                if (employee != null)
                {
                    return View(new EmployeeChangeNameViewModel
                    {
                        EmployerId = employee.EmployerId,
                        FirstName = employee.FirstName,
                        Id = employee.EmployeeId,
                        LastName = employee.LastName,
                        Prefix = employee.Prefix
                    });
                }
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmployeeName(EmployeeChangeNameViewModel employeeInfo)
        {
            if (!ModelState.IsValid)
                return View(employeeInfo);

            var employee = await _employerManager.GetEmployeeAsync(employeeInfo.Id);

            if (employee != null)
            {
                employee.FirstName = employeeInfo.FirstName;
                employee.LastName = employeeInfo.LastName;
                employee.Prefix = employeeInfo.Prefix;

                if (await _employerManager.UpdateEmployeeAsync(employee) > 0)
                {
                    var mailInfo =
                        new ChangeEmployeeNameMessageBuilder($"{employee.FirstName} {employee.Prefix} {employee.LastName}");

                    await
                        _mailingService.SendMailAsync(mailInfo.Body, mailInfo.Subject, employee.Employer?.User?.Email);

                    return RedirectToAction("EmployerProfile", new { id = employee.EmployerId });
                }
            }

            ModelState.AddModelError("", SERVER_ERROR);
            return View(employeeInfo);
        }
        #endregion

        #region Delete employee
        [HttpGet]
        public async Task<ActionResult> DeleteEmployee(Guid? id)
        {
            if (id != null && id.Value != Guid.Empty)
            {
                var employee = await _advisorManager.GetEmployeeAsync(id.Value);

                User employer = null;

                if (employee != null
                    && (employer = await _employerManager.GetUserByIdAsync(employee.EmployerId)) != null)
                {
                    _advisorManager.DeleteEmployee(employer, employee);

                    var mailInfo = 
                        new DeleteEmployeeMailMessageBuilder($"{employee.FirstName} {employee.Prefix} {employee.LastName}");

                    await _mailingService.SendMailAsync(mailInfo.Body, mailInfo.Subject,
                        ExtendRecieversMails(await GetAllAdminsEmailsAsync(), employer.Email));
                }
            }


            return RedirectToAction("Index");
        }
        #endregion


        private async Task SendEmail(Guid userId, MailMessageBuilder mailMessageBuilder)
        {
            await _userManager.SendEmailAsync(userId, mailMessageBuilder.Subject, mailMessageBuilder.Body);
        }

        private Employer MapRegisterViewModelToEmployer(AdminRegisterEmployerViewModel model)
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

        #region Helpers
        [NonAction]
        private async Task<string[]> GetAllAdminsEmailsAsync()
        {
            var admins = await _adminManager.GetAllAsync();

            if (admins.Count == 0)
                return null;

            return admins
                .Select(adm => adm.User.Email)
                .ToArray();
        }

        

        [NonAction]
        private string[] ExtendRecieversMails(string[] emails, string extend)
        {
            if (extend == null)
                return emails;
            if (emails == null || emails.Length == 0)
                return new[] { extend };

            var extended = new string[emails.Length + 1];

            Array.Copy(emails, 0, extended, 0, emails.Length);

            extended[extended.Length - 1] = extend;

            return extended;
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager.Dispose();
                _mailingService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}