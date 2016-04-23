using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Identity.Models;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.MailMessageBuilders;
using BLL.Services.PersonageService;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.ViewModels;

namespace Web.Controllers
{
    public abstract class BaseController : Controller
    {
        #region common problems messages
        internal const string SERVER_ERROR = "Server probleem(Probeer a.u.b.later)";
        internal const string USERNAME_IS_IN_USE_ERROR = "Uw gebruikersnaam is incorrect, controleer dit aub.(In use)";
        internal const string ADVISOR_ROLE = "Advisor";
        #endregion

        internal readonly PersonManager<Admin> adminManager;
        internal readonly PersonManager<Advisor> advisorManager;
        internal readonly PersonManager<Employer> employerManager;
        internal readonly IMailingService mailingService;
        internal readonly UserManager<IdentityUser, Guid> userManager;

        public BaseController(
            UserManager<IdentityUser, Guid> userManager,
            PersonManager<Admin> adminManager,
            PersonManager<Advisor> advisorManager,
            PersonManager<Employer> employerManager,
            IMailingService mailingService)
        {
            this.userManager = userManager;
            this.adminManager = adminManager;
            this.advisorManager = advisorManager;
            this.employerManager = employerManager;
            this.mailingService = mailingService;
        }

        internal IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        #region Employer

        #region Employer profile

        // Admin advisor
        [HttpGet]
        public async Task<ActionResult> EmployerProfile(string Id)
        {
            var user = await employerManager.GetUserByIdAsync(Id);
            var employer = await employerManager.GetAsync(user);

            var model = new EmployerViewModel
            {
                EmployerId = user.UserId,
                EmailAdress = user.Email,
                UserName = user.UserName,
                FirstName = employer.FirstName,
                LastName = employer.LastName,
                CompanyName = employer.CompanyName,
                Adress = employer.Adress,
                City = employer.City,
                Prefix = employer.Prefix,
                PostalCode = employer.PostalCode,
                TelephoneNumber = employer.TelephoneNumber,
                Employees = employer.Employees
            };
            return View(model);
        }

        #endregion

        #region Edit Employer

        // Admin advisor
        [HttpGet]
        public async Task<ActionResult> EditEmployer(string Id)
        {
            var user = await employerManager.GetUserByIdAsync(Id);
            var employer = await employerManager.GetAsync(user);

            var model = new EmployerViewModel
            {
                EmployerId = user.UserId,
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
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditEmployer(EmployerViewModel model, string Id)
        {
            if (ModelState.IsValid)
            {
                var user = await employerManager.GetUserByIdAsync(Id);
                var employer = await employerManager.GetAsync(user);

                user.Email = model.EmailAdress;

                employer.Adress = model.Adress;
                employer.City = model.City;
                employer.CompanyName = model.CompanyName;
                employer.FirstName = model.FirstName;
                employer.LastName = model.LastName;
                employer.PostalCode = model.PostalCode;
                employer.Prefix = model.Prefix;
                employer.TelephoneNumber = model.TelephoneNumber;

                employerManager.Update(employer);

                var messageInfo = new AdminEditEmployerMessageBuilder(
                    user.UserName,
                    employer.CompanyName,
                    employer.FirstName,
                    employer.Prefix,
                    employer.LastName,
                    employer.TelephoneNumber,
                    employer.PostalCode,
                    employer.Adress,
                    employer.City);
                await mailingService.SendMailAsync(messageInfo.Body, messageInfo.Subject, user.Email);

                ViewBag.Employees = employer.Employees;
                return View("EmployerProfile", model);
            }
            return View(model);
        }

        #endregion

        [HttpGet]
        public async Task<ActionResult> DeleteEmployer(string Id)
        {
            var employer = await employerManager.GetUserByIdAsync(Id);
            await adminManager.DeleteAsync(employer);

            var messageInfo = new AdminDeleteEmployerMessageBuilder();
            await mailingService.SendMailAsync(messageInfo.Body, messageInfo.Subject, employer.Email);

            return RedirectToAction("Index");
        }

        #region RegisterEmployer
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
                    Email = model.EmailAdress
                };

                var result = await userManager.CreateAsync(identityUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(identityUser.Id, "Employer");
                    var user = await employerManager.GetUserByIdAsync(identityUser.Id);

                    await employerManager.CreateAsync(employer, user);

                    var messageInfo = new AdminRegEmployerMessageBuilder(model.LoginName, password);
                    var mailingResult =
                        await mailingService.SendMailAsync(messageInfo.Body, messageInfo.Subject, model.EmailAdress);

                    return RedirectToAction("Index", "Admin");
                }
            }
            return View(model);
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

