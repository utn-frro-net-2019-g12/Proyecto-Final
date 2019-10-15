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
                .Include(e => e.Alumno)
                .Include(e => e.HorarioConsultaFechado)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta.Profesor)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta.Materia).ToList();
        }

        public IEnumerable<Inscripcion> GetInscripcionesByAlumno(int id_alumno) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Inscripciones.Where(e => e.AlumnoId == id_alumno).OrderByDescending(e => e.Alumno.Legajo)
                .Include(e => e.Alumno)
                .Include(e => e.HorarioConsultaFechado)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta.Profesor)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta.Materia).ToList();
        }

        public IEnumerable<Inscripcion> GetInscripcionesActivasByAlumno(int id_alumno) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Inscripciones.Where(e => (e.AlumnoId == id_alumno) && (e.State != false)).OrderByDescending(e => e.Alumno.Legajo)
                .Include(e => e.Alumno)
                .Include(e => e.HorarioConsultaFechado)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta.Profesor)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta.Materia).ToList();
        }

        public IEnumerable<Inscripcion> GetInscripcionesByProfesor(int id_profesor) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Inscripciones.Where(e => e.HorarioConsultaFechado.HorarioConsulta.ProfesorId == id_profesor)
                .OrderByDescending(e => e.HorarioConsultaFechado.HorarioConsulta.Profesor.Surname)
                .ThenByDescending(e => e.HorarioConsultaFechado.HorarioConsulta.Profesor.Firstname)
                .Include(e => e.Alumno)
                .Include(e => e.HorarioConsultaFechado)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta.Profesor)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta.Materia).ToList();
        }

        public Inscripcion GetInscripcionWithAlumnoAndHorario(int id) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Inscripciones.Where(e => e.Id == id)
                .Include(e => e.Alumno)
                .Include(e => e.HorarioConsultaFechado)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta.Profesor)
                .Include(e => e.HorarioConsultaFechado.HorarioConsulta.Materia).FirstOrDefault();
        }
    }
}
