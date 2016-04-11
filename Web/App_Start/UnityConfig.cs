using System;
using System.Configuration;
using System.Web.Mvc;
using BLL;
using BLL.Identity.Models;
using BLL.Identity.Stores;
using BLL.Services.MailingService;
using BLL.Services.MailingService.Interfaces;
using BLL.Services.PersonageService;
using IDAL.Interfaces;
using IDAL.Interfaces.Managers;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Unity.Mvc5;


namespace Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.LoadConfiguration();

            container.RegisterType<IUserStore<IdentityUser, Guid>, UserStore>(new PerHttpRequestLifetimeManager());

            container.RegisterType<RoleStore>(new PerHttpRequestLifetimeManager());
            container.RegisterType<IMailingService, MailingService>(new PerHttpRequestLifetimeManager(),
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["mailFrom"],
                    ConfigurationManager.AppSettings["mailPass"],
                    ConfigurationManager.AppSettings["mailHost"]));
            container.RegisterType<PersonManager<Employer>, EmployerManager>(new PerHttpRequestLifetimeManager(),
                new InjectionConstructor(
                    container.Resolve<IUnitOfWork>()
                    ));
            container.RegisterType<PersonManager<Admin>, AdminManager>(new PerHttpRequestLifetimeManager(),
                new InjectionConstructor(
                    container.Resolve<IUnitOfWork>()
                    ));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}