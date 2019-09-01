using Caliburn.Micro;
using DesktopPresentationWPF.Api;
using DesktopPresentationWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPresentationWPF.ViewModels
{
    public class MateriaViewModel : Screen
    {
        private IMateriaEndpoint _materiaEndpoint;
        private IDepartamentoEndpoint _departamentoEndpoint;

        public MateriaViewModel(IMateriaEndpoint materiaEndpoint, IDepartamentoEndpoint departamentoEndpoint)
        {
            _materiaEndpoint = materiaEndpoint;
            _departamentoEndpoint = departamentoEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadMaterias();
            await LoadDepartamentos(); 
        }

        public async Task LoadMaterias()
        {
            Materias = await _materiaEndpoint.GetAll();
        }

        public async Task LoadDepartamentos()
        {
            DepartamentosInForm = await _departamentoEndpoint.GetAll();
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

                NotifyOfPropertyChange(() => NameInForm);
                NotifyOfPropertyChange(() => YearInForm);
                NotifyOfPropertyChange(() => IsElectivaInForm);
                NotifyOfPropertyChange(() => SelectedDepartamento);
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
            ErrorMessage = "";

            try
            {
                await _materiaEndpoint.Delete(SelectedMateria.Id);
                await LoadMaterias();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
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
            ErrorMessage = "";

            var materia = new WpfMateriaModel { Id = SelectedMateria.Id, Name = NameInForm, Year = YearInForm, IsElectiva = IsElectivaInForm,
                DepartamentoId = SelectedDepartamento.Id, Departamento = SelectedDepartamento};

            try
            {
                await _materiaEndpoint.Put(materia);
                await LoadMaterias();
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
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
            ErrorMessage = "";

            var materia = new WpfMateriaModel { Name = NameInForm, Year = YearInForm, IsElectiva = IsElectivaInForm,
                DepartamentoId = SelectedDepartamento?.Id};

            try { 
                await _materiaEndpoint.Post(materia);
                await LoadMaterias();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void Add()
        {
            SelectedMateria = null;
            SelectedDepartamento = null;

            NotifyOfPropertyChange(() => SelectedMateria);
        }

        public bool IsErrorMessageVisible
        {
            get
            {
                var output = false;

                if(ErrorMessage?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorMessageVisible);
            }
        }
    }
}
