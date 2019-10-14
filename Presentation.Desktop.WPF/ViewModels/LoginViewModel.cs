using Caliburn.Micro;
using Presentation.Desktop.WPF.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Desktop.WPF.Models;
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Models;
using System.ComponentModel;

namespace Presentation.Desktop.WPF.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName = "nico@example.com";
        // TO-DO: Hash this
        private string _password = "Example1?";
        private IAuthenticationEndpoint _authenticationEndpoint;
        private IEventAggregator _events;
        private IUsuarioLogged _usuarioLogged;

        public LoginViewModel(IAuthenticationEndpoint authenticationEndpoint, IUsuarioLogged usuarioLogged, IEventAggregator events)
        {
            _authenticationEndpoint = authenticationEndpoint;
            _usuarioLogged = usuarioLogged;
            _events = events;
        }

        public string UserName
        {
            get{ return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get { return _password;}
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        private BindingList<string> _errorMessages;

        public BindingList<string> ErrorMessages
        {
            get
            {
                return _errorMessages;
            }
            set
            {
                _errorMessages = value;
                NotifyOfPropertyChange(() => ErrorMessages);
                NotifyOfPropertyChange(() => AreErrorMessagesVisible);
            }
        }

        public bool AreErrorMessagesVisible
        {
            get
            {
                var output = false;

                if (ErrorMessages?.Count() > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task LogIn()
        {
            ErrorMessages = null;

            var loginModel = new LoginModel() { EmailAddress = UserName, Password = Password };

            try
            {
                var token = await _authenticationEndpoint.GetToken(loginModel);

                _usuarioLogged.Set(userName: token.UserName, token: token.FullToken);

                _events.PublishOnUIThread(new LogOnEvent());
            }
            catch (UnauthorizedRequestException)
            {
                ErrorMessages = new BindingList<string> { "No tiene acceso" };
            }
            catch (BadRequestException ex)
            {
                ErrorMessages = new BindingList<string>(ex.Errors.Select(kvp => string.Join(". ", kvp.Value)).ToList());
            }
            catch (Exception ex)
            {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }
    }
}
