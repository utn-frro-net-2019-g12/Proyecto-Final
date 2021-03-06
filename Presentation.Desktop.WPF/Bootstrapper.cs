﻿using AutoMapper;
using Caliburn.Micro;
using Presentation.Desktop.WPF.Models;
using Presentation.Desktop.WPF.ViewModels;
using Presentation.Library.Models;
using Presentation.Library.Api;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Endpoints.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Presentation.Desktop.WPF.Helpers;

namespace Presentation.Desktop.WPF
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(
                PasswordBoxHelper.BoundPasswordProperty,
                "Password",
                "PasswordChanged");
        }

        private IMapper ConfigureAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Materia, WpfMateriaModel>();
                cfg.CreateMap<WpfMateriaModel, Materia>();
                cfg.CreateMap<Departamento, WpfDepartamentoModel>();
                cfg.CreateMap<WpfDepartamentoModel, Departamento>();
                cfg.CreateMap<Usuario, WpfUsuarioModel>();
                cfg.CreateMap<WpfUsuarioModel, Usuario>();
                cfg.CreateMap<HorarioConsulta, WpfHorarioConsultaModel>();
                cfg.CreateMap<WpfHorarioConsultaModel, HorarioConsulta>();
            });

            return config.CreateMapper();
        }

        protected override void Configure()
        {
            _container.Instance(ConfigureAutomapper());

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IUsuarioLogged, UsuarioLogged>()
                .Singleton<IApiHelper, ApiHelper>();

            _container.Instance(_container)
                .PerRequest<IMateriaEndpoint, MateriaEndpoint>()
                .PerRequest<IDepartamentoEndpoint, DepartamentoEndpoint>()
                .PerRequest<IUsuarioEndpoint, UsuarioEndpoint>()
                .PerRequest<IHorarioConsultaEndpoint, HorarioConsultaEndpoint>()
                .PerRequest<IAuthenticationEndpoint, AuthenticationEndpoint>();

            // As we are using just a few ViewModels, using reflection is not that necessary
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        // Wrapper for SimpleContainer method GetInstance()
        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        // Wrapper for SimpleContainer method GetAllInstances()
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        // Wrapper for SimpleContainer method BuildUp()
        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
