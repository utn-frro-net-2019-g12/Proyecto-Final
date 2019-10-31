using Caliburn.Micro;
using Presentation.Desktop.WPF.EventModels;
using Presentation.Desktop.WPF.Models;
using Presentation.Desktop.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Desktop.WPF.ViewModels
{
    public class ShellViewModel : Conductor<Object>, IHandle<LogOnEvent>, IHandle<NotAuthorizedEvent>
    {
        private IEventAggregator _events;
        private MateriaViewModel _materiaVM;
        private UsuarioViewModel _usuarioVM;
        private DepartamentoViewModel _departamentoVM;
        private HorarioConsultaViewModel _horarioConsultaVM;
        private IUsuarioLogged _user;

        public ShellViewModel(IEventAggregator events, MateriaViewModel materiaVM, UsuarioViewModel usuarioVM, DepartamentoViewModel departamentoVM,
        HorarioConsultaViewModel horarioConsultaVM, IUsuarioLogged user) {
            _events = events;
            _materiaVM = materiaVM;
            _usuarioVM = usuarioVM;
            _usuarioVM = usuarioVM;
            _departamentoVM = departamentoVM;
            _horarioConsultaVM = horarioConsultaVM;
            _user = user;

            _events.Subscribe(this);

            // Activate a new instance(injected by IoC), so we do not store LoginVM
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if(string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }

                return output;
            }
        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void Exit()
        {
            TryClose();
        }

        public void Materias()
        {
            ActivateItem(_materiaVM);
        }

        public void Usuarios()
        {
            ActivateItem(_usuarioVM);
        }

        public void Departamentos() {
            ActivateItem(_departamentoVM);
        }

        public void HorariosConsulta() {
            ActivateItem(_horarioConsultaVM);
        }

        public void Handle(LogOnEvent message)
        {
            // When user is logged, this will be activated
            ActivateItem(IoC.Get<MateriaViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void Handle(NotAuthorizedEvent message)
        {
            ActivateItem(IoC.Get<NotAuthorizedViewModel>());
        }
    }
}
