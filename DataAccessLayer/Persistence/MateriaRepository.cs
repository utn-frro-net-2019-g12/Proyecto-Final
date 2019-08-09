using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.Repositories;

namespace DataAccessLayer.Persistence {
    public class MateriaRepository : Repository<Materia>, IMateriaRepository {
        public MateriaRepository(ConsultaUTNContext context) : base(context) { }

        public ConsultaUTNContext ConsultaUTNContext {
            get {
                return Context as ConsultaUTNContext;
            }
        }

        public IEnumerable<Materia> GetMateriasOrderedByName() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Materias.OrderByDescending(e => e.Name).ToList();
        }

        public IEnumerable<Materia> GetMateriasWithDepto() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Materias.Include(p => p.Departamento);
        }

        public Materia GetMateriaWithDepto(int id) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Materias.Where(p => p.Id == id).Include(p => p.Departamento).FirstOrDefault();
        }
    }
}
