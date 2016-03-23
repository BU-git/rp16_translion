using System;
using System.Reflection;
using System.Configuration; //needs for mailing service interface implementation
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using BLL.Identity.Models;
using BLL.Identity.Stores;
using DAL;
using IDAL.Interfaces;
using Microsoft.AspNet.Identity;

using BLL.Services.MailingService;
using BLL.Services.MailingService.Interfaces; 
namespace Web
{
    class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<UserStore>().As<IUserStore<IdentityUser, Guid>>().InstancePerRequest();
            builder.RegisterType<RoleStore>().As<IRoleStore<IdentityRole, Guid>>().InstancePerRequest();
            builder.RegisterType<MailingService>().As<IMailingService>().WithParameters(
                new Parameter[] {
                    new NamedParameter("from", ConfigurationManager.AppSettings["mailFrom"]),
                    new NamedParameter("password", ConfigurationManager.AppSettings["mailPass"]),
                    new NamedParameter("host", ConfigurationManager.AppSettings["mailHost"])
                })
                .InstancePerRequest(); //configuration for mailing service

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
