using Caliburn.Micro;
using DesktopPresentationWPF.EventModels;
using DesktopPresentationWPF.Models;
using DesktopPresentationWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPresentationWPF.ViewModels
{
    public class ShellViewModel : Conductor<Object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private MateriaViewModel _materiaVM;
        private UsuarioViewModel _usuarioVM;
        private IUsuarioLogged _user;

        public ShellViewModel(IEventAggregator events, MateriaViewModel materiaVM, UsuarioViewModel usuarioVM,
            IUsuarioLogged user) {
            _events = events;
            _materiaVM = materiaVM;
            _usuarioVM = usuarioVM;
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

        public void Handle(LogOnEvent message)
        {
            // When user is logged, this will be activated
            ActivateItem(_materiaVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