        #endregion

        #endregion

        #region Employee

        #region Add Employee
        [HttpGet]
        public ActionResult AddEmployee(string id)
        {
            ViewBag.EmployerId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployee(AddEmployeeViewModel model, string id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employer = await adminManager.GetUserByIdAsync(id);

            var employee = new Employee()
            {
                EmployeeId = Guid.NewGuid(),
                LastName = model.LastName,
                FirstName = model.FirstName,
                Prefix = model.Prefix
            };

            await adminManager.CreateEmployeeAsync(employee, employer);

            var messageInfo = new AdminAddEmployeeMessageBuilder($"{employee.FirstName} {employee.Prefix} {employee.LastName}");
            await mailingService.SendMailAsync(messageInfo.Body, messageInfo.Subject, employer.Email);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete Employee

        [HttpGet]
        public async Task<ActionResult> DeleteEmployee(Guid? id)
        {
            if (id != null)
            {
                var employee = await advisorManager.GetEmployeeAsync(id.Value);

                var employer = await employerManager.GetUserByIdAsync(employee.EmployerId);



                if (employee != null && employer != null)
                {
                    advisorManager.DeleteEmployee(employer, employee);

                    // TODO: send mails to admins also
                    var mailInfo =
                       new DeleteEmployeeMailMessageBuilder($"{employee.FirstName} {employee.Prefix} {employee.LastName}");

                    await mailingService.SendMailAsync(mailInfo.Body, mailInfo.Subject, employer.Email);
                }
            }

            return RedirectToAction("Index");
        }


        #endregion

        #region Change Employee's name
        [HttpGet]
        public async Task<ActionResult> ChangeEmployeeName(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return RedirectToAction("Index");

            var employee = await GetEmployeeAsync(id);

            if (employee == null)
                return RedirectToAction("Index");

            return View(new EmployeeChangeNameViewModel
            {
                EmployerId = employee.EmployerId,
                FirstName = employee.FirstName,
                Id = employee.EmployeeId,
                LastName = employee.LastName,
                Prefix = employee.Prefix
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmployeeName(EmployeeChangeNameViewModel employeeInfo)
        {
            if (!ModelState.IsValid || employeeInfo.Id == Guid.Empty)
                return View(employeeInfo);

            var employee = await adminManager.GetEmployeeAsync(employeeInfo.Id);

            if (employee != null)
            {
                employee.FirstName = employeeInfo.FirstName;
                employee.LastName = employeeInfo.LastName;
                employee.Prefix = employeeInfo.Prefix;

                if (await adminManager.UpdateEmployeeAsync(employee) > 0)
                {
                    var messageInfo =
                        new ChangeEmployeeNameMessageBuilder($"{employee.FirstName} {employee.Prefix} {employee.LastName}");

                    await mailingService.SendMailAsync(messageInfo.Body, messageInfo.Subject,
                            employee.Employer.User.Email);

                    return RedirectToAction("EmployerProfile", new { id = employee.EmployerId });
                }
            }

            ModelState.AddModelError("", SERVER_ERROR);
            return View(employeeInfo);
        }

        [NonAction]
        private Task<Employee> GetEmployeeAsync(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return null;

            return adminManager.GetEmployeeAsync(id.Value);
        }
        #endregion

        #endregion

        public ActionResult Index()
        {
            ViewBag.Employers = employerManager.GetAll();
            return View();
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Settings()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<ActionResult> PasswordChange()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user.Id);

                if (!string.IsNullOrWhiteSpace(token))
                    return View(new ChangePasswordViewModel { Id = user.Id, Token = token });
            }

            return View("Index");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> PasswordChange(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldPassValid = false;

                var user = await userManager.FindByIdAsync(model.Id);

                if (user != null && (oldPassValid = await userManager.CheckPasswordAsync(user, model.OldPassword)))
                {
                    var opResult =
                        await userManager.ChangePasswordAsync(user.Id, model.OldPassword, model.Password);

                    if (opResult.Succeeded)
                        return RedirectToAction("Index");
                }
                else if (!oldPassValid)
                    ModelState.AddModelError(nameof(model.OldPassword), "Old password is invalid");
            }
            else
                ModelState.AddModelError("", "Server probleem(Probeer a.u.b.later)");

            return View("PasswordChange");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userManager.Dispose();
                mailingService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}