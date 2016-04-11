using System;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.PersonageService;
using IDAL.Interfaces;
using IDAL.Interfaces.Managers;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser, Guid> _userManager;
        private readonly PersonageManager<Employer> _employerManager;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public AdminController(IUserStore<IdentityUser, Guid> store, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _userManager = new UserManager<IdentityUser, Guid>(store);
            _employerManager = new EmployerManager(_unitOfWork);
        }

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Employers = _employerManager.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult EditEmployer(Guid employerId)
        {
            //Guid id = new Guid(employerId);
            Guid id = employerId;

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

            return View(model);
        }

        [HttpPost]
        public ActionResult EditEmployer(EmployerViewModel model, string employerId)
        {
            if (ModelState.IsValid)
            {
                Guid id = new Guid(employerId);

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
            }

            return View(model);
        }

        public ActionResult DeleteEmployer(string employerId)
        {
            Guid id = new Guid(employerId);

            var user = _userManager.FindById(id);
            //Employer employer = _unitOfWork.EmployerRepository.FindById(id);

            //_employerManager.Delete(employer);
            _userManager.Delete(user);

            return View("Index");
        }

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