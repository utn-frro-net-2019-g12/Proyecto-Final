﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using Presentation.Web.MVC.IocModules;
using Presentation.Web.MVC.Models;

namespace Presentation.Web.MVC.App_Start
{
    public class IocConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                .InstancePerRequest();

            builder.RegisterAssemblyModules(typeof(MvcApplication).Assembly);

            builder.RegisterType<UserSession>().As<IUserSession>()
                .InstancePerRequest();

            builder.RegisterModule<ApiModule>();
            builder.RegisterModule<AutoMapperModule>();

            // May be unnecessary
            /*builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider*/

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}