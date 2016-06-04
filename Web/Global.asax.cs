using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BLL.Services.MailingService;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.ReminderService;
using IDAL.Interfaces;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            UnityConfig.RegisterComponents();

            // ----- Initialize parameters for sheduler -----

            var container = new UnityContainer();
            container.LoadConfiguration();
            IUnitOfWork unitOfWork = container.Resolve<IUnitOfWork>();
            IMailingService mailingService = UnityConfig.GetMailingService();

            EmailSheduler.Start(unitOfWork, mailingService);
        }
    }
}