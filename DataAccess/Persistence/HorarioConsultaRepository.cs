using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Repositories;

namespace DataAccess.Persistence {
    public class HorarioConsultaRepository : Repository<HorarioConsulta>, IHorarioConsultaRepository {
        public HorarioConsultaRepository(ConsultaUTNContext context) : base(context) { }

        public ConsultaUTNContext ConsultaUTNContext {
            get {
                return Context as ConsultaUTNContext;
            }
        }

        public IEnumerable<HorarioConsulta> GetSortedByMateria() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.OrderByDescending(e => e.Materia.Name).ToList();
        }

        public IEnumerable<HorarioConsulta> GetSortedByProfesor() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.OrderByDescending(e => e.Profesor.Surname)
                .ThenByDescending(e => e.Profesor.Firstname).ToList();
        }


        public IEnumerable<HorarioConsulta> GetWithProfesorAndMateria() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Include(e => e.Profesor).Include(e => e.Materia).ToList();
        }

        public IEnumerable<HorarioConsulta> GetByPartialDescAndDeptoSorted(string desc, int? deptoId) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta
                .Where(e => desc == null || e.Materia.Name.ToLower().Contains(desc.ToLower()) || (e.Profesor.Firstname + "" + e.Profesor.Surname).ToLower().Contains(desc.ToLower()))
                .Where(e => deptoId == null || e.Materia.DepartamentoId == deptoId) // Here uses shortcircuit logic
                .Include(e => e.Profesor).Include(e => e.Materia)
                .OrderBy(e => e.Materia.Name).ThenBy(e => e.Profesor.Surname).ToList();
        }

        public IEnumerable<HorarioConsulta> GetByProfesor(int id_profesor) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Where(e => e.ProfesorId == id_profesor)
                .Include(e => e.Profesor).Include(e => e.Materia).ToList();
        }

        public IEnumerable<HorarioConsulta> GetByMateriaOrderByProfesor(int id_materia) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Where(e => e.MateriaId == id_materia).OrderBy(e => e.ProfesorId)
                .Include(e => e.Profesor).Include(e => e.Materia).ToList();
        }

        public HorarioConsulta GetWithProfesorAndMateria(int id) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Where(e => e.Id == id).Include(e => e.Profesor)
                .Include(e => e.Materia).FirstOrDefault();
        }
    }
}
