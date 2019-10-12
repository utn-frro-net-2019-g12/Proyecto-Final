using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.Repositories;

namespace DataAccessLayer.Persistence {
    public class HorarioConsultaFechadoRepository : Repository<HorarioConsultaFechado>, IHorarioConsultaFechadoRepository {
        public HorarioConsultaFechadoRepository(ConsultaUTNContext context) : base(context) { }

        public ConsultaUTNContext ConsultaUTNContext {
            get {
                return Context as ConsultaUTNContext;
            }
        }

        public IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosOrderedByMateria()
        {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.OrderByDescending(e => e.HorarioConsulta.Materia.Name).ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosOrderedByProfesor()
        {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.OrderByDescending(e => e.HorarioConsulta.Profesor.Surname)
                .ThenByDescending(e => e.HorarioConsulta.Profesor.Firstname).ToList();
        }

        public IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosWithProfesorAndMateria()
        {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.Include(p => p.HorarioConsulta.Profesor)
                .Include(p => p.HorarioConsulta.Materia);
        }

        public HorarioConsultaFechado GetHorarioConsultaFechadoWithProfesorAndMateria(int id)
        {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.HorariosConsultaFechados.Where(p => p.Id == id)
                .Include(p => p.HorarioConsulta.Profesor).Include(p => p.HorarioConsulta.Materia).FirstOrDefault();
        }
    }
}
