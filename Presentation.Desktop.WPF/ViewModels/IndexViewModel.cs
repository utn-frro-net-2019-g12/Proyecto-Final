using Caliburn.Micro;
using Presentation.Desktop.WPF.Models;
using Presentation.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Exceptions;
using AutoMapper;
using Presentation.Desktop.WPF.EventModels;

namespace Presentation.Desktop.WPF.ViewModels {
    public class IndexViewModel : Screen {
        private IEventAggregator _events;
        private IUsuarioLogged _usuarioLogged;
        private IMapper _mapper;

        public IndexViewModel(IEventAggregator events, IUsuarioLogged usuarioLogged, IMapper mapper) {
            _events = events;
            _usuarioLogged = usuarioLogged;
            _mapper = mapper;
        }

        protected override void OnViewLoaded(object view) {
            base.OnViewLoaded(view);
        }

        public bool AreErrorMessagesVisible {
            get {
                var output = false;

                if (ErrorMessages?.Count() > 0) {
                    output = true;
                }

                return output;
            }
        }

        private BindingList<string> _errorMessages;

        public BindingList<string> ErrorMessages {
            get {
                return _errorMessages;
            }
            set {
                _errorMessages = value;
                NotifyOfPropertyChange(() => ErrorMessages);
                NotifyOfPropertyChange(() => AreErrorMessagesVisible);
            }
        }
    }
}
