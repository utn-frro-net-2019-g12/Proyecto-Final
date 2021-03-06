﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataAccess {
    public class ConsultaUTNContext : DbContext {
        public virtual DbSet<Materia> Materias { get; set; }
        public virtual DbSet<Departamento> Departamentos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<HorarioConsulta> HorariosConsulta { get; set; }
        public virtual DbSet<HorarioConsultaFechado> HorariosConsultaFechados { get; set; }
        public virtual DbSet<Inscripcion> Inscripciones { get; set; }

        public ConsultaUTNContext() : base("name=ConsultaUTNContext") {
            Database.SetInitializer<ConsultaUTNContext>(new ConsultaUTNInitializer());
            Database.Initialize(false);

            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            // Prevent Circular Cascade Deletion
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}
