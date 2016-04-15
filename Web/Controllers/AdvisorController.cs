using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
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
        private readonly PersonManager<Employer> _employerManager;
        private readonly UserManager<IdentityUser, Guid> _userManager;
        private readonly IUnitOfWork _unitOfWork;


        public AdvisorController(IUnitOfWork uow, IUserStore<IdentityUser, Guid> store)
        {
            _unitOfWork = uow;
            _employerManager = new EmployerManager(uow);
            _userManager=new UserManager<IdentityUser, Guid>(store);
            _userManager.UserTokenProvider =
                new DataProtectorTokenProvider<IdentityUser, Guid>(
                    new DpapiDataProtectionProvider("Sample").Create("EmailConfirmation"));
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
        public ActionResult EmployerProfile (Guid id)
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

            return View (model);
        }

        [HttpGet]
        public ActionResult DeleteEmployer(Guid id)
        {
            var user = _userManager.FindById(id);
            _userManager.Delete(user);

            return RedirectToAction("Index","Advisor");
        }

        [HttpGet]
        public ActionResult Settings()
        {
            //bad solution, go to employer repo, not advisor!!!
            var emp = _employerManager.Get(
                new User() { UserId = Guid.Parse(User.Identity.GetUserId()) }
                );
            ViewBag.Name = $"{emp.FirstName} {emp.LastName}";
            return View();
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
    }
}