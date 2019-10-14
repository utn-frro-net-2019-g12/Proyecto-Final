using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Repositories;

namespace DataAccess.Persistence {
    public class InscripcionRepository : Repository<Inscripcion>, IInscripcionRepository {
        public InscripcionRepository(ConsultaUTNContext context) : base(context) { }

        public ConsultaUTNContext ConsultaUTNContext {
            get {
                return Context as ConsultaUTNContext;
            }
        }

        public IEnumerable<Inscripcion> GetInscripcionesOrderedByMateria() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Inscripciones
                .OrderByDescending(e => e.HorarioConsultaFechado.HorarioConsulta.Materia.Name).ToList();
        }

        public IEnumerable<Inscripcion> GetInscripcionesOrderedByProfesor() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Inscripciones
                .OrderByDescending(e => e.HorarioConsultaFechado.HorarioConsulta.Profesor.Surname)
                .ThenByDescending(e => e.HorarioConsultaFechado.HorarioConsulta.Profesor.Firstname).ToList();
        }

        public IEnumerable<Inscripcion> GetInscripcionesOrderedByAlumno() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Inscripciones.OrderByDescending(e => e.Alumno.Surname)
                .ThenByDescending(e => e.Alumno.Firstname).ToList();
        }

        public IEnumerable<Inscripcion> GetInscripcionesWithAlumnoAndHorario() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Inscripciones
                .Include(p => p.Alumno)
                .Include(p => p.HorarioConsultaFechado)
                .Include(p => p.HorarioConsultaFechado.HorarioConsulta)
                .Include(p => p.HorarioConsultaFechado.HorarioConsulta.Profesor)
                .Include(p => p.HorarioConsultaFechado.HorarioConsulta.Materia);
        }

        public Inscripcion GetInscripcionWithAlumnoAndHorario(int id) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Inscripciones.Where(p => p.Id == id)
                .Include(p => p.Alumno)
                .Include(p => p.HorarioConsultaFechado)
                .Include(p => p.HorarioConsultaFechado.HorarioConsulta)
                .Include(p => p.HorarioConsultaFechado.HorarioConsulta.Profesor)
                .Include(p => p.HorarioConsultaFechado.HorarioConsulta.Materia).FirstOrDefault();
        }
    }
}
