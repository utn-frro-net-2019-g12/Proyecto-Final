using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.Repositories;

namespace DataAccessLayer.Persistence {
    public class HorarioConsultaRepository : Repository<HorarioConsulta>, IHorarioConsultaRepository {
        public HorarioConsultaRepository(ConsultaUTNContext context) : base(context) { }

        public ConsultaUTNContext ConsultaUTNContext {
            get {
                return Context as ConsultaUTNContext;
            }
        }

        public IEnumerable<HorarioConsulta> GetHorariosConsultasOrderedByMateria()
        {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.OrderByDescending(e => e.Materia.Name).ToList();
        }

        public IEnumerable<HorarioConsulta> GetHorariosConsultasOrderedByProfesor()
        {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.OrderByDescending(e => e.Profesor.Surname)
                .ThenByDescending(e => e.Profesor.Firstname).ToList();
        }

        public IEnumerable<HorarioConsulta> GetHorariosConsultaWithProfesorAndMateria()
        {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Include(p => p.Profesor).Include(p => p.Materia);
        }

        public HorarioConsulta GetHorarioConsultaWithProfesorAndMateria(int id)
        {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsulta.Where(p => p.Id == id).Include(p => p.Profesor)
                .Include(p => p.Materia).FirstOrDefault();
        }
    }
}
