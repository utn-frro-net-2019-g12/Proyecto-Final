﻿using Autofac;
using AutoMapper;
using Presentation.Library.Models;
using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Web.MVC.IocModules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var mapperConfiguration = CreateConfiguration();

            builder.RegisterInstance<IMapper>(mapperConfiguration.CreateMapper());
        }

        private MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Materia, MvcMateriaModel>();
                cfg.CreateMap<MvcMateriaModel, Materia>();
                cfg.CreateMap<Usuario, MvcUsuarioModel>();
                cfg.CreateMap<MvcUsuarioModel, Usuario>();
                cfg.CreateMap<Departamento, MvcDepartamentoModel>();
                cfg.CreateMap<MvcDepartamentoModel, Departamento>();
                cfg.CreateMap<HorarioConsulta, MvcHorarioConsultaModel>();
                cfg.CreateMap<MvcHorarioConsultaModel, HorarioConsulta>();
                cfg.CreateMap<HorarioConsultaFechado, MvcHorarioConsultaFechadoModel>();
                cfg.CreateMap<MvcHorarioConsultaFechadoModel, HorarioConsultaFechado>();
                cfg.CreateMap<Inscripcion, MvcInscripcionModel>();
                cfg.CreateMap<MvcInscripcionModel, Inscripcion>();
            });

            return config;
        }
    }
}