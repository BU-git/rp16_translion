using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BLL.Services.TestService;
using BLL.Services.TestService.Interfaces;
using IDAL.Interfaces;
using IDAL.Models;

namespace Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly ITestService _testService;

        public ReportController(IUnitOfWork uow)
        {
            _testService = new TestManager(uow);    
        }

        [HttpGet]
        public ViewResult CreateReport()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> AddPages(Page[] pages)
        {
            if (!ModelState.IsValid || pages == null || pages.Length == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not all pages are valid");

            await _testService.DeleteAllPages(); //deleting old template

            await _testService.AddPages(pages);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public async Task<ViewResult> GetAllPages()
        {
            var pages = await _testService.GetAllPages() 
                ?? new List<Page>();

            return View(pages.OrderBy(p => p.Order).ToList());
        }

        [HttpPost]
        public async Task<ActionResult> LoadPreview(FormCollection formCollection)
        {
            return View();
        }

    }
}