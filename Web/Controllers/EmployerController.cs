using IDAL.Interfaces;
using IDAL.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.PersonageService;
using IDAL.Interfaces.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private readonly PersonManager<Employer> _employerManager;
        private readonly UserManager<IdentityUser, Guid> _userManager;

        public EmployerController(IUserStore<IdentityUser, Guid> store, PersonManager<Employer> employerManager)
        {
            _userManager = new UserManager<IdentityUser, Guid>(store);
            _userManager.UserTokenProvider =
                   new DataProtectorTokenProvider<IdentityUser, Guid>(
                          new DpapiDataProtectionProvider("Sample").Create("EmailConfirmation"));

            _employerManager = employerManager;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // GET: Employer
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var user = await _employerManager.GetUserByIdAsync(User.Identity.GetUserId());

            if (user?.Employer != null)
                return View(user.Employer.Employees);

            return RedirectToAction("Logout");
        }

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

            var user = await _employerManager.GetUserByIdAsync(User.Identity.GetUserId());

            if (user != null)
            {
                await _employerManager.CreateEmployeeAsync(new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    LastName = employeeViewModel.LastName,
                    FirstName = employeeViewModel.FirstName,
                    Prefix = employeeViewModel.Prefix
                }, user);

                return View("AddEmployeeSuccess");
            }

            return View();
        }
        #endregion

        #region Change employee's name
        [HttpGet]
        public async Task<ActionResult> ChangeEmployeeName(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return RedirectToAction("Index");

            var user = await _employerManager.GetUserByIdAsync(User.Identity.GetUserId());

            if (user?.Employer != null 
                && user.Employer.Employees.Any(empl => empl.EmployeeId == id.Value))
            {
                var employee = user.Employer.Employees.First(empl => empl.EmployeeId == id.Value);

                return View(new EmployeeChangeNameViewModel
                {
                    Id = employee.EmployeeId,
                    EmployerId = employee.EmployerId,
                    FirstName = employee.FirstName,
                    Prefix = employee.Prefix,
                    LastName = employee.LastName
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "AntiForgeryError")]
        public async Task<ActionResult> ChangeEmployeeName(EmployeeChangeNameViewModel emplInfo)
        {
            if (!ModelState.IsValid)
                return View();

            Employee employee;

            if (emplInfo.Id != Guid.Empty 
                && (employee = await _employerManager.GetEmployeeAsync(emplInfo.Id)) != null 
                && emplInfo.EmployerId == employee.EmployerId)
            {
                employee.FirstName = emplInfo.FirstName;
                employee.LastName = emplInfo.LastName;
                employee.Prefix = emplInfo.Prefix;

                await _employerManager.UpdateEmployeeAsync(employee);
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
            var user = await _employerManager.GetUserByIdAsync(User.Identity.GetUserId());

            if (user != null)
            {
                var employer = await _employerManager.GetAsync(user);

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

        #region PasswordChange
        [HttpGet]
        public async Task<ActionResult> PasswordChange()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                String token = await _userManager.GeneratePasswordResetTokenAsync(user.Id);

                if (!String.IsNullOrWhiteSpace(token))
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
                bool oldPassValid = false;

                var user = await _userManager.FindByIdAsync(chPassVM.Id);

                if (user != null && (oldPassValid = await _userManager.CheckPasswordAsync(user, chPassVM.OldPassword)))
                {
                    var opResult = await _userManager.ChangePasswordAsync(user.Id, chPassVM.OldPassword, chPassVM.Password);

                    if (opResult.Succeeded)
                        return View("Index");
                }
                else if (!oldPassValid)
                    ModelState.AddModelError(nameof(chPassVM.OldPassword), "Old password is invalid");
            }
            else
                ModelState.AddModelError("", "Server probleem(Probeer a.u.b.later)");

            return View("PasswordChange");
        }
        #endregion

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _userManager.Dispose();

            base.Dispose(disposing);
        }
        #endregion
    }
}