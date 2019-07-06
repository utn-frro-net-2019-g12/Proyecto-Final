﻿using DataAccessLayer.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace DataAccessLayer.Persistence
{
    public class MateriaRepository : Repository<Materia>, IMateriaRepository
    {
        public MateriaRepository(ConsultaUTNContext context) : base(context)
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
