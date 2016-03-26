using System;
using System.Configuration;
using System.Web.Mvc;
using BLL.Identity.Models;
using BLL.Identity.Stores;
using BLL.Services.MailingService;
using BLL.Services.MailingService.Interfaces;
using IDAL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Unity.Mvc5;
using Web.App_Start;

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

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}