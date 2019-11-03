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
    public class HorarioConsultaViewModel : Screen {
        private IEventAggregator _events;
        private IHorarioConsultaEndpoint _horarioConsultaEndpoint;
        private IMateriaEndpoint _materiaEndpoint;
        private IUsuarioEndpoint _usuarioEndpoint;
        private IUsuarioLogged _usuarioLogged;
        private IMapper _mapper;

        public HorarioConsultaViewModel(IEventAggregator events, IHorarioConsultaEndpoint horarioConsultaEndpoint,
            IMateriaEndpoint materiaEndpoint, IUsuarioEndpoint usuarioEndpoint, IUsuarioLogged usuarioLogged, IMapper mapper) {
            _events = events;
            _horarioConsultaEndpoint = horarioConsultaEndpoint;
            _materiaEndpoint = materiaEndpoint;
            _usuarioEndpoint = usuarioEndpoint;
            _usuarioLogged = usuarioLogged;
            _mapper = mapper;
        }

        protected override async void OnViewLoaded(object view) {
            base.OnViewLoaded(view);
            await LoadHorariosConsulta();
            await LoadMaterias();
            await LoadUsuariosProfesores();
            LoadDiasSemana();
            ErrorMessages = null;
        }

        public async Task LoadHorariosConsulta() {
            try {
                IEnumerable<HorarioConsulta> entities = await _horarioConsultaEndpoint.GetAll(_usuarioLogged.Token);

                HorariosConsulta = _mapper.Map<BindingList<WpfHorarioConsultaModel>>(entities);
            } catch (UnauthorizedRequestException) {
                _events.PublishOnUIThread(new NotAuthorizedEvent());
            } catch (Exception ex) {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        public async Task LoadMaterias() {
            try {
                IEnumerable<Materia> entities = await _materiaEndpoint.GetAll(_usuarioLogged.Token);

                MateriasInForm = _mapper.Map<BindingList<WpfMateriaModel>>(entities);
            } catch (UnauthorizedRequestException) {
                _events.PublishOnUIThread(new NotAuthorizedEvent());
            } catch (Exception ex) {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        public async Task LoadUsuariosProfesores() {
            try {
                IEnumerable<Usuario> entities = await _usuarioEndpoint.GetAllProfesores(_usuarioLogged.Token);

                UsuariosProfesoresInForm = _mapper.Map<BindingList<WpfUsuarioModel>>(entities);
            } catch (UnauthorizedRequestException) {
                _events.PublishOnUIThread(new NotAuthorizedEvent());
            } catch (Exception ex) {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        public void LoadDiasSemana() {
            DiasSemanaInForm = new BindingList<string> { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
        }

        private BindingList<WpfHorarioConsultaModel> _horariosConsulta;

        public BindingList<WpfHorarioConsultaModel> HorariosConsulta {
            get {
                return _horariosConsulta;
            }
            set {
                _horariosConsulta = value;
                NotifyOfPropertyChange(() => HorariosConsulta);
            }
        }

        private BindingList<WpfMateriaModel> _materiasInForm;

        public BindingList<WpfMateriaModel> MateriasInForm {
            get {
                return _materiasInForm;
            }
            set {
                _materiasInForm = value;
                NotifyOfPropertyChange(() => MateriasInForm);
            }
        }


        public BindingList<WpfUsuarioModel> _usuariosProfesoresInForm;

        public BindingList<WpfUsuarioModel> UsuariosProfesoresInForm {
            get {
                return _usuariosProfesoresInForm;
            }
            set {
                _usuariosProfesoresInForm = value;
                NotifyOfPropertyChange(() => UsuariosProfesoresInForm);
            }
        }

        public BindingList<string> _diasSemanaInForm;

        public BindingList<string> DiasSemanaInForm {
            get {
                return _diasSemanaInForm;
            }
            set {
                _diasSemanaInForm = value;
                NotifyOfPropertyChange(() => DiasSemanaInForm);
            }
        }

        private WpfHorarioConsultaModel _selectedHorarioConsulta;

        public WpfHorarioConsultaModel SelectedHorarioConsulta {
            get {
                return _selectedHorarioConsulta;
            }
            set {
                _selectedHorarioConsulta = value;
                NotifyOfPropertyChange(() => SelectedHorarioConsulta);
                NotifyOfPropertyChange(() => CanDelete);
                NotifyOfPropertyChange(() => IsEditVisible);
                NotifyOfPropertyChange(() => IsCreateVisible);

                // WeekdayInForm = SelectedHorarioConsulta?.Weekday;
                StartHourInForm = SelectedHorarioConsulta?.StartHour;
                EndHourInForm = SelectedHorarioConsulta?.EndHour;
                PlaceInForm = SelectedHorarioConsulta?.Place;
                EliminationDateInForm = SelectedHorarioConsulta?.EliminationDate;
                SelectedMateria = SelectedHorarioConsulta?.Materia;
                SelectedUsuarioProfesor = SelectedHorarioConsulta?.Profesor;
                SelectedDiaSemana = SelectedHorarioConsulta?.Weekday;
                ErrorMessages = null;

                // NotifyOfPropertyChange(() => WeekdayInForm);
                NotifyOfPropertyChange(() => StartHourInForm);
                NotifyOfPropertyChange(() => EndHourInForm);
                NotifyOfPropertyChange(() => PlaceInForm);
                NotifyOfPropertyChange(() => EliminationDateInForm);
                NotifyOfPropertyChange(() => SelectedMateria);
                NotifyOfPropertyChange(() => SelectedUsuarioProfesor);
                NotifyOfPropertyChange(() => SelectedDiaSemana);
                NotifyOfPropertyChange(() => ErrorMessages);
            }
        }

        private WpfMateriaModel _selectedMateria;

        public WpfMateriaModel SelectedMateria {
            get {
                return _selectedMateria;
            }
            set {
                _selectedMateria = value;
                NotifyOfPropertyChange(() => SelectedMateria);
            }
        }

        private WpfUsuarioModel _selectedUsuarioProfesor;

        public WpfUsuarioModel SelectedUsuarioProfesor {
            get {
                return _selectedUsuarioProfesor;
            }
            set {
                _selectedUsuarioProfesor = value;
                NotifyOfPropertyChange(() => SelectedUsuarioProfesor);
            }
        }

        private string _selectedDiaSemana;

        public string SelectedDiaSemana {
            get {
                return _selectedDiaSemana;
            }
            set {
                _selectedDiaSemana = value;
                NotifyOfPropertyChange(() => SelectedDiaSemana);
            }
        }

        /* private string _weekdayInForm;

        public string WeekdayInForm {
            get {
                return _weekdayInForm;
            }
            set {
                _weekdayInForm = value;
            }
        }
        */

        private string _startHourInForm;

        public string StartHourInForm {
            get {
                return _startHourInForm;
            }
            set {
                _startHourInForm = value;
            }
        }

        private string _endHourInForm;

        public string EndHourInForm {
            get {
                return _endHourInForm;
            }
            set {
                _endHourInForm = value;
            }
        }

        private string _placeInForm;

        public string PlaceInForm {
            get {
                return _placeInForm;
            }
            set {
                _placeInForm = value;
            }
        }

        private DateTime? _eliminationDateInForm;

        public DateTime? EliminationDateInForm {
            get {
                return _eliminationDateInForm;
            }
            set {
                _eliminationDateInForm = value;
            }
        }

        public bool CanDelete {
            get {
                bool output = false;

                if (SelectedHorarioConsulta != null) {
                    output = true;
                }

                return output;
            }
        }

        public async void Delete() {
            ErrorMessages = null;

            try {
                await _horarioConsultaEndpoint.Delete(SelectedHorarioConsulta.Id, _usuarioLogged.Token);
                await LoadHorariosConsulta();
            } catch (UnauthorizedRequestException) {
                ErrorMessages = new BindingList<string> { "No tiene acceso" };
            } catch (BadRequestException ex) {
                ErrorMessages = new BindingList<string>(ex.Errors.Select(kvp => string.Join(". ", kvp.Value)).ToList());
            } catch (NotFoundRequestException ex) {
                ErrorMessages = new BindingList<string> { $"{ex.NotFoundElement}: Elemento no encontrado" };
            } catch (Exception ex) {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        public bool IsEditVisible {
            get {
                bool output = false;

                if (SelectedHorarioConsulta != null) {
                    output = true;
                }

                return output;
            }
        }

        public async void Edit() {
            ErrorMessages = null;

            var horarioConsulta = new WpfHorarioConsultaModel {
                Id = SelectedHorarioConsulta.Id, Weekday = SelectedDiaSemana, StartHour = StartHourInForm,
                EndHour = EndHourInForm, Place = PlaceInForm, EliminationDate = EliminationDateInForm,
                MateriaId = SelectedMateria?.Id, ProfesorId = SelectedUsuarioProfesor?.Id
            };

            try {
                var entity = _mapper.Map<HorarioConsulta>(horarioConsulta);

                await _horarioConsultaEndpoint.Put(entity, _usuarioLogged.Token);
                await LoadHorariosConsulta();
            } catch (UnauthorizedRequestException) {
                ErrorMessages = new BindingList<string> { "No tiene acceso" };
            } catch (BadRequestException ex) {
                ErrorMessages = new BindingList<string>(ex.Errors.Select(kvp => string.Join(". ", kvp.Value)).ToList());
            } catch (Exception ex) {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        public bool IsCreateVisible {
            get {
                bool output = false;

                if (SelectedHorarioConsulta == null) {
                    output = true;
                }

                return output;
            }
        }

        public async void Create() {
            ErrorMessages = null;

            var horarioConsulta = new WpfHorarioConsultaModel {
                Weekday = SelectedDiaSemana, StartHour = StartHourInForm, EndHour = EndHourInForm,
                Place = PlaceInForm, EliminationDate = EliminationDateInForm, MateriaId = SelectedMateria?.Id,
                ProfesorId = SelectedUsuarioProfesor?.Id
            };

            try {
                var entity = _mapper.Map<HorarioConsulta>(horarioConsulta);

                await _horarioConsultaEndpoint.Post(entity, _usuarioLogged.Token);
                await LoadHorariosConsulta();
                SelectedHorarioConsulta = null;
            } catch (UnauthorizedRequestException) {
                ErrorMessages = new BindingList<string> { "No tiene acceso" };
            } catch (BadRequestException ex) {
                ErrorMessages = new BindingList<string>(ex.Errors.Select(kvp => string.Join(". ", kvp.Value)).ToList());
            } catch (Exception ex) {
                ErrorMessages = new BindingList<string> { $"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte" };
            }
        }

        public void Add() {
            SelectedHorarioConsulta = null;
            SelectedMateria = null;
            SelectedUsuarioProfesor = null;
            SelectedDiaSemana = null;

            NotifyOfPropertyChange(() => SelectedHorarioConsulta);
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
