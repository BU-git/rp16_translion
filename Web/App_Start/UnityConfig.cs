using System;
using System.Configuration;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Identity.Stores;
using BLL.Services.AlertService;
using BLL.Services.MailingService;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.PersonageService;
using BLL.Services.ReportService;
using IDAL.Interfaces;
using IDAL.Interfaces.IManagers;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Unity.Mvc5;

namespace Web
{
    public static class UnityConfig
    {
        private static UnityContainer container;

        public static void RegisterComponents()
        {
            container = new UnityContainer();
            container.LoadConfiguration();

            container.RegisterType<IUserStore<IdentityUser, Guid>, UserStore>(new PerHttpRequestLifetimeManager());
            container.RegisterType<UserManager<IdentityUser, Guid>>(new InjectionFactory(
                x => GetUserManager()
                ));

            container.RegisterType<IAlertManager, AlertManager>(new PerHttpRequestLifetimeManager(),
                new InjectionConstructor(
                    container.Resolve<IUnitOfWork>()
                    ));

            container.RegisterType<RoleStore>(new PerHttpRequestLifetimeManager());
            container.RegisterType<IMailingService, MailingService>(new PerHttpRequestLifetimeManager(),
                new InjectionFactory(
                    x => GetMailingService()));

            container.RegisterType<PersonManager<Employer>, EmployerManager>(new PerHttpRequestLifetimeManager(),
                new InjectionConstructor(
                    container.Resolve<IUnitOfWork>()
                    ));
            container.RegisterType<PersonManager<Admin>, AdminManager>(new PerHttpRequestLifetimeManager(),
                new InjectionConstructor(
                    container.Resolve<IUnitOfWork>()
                    ));
            container.RegisterType<PersonManager<Advisor>, AdvisorManager>(new PerHttpRequestLifetimeManager(),
                new InjectionConstructor(
                    container.Resolve<IUnitOfWork>()
                    ));
            container.RegisterType<ReportPassingManager>(new PerHttpRequestLifetimeManager(),
                new InjectionConstructor(
                    container.Resolve<IUnitOfWork>()
                    ));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static UserManager<IdentityUser, Guid> GetUserManager()
        {
            IUnitOfWork unitOfWork = container.Resolve<IUnitOfWork>();
            IUserStore<IdentityUser, Guid> userStore = new UserStore(unitOfWork);
            UserManager<IdentityUser, Guid> userManager = new UserManager<IdentityUser, Guid>(userStore);
            userManager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser, Guid>(
                new DpapiDataProtectionProvider("Sample").Create("EmailConfirmation"));
            return userManager;
        }

        internal static IMailingService GetMailingService()
        {
            string from = ConfigurationManager.AppSettings["mailFrom"];
            string password = ConfigurationManager.AppSettings["mailPass"];
            string host = ConfigurationManager.AppSettings["mailHost"];

            IMailingService mailingService = new MailingService(from, password, host);
            mailingService.IgnoreQueue();
            return mailingService;
        }
    }
}