using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Repositories;

namespace DataAccess.Persistence {
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
            return ConsultaUTNContext.Materias.Include(e => e.Departamento).ToList();
        }

        public IEnumerable<Materia> GetMateriasByDepto(int id_depto) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Materias.Where(e => e.DepartamentoId == id_depto).OrderByDescending(e => e.Name).ToList();
        }

        public IEnumerable<Materia> GetMateriasByPartialDesc(string desc) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Materias.Where(e => e.Name.ToLower().Contains(desc.ToLower())).Include(e => e.Departamento)
                .OrderByDescending(e => e.Name).ToList();
        }

        public Materia GetMateriaWithDepto(int id) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Materias.Where(e => e.Id == id).Include(e => e.Departamento).FirstOrDefault();
        }
    }
}
