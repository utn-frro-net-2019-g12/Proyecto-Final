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

namespace Presentation.Desktop.WPF.ViewModels
{
    public class MateriaViewModel : Screen
    {
        private IEventAggregator _events;
        private IMateriaEndpoint _materiaEndpoint;
        private IDepartamentoEndpoint _departamentoEndpoint;
        private IUsuarioLogged _usuarioLogged;
        private IMapper _mapper;

        public MateriaViewModel(IEventAggregator events, IMateriaEndpoint materiaEndpoint
            , IDepartamentoEndpoint departamentoEndpoint, IUsuarioLogged usuarioLogged, IMapper mapper)
        {
            _events = events;
            _materiaEndpoint = materiaEndpoint;
            _departamentoEndpoint = departamentoEndpoint;
            _usuarioLogged = usuarioLogged;
            _mapper = mapper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadMaterias();
            await LoadDepartamentos();
        }

        public async Task LoadMaterias()
        {
            try
            {
                IEnumerable<Materia> entities = await _materiaEndpoint.GetAll(_usuarioLogged.Token);

                Materias = _mapper.Map<BindingList<WpfMateriaModel>>(entities);
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

        public async Task LoadDepartamentos()
        {
            try
            {
                IEnumerable<Departamento> entities = await _departamentoEndpoint.GetAll(_usuarioLogged.Token);

                DepartamentosInForm = _mapper.Map<BindingList<WpfDepartamentoModel>>(entities);
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

        private BindingList<WpfMateriaModel> _materias;

        public BindingList<WpfMateriaModel> Materias
        {
            get
            {
                return _materias;
            }
            set
            {
                _materias = value;
                NotifyOfPropertyChange(() => Materias);
            }
        }


        public BindingList<WpfDepartamentoModel> _departamentosInForm;

        public BindingList<WpfDepartamentoModel> DepartamentosInForm
        {
            get
            {
                return _departamentosInForm;
            }
            set
            {
                _departamentosInForm = value;
                NotifyOfPropertyChange(() => DepartamentosInForm);
            }
        }

        private WpfMateriaModel _selectedMateria;

        public WpfMateriaModel SelectedMateria
        {
            get
            {
                return _selectedMateria;
            }
            set
            {
                _selectedMateria = value;
                NotifyOfPropertyChange(() => SelectedMateria);
                NotifyOfPropertyChange(() => CanDelete);
                NotifyOfPropertyChange(() => IsEditVisible);
                NotifyOfPropertyChange(() => IsCreateVisible);

                NameInForm = SelectedMateria?.Name;
                YearInForm = SelectedMateria?.Year;
                IsElectivaInForm = SelectedMateria?.IsElectiva;
                SelectedDepartamento = SelectedMateria?.Departamento;
                ErrorMessages = null;

                NotifyOfPropertyChange(() => NameInForm);
                NotifyOfPropertyChange(() => YearInForm);
                NotifyOfPropertyChange(() => IsElectivaInForm);
                NotifyOfPropertyChange(() => SelectedDepartamento);
                NotifyOfPropertyChange(() => ErrorMessages);
            }
        }

        private WpfDepartamentoModel _selectedDepartamento;

        public WpfDepartamentoModel SelectedDepartamento
        {
            get
            {
                return _selectedDepartamento;
            }
            set
            {
                _selectedDepartamento = value;
                NotifyOfPropertyChange(() => SelectedDepartamento);
            }
        }

        private string _nameInForm;

        public string NameInForm
        {
            get
            {
                return _nameInForm;
            }
            set
            {
                _nameInForm = value;
            }
        }

        private int? _yearInForm;

        public int? YearInForm
        {
            get
            {
                return _yearInForm;
            }
            set
            {
                _yearInForm = value;
            }
        }

        private bool? _isElectivaInForm;

        public bool? IsElectivaInForm
        {
            get
            {
                return _isElectivaInForm;
            }
            set
            {
                _isElectivaInForm = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                bool output = false;

                if (SelectedMateria != null)
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
                await _materiaEndpoint.Delete(SelectedMateria.Id, _usuarioLogged.Token);
                await LoadMaterias();
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

                if (SelectedMateria != null)
                {
                    output = true;
                }

                return output;
            }
        }

        public async void Edit()
        {
            ErrorMessages = null;

            var materia = new WpfMateriaModel { Id = SelectedMateria.Id, Name = NameInForm, Year = YearInForm, IsElectiva = IsElectivaInForm,
                DepartamentoId = SelectedDepartamento.Id, Departamento = SelectedDepartamento};

            try
            {
                var entity = _mapper.Map<Materia>(materia);

                await _materiaEndpoint.Put(entity, _usuarioLogged.Token);
                await LoadMaterias();
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

                if(SelectedMateria == null)
                {
                    output = true;
                }

                return output;
            }
        }

        public async void Create()
        {
            ErrorMessages = null;

            var materia = new WpfMateriaModel { Name = NameInForm, Year = YearInForm, IsElectiva = IsElectivaInForm,
                DepartamentoId = SelectedDepartamento?.Id};

            try {
                var entity = _mapper.Map<Materia>(materia);

                await _materiaEndpoint.Post(entity, _usuarioLogged.Token);
                await LoadMaterias();
                SelectedMateria = null;
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
            SelectedMateria = null;
            SelectedDepartamento = null;

            NotifyOfPropertyChange(() => SelectedMateria);
        }

        public bool AreErrorMessagesVisible
        {
            get
            {
                var output = false;

                if(ErrorMessages?.Count() > 0)
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
