using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Repositories;

namespace DataAccess.Persistence {
    public class HorarioConsultaFechadoRepository : Repository<HorarioConsultaFechado>, IHorarioConsultaFechadoRepository {
        public HorarioConsultaFechadoRepository(ConsultaUTNContext context) : base(context) { }

        public ConsultaUTNContext ConsultaUTNContext {
            get {
                return Context as ConsultaUTNContext;
            }
        }

        public IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosOrderedByMateria() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.OrderByDescending(e => e.HorarioConsulta.Materia.Name).ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosOrderedByProfesor() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.OrderByDescending(e => e.HorarioConsulta.Profesor.Surname)
                .ThenByDescending(e => e.HorarioConsulta.Profesor.Firstname).ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosWithProfesorAndMateria() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.Include(e => e.HorarioConsulta).Include(e => e.HorarioConsulta.Profesor)
                .Include(e => e.HorarioConsulta.Materia).ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosByHorarioConsulta(int id_horario_consulta) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.Where(e => e.HorarioConsultaId == id_horario_consulta)
                .Include(e => e.HorarioConsulta).Include(e => e.HorarioConsulta.Profesor).Include(e => e.HorarioConsulta.Materia).OrderBy(e => e.Date)
                .ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosByProfesor(int id_profesor) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.Where(e => e.HorarioConsulta.ProfesorId == id_profesor)
                .Include(e => e.HorarioConsulta).Include(e => e.HorarioConsulta.Profesor).Include(e => e.HorarioConsulta.Materia)
                .OrderBy(e => e.HorarioConsulta.Weekday).ThenBy(e => e.HorarioConsulta.StartHour).ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosByNewSearch(string descMateria, string descProfesor) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            if (descProfesor == "") {
                return ConsultaUTNContext.HorariosConsultaFechados.Where(e => e.HorarioConsulta.Materia.Name.ToLower().Contains(descMateria.ToLower()))
                    .Include(e => e.HorarioConsulta).Include(e => e.HorarioConsulta.Profesor).Include(e => e.HorarioConsulta.Materia)
                    .Include(e => e.HorarioConsulta.Materia.Departamento).OrderByDescending(e => e.HorarioConsulta.Materia.Name)
                    .ThenBy(e => e.HorarioConsulta.Weekday).ThenBy(e => e.HorarioConsulta.StartHour).ToList();
            } else if (descMateria == "") {
                return ConsultaUTNContext.HorariosConsultaFechados.Where(e => (e.HorarioConsulta.Profesor.Surname.ToLower() + " " +
                    e.HorarioConsulta.Profesor.Firstname.ToLower()).Contains(descProfesor.ToLower()))
                    .Include(e => e.HorarioConsulta).Include(e => e.HorarioConsulta.Profesor).Include(e => e.HorarioConsulta.Materia)
                    .Include(e => e.HorarioConsulta.Materia.Departamento).OrderByDescending(e => e.HorarioConsulta.Profesor.Surname)
                    .ThenBy(e => e.HorarioConsulta.Profesor.Firstname).ThenBy(e => e.HorarioConsulta.Weekday).ThenBy(e => e.HorarioConsulta.StartHour).ToList();
            } else {
                return ConsultaUTNContext.HorariosConsultaFechados.Where(e => e.HorarioConsulta.Materia.Name.ToLower().Contains(descMateria.ToLower()))
                    .Where(e => (e.HorarioConsulta.Profesor.Surname.ToLower() + " " + e.HorarioConsulta.Profesor.Firstname.ToLower()).Contains(descProfesor.ToLower()))
                    .Include(e => e.HorarioConsulta).Include(e => e.HorarioConsulta.Profesor).Include(e => e.HorarioConsulta.Materia)
                    .Include(e => e.HorarioConsulta.Materia.Departamento).OrderByDescending(e => e.HorarioConsulta.Materia.Name)
                    .ThenBy(e => e.HorarioConsulta.Profesor.Surname).ThenBy(e => e.HorarioConsulta.Profesor.Firstname).ThenBy(e => e.HorarioConsulta.Weekday)
                    .ThenBy(e => e.HorarioConsulta.StartHour).ToList();
            }
            
        }

        public HorarioConsultaFechado GetHorarioConsultaFechadoWithProfesorAndMateria(int id) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.Where(e => e.Id == id).Include(e => e.HorarioConsulta)
                .Include(e => e.HorarioConsulta.Profesor).Include(e => e.HorarioConsulta.Materia).FirstOrDefault();
        }
    }
}
