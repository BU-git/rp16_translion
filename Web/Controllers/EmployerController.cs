using System;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using IDAL.Interfaces;
using IDAL.Interfaces.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private readonly IEmployerManager _employerManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser, Guid> _userManager;

        public EmployerController(IUserStore<IdentityUser, Guid> store, IEmployerManager employerManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = new UserManager<IdentityUser, Guid>(store);
            _employerManager = employerManager;
            _unitOfWork = unitOfWork;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // GET: Employer
        public ActionResult Index()
        {
            return View();
        }

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
            _employerManager.AddEmployee(employeeViewModel.FirstName, employeeViewModel.Prefix,
                employeeViewModel.LastName, employerId);
            return View("AddEmployeeSuccess");
        }

        //test logout method
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _userManager.Dispose();

            base.Dispose(disposing);
        }
    }
}