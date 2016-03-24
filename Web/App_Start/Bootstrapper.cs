using System;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using BLL.Identity.Models;
using BLL.Identity.Stores;
using BLL.Services.MailingService;
using BLL.Services.MailingService.Interfaces;
using DAL;
using IDAL.Interfaces;
using Microsoft.AspNet.Identity;
//needs for mailing service interface implementation

namespace Web
{
    internal class Bootstrapper
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
                new Parameter[]
                {
                    new NamedParameter("from", ConfigurationManager.AppSettings["mailFrom"]),
                    new NamedParameter("password", ConfigurationManager.AppSettings["mailPass"]),
                    new NamedParameter("host", ConfigurationManager.AppSettings["mailHost"])
                })
                .InstancePerRequest(); //configuration for mailing service

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}