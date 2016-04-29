using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.AlertService;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.MailMessageBuilders;
using BLL.Services.PersonageService;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private readonly PersonManager<Admin> _adminManager;
        private readonly PersonManager<Employer> _employerManager;
        private readonly IMailingService _mailingService;
        private readonly UserManager<IdentityUser, Guid> _userManager;
        private readonly AlertManager _alertManager;

        public EmployerController(IUserStore<IdentityUser, Guid> store, PersonManager<Employer> employerManager,
            IMailingService mailService, PersonManager<Admin> adminManager, AlertManager alertManager)
        {
            _userManager = new UserManager<IdentityUser, Guid>(store);
            _userManager.UserTokenProvider =
                new DataProtectorTokenProvider<IdentityUser, Guid>(
                    new DpapiDataProtectionProvider("Sample").Create("EmailConfirmation"));

            _employerManager = employerManager;

            _adminManager = adminManager;
            _alertManager = alertManager;

            _mailingService = mailService;
            _mailingService.IgnoreQueue(); //setting manager to ignore mail message queue and tell about errors
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // GET: Employer
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var user = await _employerManager.GetBaseUserByGuid(User.Identity.GetUserId());

            if (user?.Employer != null)
                return View(user.Employer.Employees.Where(empl => !empl.IsDeleted));

            return RedirectToAction("Logout");
        }

        #region Employee info

        [HttpGet]
        public async Task<ActionResult> EmployeeInfo(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return RedirectToAction("Index");

            var employee = await GetEmployeeByIdAsync(id.Value);

            if (employee == null)
                return RedirectToAction("Index");

            return View(new EmployeeInfoViewModel
            {
                Id = employee.EmployeeId,
                Reports = new string[] {}, //TODO: change this in future
                FullName = $"{employee.FirstName} {employee.Prefix} {employee.LastName}"
            });
        }

        #endregion

        #region Remove employee

        [HttpGet]
        public async Task<ActionResult> RemoveEmployee(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return RedirectToAction("Index");

            var employee = await GetEmployeeByIdAsync(id.Value);

            if (employee != null && !employee.IsDeleted)
            {
                employee.IsDeleted = true;
                var alert = new Alert()
                {
                    AlertId = Guid.NewGuid(),
                    AlertEmployerId = employee.EmployerId,
                    AlertType = AlertType.Employee_Delete,
                    AlertComment = "",
                    AlertIsDeleted = false,
                    AlertCreateTS = DateTime.Now,
                    AlertUpdateTS = DateTime.Now
                };

                //await _alertManager.CreateAsync(alert);

                await _employerManager.DeleteEmployee(employee);

                var messageInfo = new EmployerDelEmployeeMessageBuilder(User.Identity.Name,
                    $"{employee.FirstName} {employee.Prefix} {employee.LastName}");

                await _mailingService.SendMailAsync(messageInfo.Body, messageInfo.Subject,
                    await GetAllAdminsEmailsAsync());
            }

            return RedirectToAction("Index");
        }

        #endregion

        //test logout method
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ViewResult> Settings()
        {
            var user = await _employerManager.GetBaseUserByGuid(User.Identity.GetUserId());

            if (user != null)
            {
                var employer = await _employerManager.Get(user.UserId);

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

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager.Dispose();
                _mailingService.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Add employee

        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof (HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> AddEmployee(AddEmployeeViewModel employeeViewModel)
        {
            if (!ModelState.IsValid)
                return View(employeeViewModel);

            var user = await _employerManager.GetBaseUserByGuid(User.Identity.GetUserId());

            if (user == null)
                return RedirectToAction("Logout");
            Employee employee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                EmployerId =  user.UserId,
                LastName = employeeViewModel.LastName,
                FirstName = employeeViewModel.FirstName,
                Prefix = employeeViewModel.Prefix,
                IsApprove = false

            };
            var alert = new Alert()
            {
                AlertId = Guid.NewGuid(),
                AlertEmployerId = user.UserId,
                AlertType = AlertType.Employee_Add,
                AlertComment = "",
                AlertIsDeleted = false,
                AlertCreateTS = DateTime.Now,
                AlertUpdateTS = DateTime.Now
            };
            //alert.Employees.Add(employee);
            //employee.Alerts.Add(alert);

            await _employerManager.CreateEmployee(employee);

            await _alertManager.CreateAsync(alert);
            var mailMessageData = new CreateEmployeeMailMessageBuilder(User.Identity.Name,
                $"{employeeViewModel.FirstName} {employeeViewModel.Prefix} {employeeViewModel.LastName}");

            await _mailingService.SendMailAsync(mailMessageData.Body, mailMessageData.Subject,
                await GetAllAdminsEmailsAsync());

            return View("AddEmployeeSuccess");
        }

        #endregion

        #region Change employee's name

        [HttpGet]
        public async Task<ActionResult> ChangeEmployeeName(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return RedirectToAction("Index");

            var employee = await GetEmployeeByIdAsync(id.Value);

            if (employee == null)
                return RedirectToAction("Index");

            return View(new EmployeeChangeNameViewModel
            {
                Id = employee.EmployeeId,
                EmployerId = employee.EmployerId,
                FirstName = employee.FirstName,
                Prefix = employee.Prefix,
                LastName = employee.LastName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof (HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> ChangeEmployeeName(EmployeeChangeNameViewModel emplInfo)
        {
            if (!ModelState.IsValid)
                return View();
            Employee employee = await _employerManager.GetEmployee(emplInfo.Id);
            if ((employee != null))
            {
                var alert = new Alert()
                {
                    AlertId = Guid.NewGuid(),
                    AlertEmployerId = employee.EmployerId,
                    AlertType = AlertType.Employee_Rename,
                    AlertComment = employee.LastName + " " + employee.FirstName, //old name
                    AlertIsDeleted = false,
                    AlertCreateTS = DateTime.Now,
                    AlertUpdateTS = DateTime.Now
                };

                //_alertManager.Create(alert);

                employee.FirstName = emplInfo.FirstName;
                employee.LastName = emplInfo.LastName;
                employee.Prefix = emplInfo.Prefix;

                await _employerManager.UpdateEmployee(employee);

                var user = await _employerManager.GetBaseUserByGuid(User.Identity.GetUserId());

                var mailMessageData = 
                    new ChangeEmployeeNameMessageBuilder($"{employee.FirstName} {employee.Prefix} {employee.LastName}");

                await _mailingService.SendMailAsync(mailMessageData.Body, mailMessageData.Subject,
                    ExtendRecieversMails(await GetAllAdminsEmailsAsync(), user?.Email));
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region PasswordChange

        [HttpGet]
        public async Task<ActionResult> PasswordChange()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user.Id);

                if (!string.IsNullOrWhiteSpace(token))
                    return View(new ChangePasswordViewModel() {Id = user.Id, Token = token});
            }

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof (HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> PasswordChange(ChangePasswordViewModel chPassVM)
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

        #endregion

        #region Helpers

        [NonAction]
        private async Task<string[]> GetAllAdminsEmailsAsync()
        {
            var admins = await _adminManager.GetAll();

            if (admins.Count == 0)
                return null;

            return admins
                .Select(adm => adm.User.Email)
                .ToArray();
        }

        [NonAction]
        private async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            var user = await _employerManager.GetBaseUserByGuid(User.Identity.GetUserId());

            return user?.Employer != null
                   && user.Employer.Employees.Any(empl => empl.EmployeeId == id)
                ? user.Employer.Employees.First(empl => empl.EmployeeId == id)
                : null;
        }

        [NonAction]
        private string[] ExtendRecieversMails(string[] emails, string extend)
        {
            if (extend == null)
                return emails;
            if (emails == null || emails.Length == 0)
                return new[] {extend};

            var extended = new string[emails.Length + 1];

            Array.Copy(emails, 0, extended, 0, emails.Length);

            extended[extended.Length - 1] = extend;

            return extended;
        }

        #endregion
    }
}