using BLL.Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        readonly UserManager<IdentityUser, Guid> _userManager;

        IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public EmployerController(IUserStore<IdentityUser, Guid> store)
        {
            _userManager = new UserManager<IdentityUser, Guid>(store);
        }

        // GET: Employer
        public ActionResult Index()
        {
            return View();
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