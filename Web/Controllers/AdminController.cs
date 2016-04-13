using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Services.PersonageService;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        #region common problems messages
        private const string SERVER_ERROR = "Server probleem(Probeer a.u.b.later)";
        private const string USERNAME_IS_IN_USE_ERROR = "Uw gebruikersnaam is incorrect, controleer dit aub.(In use)";
        #endregion

        private readonly UserManager<IdentityUser, Guid> _userManager;
        private readonly PersonManager<Admin> _adminManager;
        private readonly PersonManager<Advisor> _advisorManager;
        public AdminController(IUserStore<IdentityUser, Guid> store, PersonManager<Admin> adminManager,
            PersonManager<Advisor> advisorManager)
        {
            _userManager = new UserManager<IdentityUser, Guid>(store);
            _adminManager = adminManager;
            _advisorManager = advisorManager;
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
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ViewResult Settings()
        {
            return View();
        }

        #region Add advisor

        [HttpGet]
        public ViewResult AddAdvisor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAdvisor(CreateAdvisorViewModel advisorInfo)
        {
            if (!ModelState.IsValid)
                return View(advisorInfo);

            if (await _adminManager.GetUserByNameAsync(advisorInfo.Username) != null)
            {
                ModelState.AddModelError(nameof(advisorInfo.Username), USERNAME_IS_IN_USE_ERROR);
                return View(advisorInfo);
            }

            var creationRes = await 
                _userManager.CreateAsync( new IdentityUser {Id = Guid.NewGuid(), UserName = advisorInfo.Username},
                        advisorInfo.Password);

            User user = null;

            if (!creationRes.Succeeded 
                || (user = await _adminManager.GetUserByNameAsync(advisorInfo.Username)) == null)
            {
                ModelState.AddModelError("", SERVER_ERROR);
                return View(advisorInfo);
            }
        
            await _advisorManager.CreateAsync(new Advisor { Name = advisorInfo.Name }, user);

            return RedirectToAction("Settings");
        }

        [HttpGet]
        public async Task<JsonResult> CheckAdvisorName(String userName)
        {
            if (String.IsNullOrWhiteSpace(userName))
                return Json(false, JsonRequestBehavior.AllowGet);

            return Json(await _adminManager.GetUserByNameAsync(userName) == null,
                JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region All advisors
        [HttpGet]
        public async Task<ActionResult> AdvisorsList()
        {
            return View(await _advisorManager.GetAllAsync());
        }
        #endregion

        #region Advisor's info
        [HttpGet]
        public async Task<ActionResult> AdvisorInfo(Guid? id)
        {
            var user = await GetUserIfAdvisorAsync(id);

            if (user != null)
                return View(user.Advisor);

            return RedirectToAction("AdvisorsList");
        }
        #endregion

        #region Remove advisor
        [HttpGet]
        public async Task<ActionResult> RemoveAdvisor(Guid? id)
        {
            var user = await GetUserIfAdvisorAsync(id);

            if (user != null && await _adminManager.DeleteAsync(user) > 0)
                return View("Settings");

            return RedirectToAction("AdvisorsList");
        }
        #endregion

        #region Change advisor's name
        [HttpGet]
        public async Task<ActionResult> ChangeAdvisorName(Guid? id)
        {
            var user = await GetUserIfAdvisorAsync(id);

            if (user == null)
                return RedirectToAction("AdvisorsList");

            return View(new AdvisorChangeNameViewModel
            {
                Id = user.UserId,
                Name = user.Advisor.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeAdvisorName(AdvisorChangeNameViewModel advInfo)
        {
            if (!ModelState.IsValid)
                return View(advInfo);

            var user = await GetUserIfAdvisorAsync(advInfo.Id);

            if (user != null)
            {
                user.Advisor.Name = advInfo.Name;

                if (await _advisorManager.UpdateAsync(user.Advisor) > 0)
                    return View("Settings");
            }

            ModelState.AddModelError("", SERVER_ERROR);
            return View(advInfo);
        }
        #endregion

        #region Helpers

        [NonAction]
        public async Task<User> GetUserIfAdvisorAsync(Guid? id)
        {
            if (id == null && id.Value == Guid.Empty)
                return null;

            var user = await _adminManager.GetUserByIdAsync(id.Value);

            return user?.Advisor != null ? user : null;
        }

        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _userManager.Dispose();

            base.Dispose(disposing);
        }
    }
}