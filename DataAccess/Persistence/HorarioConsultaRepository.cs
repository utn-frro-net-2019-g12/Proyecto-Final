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

        public IEnumerable<HorarioConsulta> GetHorariosConsultaOrderedByMateria() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.OrderByDescending(e => e.Materia.Name).ToList();
        }

        public IEnumerable<HorarioConsulta> GetHorariosConsultaOrderedByProfesor() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.OrderByDescending(e => e.Profesor.Surname)
                .ThenByDescending(e => e.Profesor.Firstname).ToList();
        }


        public IEnumerable<HorarioConsulta> GetHorariosConsultaWithProfesorAndMateria() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Include(e => e.Profesor).Include(e => e.Materia).ToList();
        }

        public IEnumerable<HorarioConsulta> GetHorariosConsultaByPartialDesc(string desc) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Where(e => (e.Materia.Name.ToLower().Contains(desc.ToLower()) ||
                (e.Profesor.Surname.ToLower() + " " + e.Profesor.Firstname.ToLower()).Contains(desc.ToLower())))
                .Include(e => e.Profesor).Include(e => e.Materia).ToList().OrderByDescending(e => e.Materia.Name).ToList();
        }

        public IEnumerable<HorarioConsulta> GetHorariosConsultaByProfesor(int id_profesor) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Where(e => e.ProfesorId == id_profesor)
                .Include(e => e.Profesor).Include(e => e.Materia).ToList();
        }

        public IEnumerable<HorarioConsulta> GetHorariosConsultaByMateriaOrderByProfesor(int id_materia) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Where(e => e.MateriaId == id_materia).OrderBy(e => e.ProfesorId)
                .Include(e => e.Profesor).Include(e => e.Materia).ToList();
        }

        public HorarioConsulta GetHorarioConsultaWithProfesorAndMateria(int id) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Where(e => e.Id == id).Include(e => e.Profesor)
                .Include(e => e.Materia).FirstOrDefault();
        }
    }
}
