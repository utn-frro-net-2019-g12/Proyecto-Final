﻿using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Web.MVC.ViewModels
{
    public class ShowHorariosParaInscribirViewModel
    {
        public ShowHorariosParaInscribirViewModel(
            IEnumerable<MvcInscripcionModel> inscripciones,
            IEnumerable<MvcDepartamentoModel> departamentos,
            IEnumerable<MvcMateriaModel> materias,
            IEnumerable<MvcUsuarioModel> profesores,
            int? departamentoId = null,
            int? materiaId = null,
            int? profesorId = null)
        {
            Inscripciones = inscripciones;

            SetDepartamentosAsSelectList(departamentos);
            SetMateriasAsSelectList(materias);
            SetProfesoresAsSelectList(profesores);
            DepartamentoId = departamentoId;
            MateriaId = materiaId;
            ProfesorId = profesorId;
        }

        public IEnumerable<MvcInscripcionModel> Inscripciones { get; set; }

        public IEnumerable<SelectListItem> DepartamentosList { get; set; }
        public int? DepartamentoId { get; set; }

        public void SetDepartamentosAsSelectList(IEnumerable<MvcDepartamentoModel> departamentos)
        {
            DepartamentosList = departamentos.Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }) as IEnumerable<SelectListItem>;
        }

        public IEnumerable<SelectListItem> MateriasList { get; set; }
        public int? MateriaId { get; set; }

        public void SetMateriasAsSelectList(IEnumerable<MvcMateriaModel> materias)
        {
            DepartamentosList = materias.Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }) as IEnumerable<SelectListItem>;
        }

        public IEnumerable<SelectListItem> ProfesoresList { get; set; }
        public int? ProfesorId { get; set; }

        public void SetProfesoresAsSelectList(IEnumerable<MvcUsuarioModel> profesores)
        {
            DepartamentosList = profesores.Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Surname + " " + e.Firstname
            }) as IEnumerable<SelectListItem>;
        }
    }
}