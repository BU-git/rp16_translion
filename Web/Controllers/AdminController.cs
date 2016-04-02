using System;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser, Guid> _userManager;

        public AdminController(IUserStore<IdentityUser, Guid> store)
        {
            _userManager = new UserManager<IdentityUser, Guid>(store);
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _userManager.Dispose();

            base.Dispose(disposing);
        }
    }
}