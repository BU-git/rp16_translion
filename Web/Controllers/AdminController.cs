﻿using System;
using System.Linq;
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
using Microsoft.Owin.Security;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        #region common problems messages
        private const string SERVER_ERROR = "Server probleem(Probeer a.u.b.later)";
        private const string USERNAME_IS_IN_USE_ERROR = "Uw gebruikersnaam is incorrect, controleer dit aub.(In use)";
        private const string ADVISOR_ROLE = "Advisor";
        #endregion

        private readonly UserManager<IdentityUser, Guid> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PersonManager<Admin> _adminManager;
        private readonly PersonManager<Advisor> _advisorManager;
        private readonly PersonManager<Employer> _employerManager;

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private readonly IMailingService _mailingService;

        public AdminController(IUserStore<IdentityUser, Guid> store, PersonManager<Admin> adminManager,
            PersonManager<Advisor> advisorManager, PersonManager<Employer> employerManager, IUnitOfWork unitOfWork, IMailingService mailService)
        {
            _unitOfWork = unitOfWork;

            _userManager = new UserManager<IdentityUser, Guid>(store);

            _adminManager = adminManager;
            _advisorManager = advisorManager;

            _employerManager = employerManager;

            _mailingService = mailService;
        }

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Employers = _employerManager.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult AddEmployee(Guid id)
        {
            ViewBag.EmployerId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployee(AddEmployeeViewModel model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employer = await _adminManager.GetUserByIdAsync(id);

            var employee = new Employee()
            {
                EmployeeId = Guid.NewGuid(),
                LastName = model.LastName,
                FirstName = model.FirstName,
                Prefix = model.Prefix
            };

            await _adminManager.CreateEmployeeAsync(employee, employer);

            var messageInfo = new AdminAddEmployeeMessageBuilder($"{employee.FirstName} {employee.Prefix} {employee.LastName}");

            await _mailingService.SendMailAsync(messageInfo.Body, messageInfo.Subject,
                    employer.Email);

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
        public async Task<ActionResult> DeleteEmployer(Guid? id)
        {
            var employer = await GetEmployerAsUser(id);
            await _adminManager.DeleteAsync(employer);

            return RedirectToAction("Index");
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

        #region Change employee's name
        [HttpGet]
        public async Task<ActionResult> ChangeEmployeeName(Guid? id)
        {
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

            var employee = await _adminManager.GetEmployeeAsync(employeeInfo.Id);

            if (employee != null)
            {
                employee.FirstName = employeeInfo.FirstName;
                employee.LastName = employeeInfo.LastName;
                employee.Prefix = employeeInfo.Prefix;

                if (await _adminManager.UpdateEmployeeAsync(employee) > 0)
                {
                    var messageInfo =
                        new ChangeEmployeeNameMessageBuilder($"{employee.FirstName} {employee.Prefix} {employee.LastName}");

                    await _mailingService.SendMailAsync(messageInfo.Body, messageInfo.Subject,
                            employee.Employer.User.Email);

                    return RedirectToAction("EmployerProfile", new { id = employee.EmployerId });
                }
            }

            ModelState.AddModelError("", SERVER_ERROR);
            return View(employeeInfo);
        }
        #endregion

        #region Add advisor

        [HttpGet]
        public ViewResult AddAdvisor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAdvisor(CreateAdvisorViewModel advisorInfo)
        {
            if (!ModelState.IsValid)
                return View(advisorInfo);


            if (await _adminManager.GetUserByNameAsync(advisorInfo.Username) != null)
            {
                ModelState.AddModelError(nameof(advisorInfo.Username), USERNAME_IS_IN_USE_ERROR);
                return View(advisorInfo);
            }

            var creationRes = await
                _userManager.CreateAsync(new IdentityUser { Id = Guid.NewGuid(), UserName = advisorInfo.Username },
                        advisorInfo.Password);

            User user = null;

            if (creationRes.Succeeded
                && (user = await _adminManager.GetUserByNameAsync(advisorInfo.Username)) != null)
            {
                await _advisorManager.CreateAsync(new Advisor { Name = advisorInfo.Name }, user);

                var roleResult = await _userManager.AddToRoleAsync(user.UserId, ADVISOR_ROLE);

                if (roleResult.Succeeded)
                    return RedirectToAction("Settings");
            }

            ModelState.AddModelError("", SERVER_ERROR);
            return View(advisorInfo);
        }

        [HttpGet]
        public async Task<JsonResult> CheckAdvisorName(String userName)
        {
            if (String.IsNullOrWhiteSpace(userName))
                return Json(false, JsonRequestBehavior.AllowGet);

            return Json(await _adminManager.GetUserByNameAsync(userName) == null,
                JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region All advisors
        [HttpGet]
        public async Task<ActionResult> AdvisorsList()
        {
            return View(await _advisorManager.GetAllAsync());
        }
        #endregion

        #region Advisor's info
        [HttpGet]
        public async Task<ActionResult> AdvisorInfo(Guid? id)
        {
            var user = await GetUserIfAdvisorAsync(id);

            if (user != null)
                return View(user.Advisor);

            return RedirectToAction("AdvisorsList");
        }
        #endregion

        #region Remove advisor
        [HttpGet]
        public async Task<ActionResult> RemoveAdvisor(Guid? id)
        {
            var user = await GetUserIfAdvisorAsync(id);

            if (user != null && await _adminManager.DeleteAsync(user) > 0)
                return View("Settings");

            return RedirectToAction("AdvisorsList");
        }
        #endregion

        #region Change advisor's name
        [HttpGet]
        public async Task<ActionResult> ChangeAdvisorName(Guid? id)
        {
            var user = await GetUserIfAdvisorAsync(id);

            if (user == null)
                return RedirectToAction("AdvisorsList");

            return View(new AdvisorChangeNameViewModel
            {
                Id = user.UserId,
                Name = user.Advisor.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeAdvisorName(AdvisorChangeNameViewModel advInfo)
        {
            if (!ModelState.IsValid)
                return View(advInfo);

            var user = await GetUserIfAdvisorAsync(advInfo.Id);

            if (user != null)
            {
                user.Advisor.Name = advInfo.Name;

                if (await _advisorManager.UpdateAsync(user.Advisor) > 0)
                    return View("Settings");
            }

            ModelState.AddModelError("", SERVER_ERROR);
            return View(advInfo);
        }
        #endregion

        #region Helpers
        private async Task SendEmail(Guid userId, MailMessageBuilder mailMessageBuilder)
        {
            await _userManager.SendEmailAsync(userId, mailMessageBuilder.Subject, mailMessageBuilder.Body);
        }

        [NonAction]
        private async Task<User> GetUserIfAdvisorAsync(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return null;

            var user = await _adminManager.GetUserByIdAsync(id.Value);

            return user?.Advisor != null ? user : null;
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


        [NonAction]
        private async Task<Employer> GetEmployerAsync(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return null;

            return await _unitOfWork.EmployerRepository.FindByIdAsync(id);
        }

        [NonAction]
        private async Task<User> GetEmployerAsUser(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return null;

            var user = await _adminManager.GetUserByIdAsync(id.Value);

            return user?.Employer != null ? user : null;
        }

        [NonAction]
        private Task<Employee> GetEmployeeAsync(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return null;

            return _adminManager.GetEmployeeAsync(id.Value);
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