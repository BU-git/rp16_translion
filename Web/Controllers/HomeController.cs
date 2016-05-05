using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectIfSignedIn() ?? RedirectToAction("Login", "Account");
        }

        [NonAction]
        private ActionResult RedirectIfSignedIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    RedirectToAction("Index", "Admin");
                }
                if (User.IsInRole("Advisor"))
                {
                    RedirectToAction("Index", "Advisor");
                }
                if (User.IsInRole("Employer"))
                {
                    RedirectToAction("Index", "Employer");
                }
            }
            return null;
        }
    }
}