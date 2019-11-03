using AutoMapper;
using Caliburn.Micro;
using Presentation.Desktop.WPF.EventModels;
using Presentation.Desktop.WPF.Models;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Desktop.WPF.ViewModels
{
    public class UsuarioViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IUsuarioEndpoint _usuarioEndpoint;
        private readonly IUsuarioLogged _usuarioLogged;
        private readonly IMapper _mapper;

        public UsuarioViewModel(
            IEventAggregator events,
            IUsuarioEndpoint usuarioEndpoint,
            IUsuarioLogged usuarioLogged,
            IMapper mapper)
        {
            _events = events;
            _usuarioEndpoint = usuarioEndpoint;
            _usuarioLogged = usuarioLogged;
            _mapper = mapper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadUsuarios();
            ErrorMessages = null;
        }

        public async Task LoadUsuarios()
        {
            try
            {
                IEnumerable<Usuario> entities = await _usuarioEndpoint.GetAll(_usuarioLogged.Token);

                Usuarios = _mapper.Map<BindingList<WpfUsuarioModel>>(entities);
            }
            catch (UnauthorizedRequestException)
            {
                _events.PublishOnUIThread(new NotAuthorizedEvent());
            }
            catch (Exception ex)
            {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        private BindingList<WpfUsuarioModel> _usuarios;

        public BindingList<WpfUsuarioModel> Usuarios
        {
            get
            {
                return _usuarios;
            }
            set
            {
                _usuarios = value;
                NotifyOfPropertyChange(() => Usuarios);
            }
        }


        private WpfUsuarioModel _selectedUsuario;

        public WpfUsuarioModel SelectedUsuario
        {
            get
            {
                return _selectedUsuario;
            }
            set
            {
                _selectedUsuario = value;
                NotifyOfPropertyChange(() => SelectedUsuario);
                NotifyOfPropertyChange(() => CanDelete);
                NotifyOfPropertyChange(() => IsEditVisible);
                NotifyOfPropertyChange(() => IsCreateVisible);

                UsernameInForm = SelectedUsuario?.Username;
                FirstnameInForm = SelectedUsuario?.Firstname;
                SurnameInForm = SelectedUsuario?.Surname;
                EmailInForm = SelectedUsuario?.Email;
                LegajoInForm = SelectedUsuario?.Legajo;
                MatriculaInForm = SelectedUsuario?.Matricula;
                IsAdminInForm = SelectedUsuario?.IsAdmin;
                Phone1InForm = SelectedUsuario?.Phone1;
                Phone2InForm = SelectedUsuario?.Phone2;
                ErrorMessages = null;

                NotifyOfPropertyChange(() => UsernameInForm);
                NotifyOfPropertyChange(() => FirstnameInForm);
                NotifyOfPropertyChange(() => SurnameInForm);
                NotifyOfPropertyChange(() => EmailInForm);
                NotifyOfPropertyChange(() => LegajoInForm);
                NotifyOfPropertyChange(() => MatriculaInForm);
                NotifyOfPropertyChange(() => IsAdminInForm);
                NotifyOfPropertyChange(() => Phone1InForm);
                NotifyOfPropertyChange(() => Phone2InForm);
                NotifyOfPropertyChange(() => ErrorMessages);
            }
        }

        private string _usernameInForm;

        public string UsernameInForm
        {
            get
            {
                return _usernameInForm;
            }
            set
            {
                _usernameInForm = value;
            }
        }

        private string _email;

        public string EmailInForm
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        private int? _legajo;

        public int? LegajoInForm {
            get
            {
                return _legajo;
            }
            set
            {
                _legajo = value;
            }
        }

        private string _matricula;

        public string MatriculaInForm
        {
            get
            {
                return _matricula;
            }
            set
            {
                _matricula = value;
            }
        }

        private bool? _isAdmin;

        public bool? IsAdminInForm
        {
            get
            {
                return _isAdmin;
            }
            set
            {
                _isAdmin = value;
            }
        }

        private string _firstName;

        public string FirstnameInForm
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        private string _surname;

        public string SurnameInForm
        {
            get
            {
                return _surname;
            }
            set
            {
                _surname = value;
            }
        }

        private int? _phone1;

        public int? Phone1InForm
        {
            get
            {
                return _phone1;
            }
            set
            {
                _phone1 = value;
            }
        }

        private int? _phone2;

        public int? Phone2InForm
        {
            get
            {
                return _phone2;
            }
            set
            {
                _phone2 = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                bool output = false;

                if (SelectedUsuario != null)
                {
                    output = true;
                }

                return output;
            }
        }

        public async void Delete()
        {
            ErrorMessages = null;

            try
            {
                await _usuarioEndpoint.Delete(SelectedUsuario.Id, _usuarioLogged.Token);
                await LoadUsuarios();
            }
            catch (UnauthorizedRequestException)
            {
                ErrorMessages = new BindingList<string> { "No tiene acceso" };
            }
            catch (BadRequestException ex)
            {
                ErrorMessages = new BindingList<string>(ex.Errors.Select(kvp => string.Join(". ", kvp.Value)).ToList());
            }
            catch (NotFoundRequestException ex)
            {
                ErrorMessages = new BindingList<string> { $"{ex.NotFoundElement}: Elemento no encontrado" };
            }
            catch (Exception ex)
            {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        public bool IsEditVisible
        {
            get
            {
                bool output = false;

                if (SelectedUsuario != null)
                {
                    output = true;
                }

                return output;
            }
        }

        public async void Edit()
        {
            ErrorMessages = null;

            var usuario = new WpfUsuarioModel
            {
                Id = SelectedUsuario.Id,
                Firstname = FirstnameInForm,
                Surname = SurnameInForm,
                Email = EmailInForm,
                Username = UsernameInForm,
                Legajo = LegajoInForm,
                Matricula = MatriculaInForm,
                IsAdmin = IsAdminInForm,
                Phone1 = Phone1InForm,
                Phone2 = Phone2InForm
            };

            try
            {
                var entity = _mapper.Map<Usuario>(usuario);

                await _usuarioEndpoint.Put(entity, _usuarioLogged.Token);
                await LoadUsuarios();
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

        public bool IsCreateVisible
        {
            get
            {
                bool output = false;

                if (SelectedUsuario == null)
                {
                    output = true;
                }

                return output;
            }
        }

        public async void Create()
        {
            ErrorMessages = null;

            var usuario = new WpfUsuarioModel
            {
                Firstname = FirstnameInForm,
                Surname = SurnameInForm,
                Email = EmailInForm,
                Username = UsernameInForm,
                Legajo = LegajoInForm,
                Matricula = MatriculaInForm,
                IsAdmin = IsAdminInForm,
                Phone1 = Phone1InForm,
                Phone2 = Phone2InForm
            };

            try
            {
                var entity = _mapper.Map<Usuario>(usuario);

                await _usuarioEndpoint.Post(entity, _usuarioLogged.Token);
                await LoadUsuarios();
                SelectedUsuario = null;
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

        public void Add()
        {
            SelectedUsuario = null;

            NotifyOfPropertyChange(() => SelectedUsuario);
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
    }
}
