using BLL.Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces;
using IDAL.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        readonly UserManager<IdentityUser, Guid> _userManager;
        private readonly IEmployerManager _employerManager;
        private readonly IUnitOfWork _unitOfWork;

        IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public EmployerController(IUserStore<IdentityUser, Guid> store, IEmployerManager employerManager, IUnitOfWork unitOfWork)
        {
            _userManager = new UserManager<IdentityUser, Guid>(store);
            _employerManager = employerManager;
            _unitOfWork = unitOfWork;
        }

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
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "AntiForgeryError")]
        public ActionResult AddEmployee(AddEmployeeViewModel employeeViewModel)
        {
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