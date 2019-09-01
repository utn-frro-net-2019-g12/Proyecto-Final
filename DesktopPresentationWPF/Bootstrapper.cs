using Caliburn.Micro;
using DesktopPresentationWPF.Api;
using DesktopPresentationWPF.Models;
using DesktopPresentationWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TRMDesktopUI.Helpers;

namespace DesktopPresentationWPF
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

        protected override void Configure()
        {
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IUsuarioLoggedModel, UsuarioLoggedInModel>()
                .Singleton<IApiHelper, ApiHelper>();

            _container.Instance(_container)
                .PerRequest<IMateriaEndpoint, MateriaEndpoint>()
                .PerRequest<IDepartamentoEndpoint, DepartamentoEndpoint>();

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
