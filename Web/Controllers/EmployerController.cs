using IDAL.Interfaces;
using IDAL.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using IDAL.Interfaces.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private readonly IPersonageManager<Employer> _employerManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser, Guid> _userManager;

        public EmployerController(IUserStore<IdentityUser, Guid> store, IPersonageManager<Employer> employerManager, IUnitOfWork unitOfWork)
        {
            _userManager = new UserManager<IdentityUser, Guid>(store);

            _userManager.UserTokenProvider =
                   new DataProtectorTokenProvider<IdentityUser, Guid>(
                          new DpapiDataProtectionProvider("Sample").Create("EmailConfirmation"));
            _employerManager = employerManager;
            _unitOfWork = unitOfWork;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // GET: Employer
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof (HttpAntiForgeryException), View = "AntiForgeryError")]
        public ActionResult AddEmployee(AddEmployeeViewModel employeeViewModel)
        {
            if (!ModelState.IsValid)
                return View(employeeViewModel);

            var employerId = Guid.Parse(User.Identity.GetUserId());
            //TODO Need fix
            //_employerManager.AddEmployee(employeeViewModel.FirstName, employeeViewModel.Prefix,
            //    employeeViewModel.LastName, employerId);
            return View("AddEmployeeSuccess");
        }

        //test logout method
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ViewResult> Settings()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                var employer = await _unitOfWork.EmployerRepository.FindByIdAsync(user.Id);

                if (employer != null)
                    return View(employer);
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