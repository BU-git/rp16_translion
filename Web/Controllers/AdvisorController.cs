using System.Net;
using System.Web;
using System.Web.Mvc;
using BLL.Services.PersonageService;
using IDAL.Interfaces;
using IDAL.Models;
using Microsoft.Owin.Security;

namespace Web.Controllers
{
    [Authorize(Roles = "Advisor")]
    public class AdvisorController : Controller
    {
        private readonly PersonManager<Employer> _employerManager;
        public AdvisorController(IUnitOfWork uow)
        {
            _employerManager = new EmployerManager(uow);
        }
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        public ActionResult Index()
        {
            var employers = _employerManager.GetAll();
            return View(employers);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index");
        }
    }
}