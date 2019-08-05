using Autofac;
using AutoMapper;
using DataAccessLayer;
using WebApi.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.AutofacModules
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
                cfg.CreateMap<CreateMateriaDTO, Materia>();
            });

            return config;
        }
    }
}