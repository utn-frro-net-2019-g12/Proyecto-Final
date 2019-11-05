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

        public IEnumerable<HorarioConsultaFechado> GetOrderedByMateria() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.OrderByDescending(e => e.HorarioConsulta.Materia.Name).ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetOrderedByProfesor() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.OrderByDescending(e => e.HorarioConsulta.Profesor.Surname)
                .ThenByDescending(e => e.HorarioConsulta.Profesor.Firstname).ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetWithProfesorAndMateria() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.Include(e => e.HorarioConsulta).Include(e => e.HorarioConsulta.Profesor)
                .Include(e => e.HorarioConsulta.Materia).ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetByHorarioConsulta(int id_horario_consulta) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.Where(e => e.HorarioConsultaId == id_horario_consulta)
                .Include(e => e.HorarioConsulta).Include(e => e.HorarioConsulta.Profesor).Include(e => e.HorarioConsulta.Materia).OrderBy(e => e.Date)
                .ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetByProfesor(int id_profesor) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.Where(e => e.HorarioConsulta.ProfesorId == id_profesor)
                .Include(e => e.HorarioConsulta).Include(e => e.HorarioConsulta.Profesor).Include(e => e.HorarioConsulta.Materia)
                .OrderBy(e => e.HorarioConsulta.Weekday).ThenBy(e => e.HorarioConsulta.StartHour).ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetByDeptoAndMateriaAndProfe(int? deptoId, int? materiaId, int? profeId) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados
                .Where(e => deptoId == null || e.HorarioConsulta.Materia.DepartamentoId == deptoId)
                .Where(e => materiaId == null || e.HorarioConsulta.MateriaId == materiaId)
                .Where(e => profeId == null || e.HorarioConsulta.ProfesorId == profeId)
                .Include(e => e.HorarioConsulta).Include(e => e.HorarioConsulta.Profesor).Include(e => e.HorarioConsulta.Materia)
                .ToList();
        }

        public HorarioConsultaFechado GetWithProfesorAndMateria(int id) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.Where(e => e.Id == id).Include(e => e.HorarioConsulta)
                .Include(e => e.HorarioConsulta.Profesor).Include(e => e.HorarioConsulta.Materia).FirstOrDefault();
        }
    }
}
