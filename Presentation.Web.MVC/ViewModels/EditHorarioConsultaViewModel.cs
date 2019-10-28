﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Presentation.Web.MVC.Models;

namespace Presentation.Web.MVC.ViewModels {
    public class EditHorarioConsultaViewModel {
        public EditHorarioConsultaViewModel() { }

        public EditHorarioConsultaViewModel(IEnumerable<MvcUsuarioModel> profesores, IEnumerable<MvcMateriaModel> materias, List<string> dias, MvcHorarioConsultaModel horarioConsulta) {
            this.SetProfesoresAsSelectList(profesores);
            this.SetMateriasAsSelectList(materias);
            this.HorarioConsulta = horarioConsulta;
            this.SetDiasSemanaAsSelectList(dias);
        }

        public MvcHorarioConsultaModel HorarioConsulta { get; set; }
        public IEnumerable<SelectListItem> ProfesoresList { get; set; }
        public IEnumerable<SelectListItem> MateriasList { get; set; }
        public IEnumerable<SelectListItem> DiasList { get; set; }

        public void SetProfesoresAsSelectList(IEnumerable<MvcUsuarioModel> profesores) {
            ProfesoresList = profesores.Where(e => e.Matricula != null).Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Surname + " " + e.Firstname
            }) as IEnumerable<SelectListItem>;
        }

        public void SetMateriasAsSelectList(IEnumerable<MvcMateriaModel> materias) {
            MateriasList = materias.Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Name
            }) as IEnumerable<SelectListItem>;
        }

        public void SetDiasSemanaAsSelectList(List<string> dias) {
            DiasList = dias.Select(e => new SelectListItem() { Value = e, Text = e }) as IEnumerable<SelectListItem>;
        }
    }
}
