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

        public MateriaViewModel(IMateriaEndpoint materiaEndpoint)
        {
            _materiaEndpoint = materiaEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadMaterias();
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

        public async Task LoadMaterias()
        {
            Materias = await _materiaEndpoint.GetAll();
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
            }
        }

        public bool CanDelete
        {
            get
            {
                bool output = false;

                if(SelectedMateria != null)
                {
                    output = true;
                }

                return output;
            }
        }

        public async void Delete()
        {
            // TO-DO catch errors thrown by Endpoints
            await _materiaEndpoint.Delete(SelectedMateria.Id);
            await LoadMaterias();
        }

        public bool CanSave
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

        public void Save()
        {

        }

    }
}
