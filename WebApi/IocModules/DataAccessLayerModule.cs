using Autofac;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.IocModules
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<ConsultaUTNContext>().AsSelf();
        }
    }
}