using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Repositories;

namespace DataAccess.Persistence {
    public class DepartamentoRepository : Repository<Departamento>, IDepartamentoRepository {
        public DepartamentoRepository(ConsultaUTNContext context) : base(context) { }

        public ConsultaUTNContext ConsultaUTNContext {
            get {
                return Context as ConsultaUTNContext;
            }
        }

        public IEnumerable<Departamento> GetDepartamentosOrderedByName() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Departamentos.OrderByDescending(e => e.Name).ToList();
        }

        public IEnumerable<Departamento> GetDepartamentosByPartialDesc(string desc) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Departamentos.Where(e => e.Name.ToLower().Contains(desc.ToLower()))
                .OrderByDescending(e => e.Name).ToList();
        }
    }
}
