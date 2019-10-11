using Autofac;
using WebPresentationMVC.Api;
using WebPresentationMVC.Api.Endpoints.Interfaces;
using WebPresentationMVC.Api.Endpoints.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPresentationMVC.IocModules
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiHelper>().As<IApiHelper>()
                .SingleInstance(); // Singleton
            builder.RegisterType<MateriaEndpoint>().As<IMateriaEndpoint>()
                .InstancePerRequest();
            builder.RegisterType<DepartamentoEndpoint>().As<IDepartamentoEndpoint>()
                .InstancePerRequest();
            builder.RegisterType<AuthenticationEndpoint>().As<IAuthenticationEndpoint>()
                .InstancePerRequest();
            builder.RegisterType<UsuarioEndpoint>().As<IUsuarioEndpoint>()
                .InstancePerRequest();
            builder.RegisterType<HorarioConsulaEndpoint>().As<IHorarioConsultaEndpoint>()
                .InstancePerRequest();
        }
    }
}