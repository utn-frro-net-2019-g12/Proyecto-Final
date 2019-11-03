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

namespace Presentation.Desktop.WPF.ViewModels {

    public class DepartamentoViewModel : Screen {
        private IEventAggregator _events;
        private IDepartamentoEndpoint _departamentoEndpoint;
        private IUsuarioLogged _usuarioLogged;
        private IMapper _mapper;

        public DepartamentoViewModel(
            IEventAggregator events,
            IDepartamentoEndpoint departamentoEndpoint,
            IUsuarioLogged usuarioLogged,
            IMapper mapper) 
        {
            _events = events;
            _departamentoEndpoint = departamentoEndpoint;
            _usuarioLogged = usuarioLogged;
            _mapper = mapper;
        }

        protected override async void OnViewLoaded(object view) {
            base.OnViewLoaded(view);
            await LoadDepartamentos();
            ErrorMessages = null;
        }

        public async Task LoadDepartamentos() {
            try {
                IEnumerable<Departamento> entities = await _departamentoEndpoint.GetAll(_usuarioLogged.Token);

                Departamentos = _mapper.Map<BindingList<WpfDepartamentoModel>>(entities);
            }
            catch (UnauthorizedRequestException) {
                _events.PublishOnUIThread(new NotAuthorizedEvent());
            }
            catch (Exception ex) {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        private BindingList<WpfDepartamentoModel> _departamentos;

        public BindingList<WpfDepartamentoModel> Departamentos {
            get {
                return _departamentos;
            }
            set {
                _departamentos = value;
                NotifyOfPropertyChange(() => Departamentos );
            }
        }


        private WpfDepartamentoModel _selectedDepartamento;

        public WpfDepartamentoModel SelectedDepartamento {
            get {
                return _selectedDepartamento;
            }
            set {
                _selectedDepartamento = value;
                NotifyOfPropertyChange(() => SelectedDepartamento);
                NotifyOfPropertyChange(() => CanDelete);
                NotifyOfPropertyChange(() => IsEditVisible);
                NotifyOfPropertyChange(() => IsCreateVisible);

                NameInForm = SelectedDepartamento?.Name;
                ErrorMessages = null;

                NotifyOfPropertyChange(() => NameInForm);
                NotifyOfPropertyChange(() => ErrorMessages);
            }
        }

        private string _nameInForm;

        public string NameInForm {
            get {
                return _nameInForm;
            }
            set {
                _nameInForm = value;
            }
        }

        public bool CanDelete {
            get {
                bool output = false;

                if (SelectedDepartamento != null) {
                    output = true;
                }

                return output;
            }
        }

        public async void Delete() {
            ErrorMessages = null;

            try {
                await _departamentoEndpoint.Delete(SelectedDepartamento.Id, _usuarioLogged.Token);
                await LoadDepartamentos();
            }
            catch (UnauthorizedRequestException) {
                ErrorMessages = new BindingList<string> { "No tiene acceso" };
            }
            catch (BadRequestException ex) {
                ErrorMessages = new BindingList<string>(ex.Errors.Select(kvp => string.Join(". ", kvp.Value)).ToList());
            }
            catch (NotFoundRequestException ex) {
                ErrorMessages = new BindingList<string> { $"{ex.NotFoundElement}: Elemento no encontrado" };
            }
            catch (Exception ex) {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        public bool IsEditVisible {
            get {
                bool output = false;

                if (SelectedDepartamento != null) {
                    output = true;
                }

                return output;
            }
        }

        public async void Edit() {
            ErrorMessages = null;

            var materia = new WpfDepartamentoModel {
                Id = SelectedDepartamento.Id,
                Name = NameInForm
            };

            try {
                var entity = _mapper.Map<Departamento>(materia);

                await _departamentoEndpoint.Put(entity, _usuarioLogged.Token);
                await LoadDepartamentos();
            }
            catch (UnauthorizedRequestException) {
                ErrorMessages = new BindingList<string> { "No tiene acceso" };
            }
            catch (BadRequestException ex) {
                ErrorMessages = new BindingList<string>(ex.Errors.Select(kvp => string.Join(". ", kvp.Value)).ToList());
            }
            catch (Exception ex) {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        public bool IsCreateVisible {
            get {
                bool output = false;

                if (SelectedDepartamento == null) {
                    output = true;
                }

                return output;
            }
        }

        public async void Create() {
            ErrorMessages = null;

            var departamento = new WpfDepartamentoModel {
                Name = NameInForm
            };

            try {
                var entity = _mapper.Map<Departamento>(departamento);

                await _departamentoEndpoint.Post(entity, _usuarioLogged.Token);
                await LoadDepartamentos();
                SelectedDepartamento = null;
            }
            catch (UnauthorizedRequestException) {
                ErrorMessages = new BindingList<string> { "No tiene acceso" };
            }
            catch (BadRequestException ex) {
                ErrorMessages = new BindingList<string>(ex.Errors.Select(kvp => string.Join(". ", kvp.Value)).ToList());
            }
            catch (Exception ex) {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        public void Add() {
            SelectedDepartamento = null;

            NotifyOfPropertyChange(() => SelectedDepartamento);
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
