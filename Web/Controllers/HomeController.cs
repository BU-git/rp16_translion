using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.MailingService;
using BLL.Services.PersonageService;
using IDAL.Models;
using Microsoft.AspNet.Identity;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PersonManager<Advisor> _userManager;

        public HomeController(PersonManager<Advisor> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var roles = await _userManager.GetUserRolesById(User.Identity.GetUserId());

            return roles.Count == 0
                ? RedirectToAction("Login", "Account")
                : RedirectToAction("Index", roles.First().Name);
        }
    }
}