using DataAccessLayer.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace DataAccessLayer.Persistence
{
    public class MateriasRepository : Repository<Materia>, IMateriaRepository
    {
        public MateriasRepository(ConsultaUTNContext context) : base(context)
        {
        }

        public ConsultaUTNContext ConsultaUTNContext
        {
            get
            {
                return Context as ConsultaUTNContext;
            }
        }

        public IEnumerable<Materia> GetMateriasOrderedByName()
        {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);

            return ConsultaUTNContext.Materias.OrderByDescending(e => e.Name).ToList();
        }
    }
}
