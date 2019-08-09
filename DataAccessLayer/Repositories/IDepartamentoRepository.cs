using System.Collections.Generic;

namespace DataAccessLayer.Repositories {
    public interface IDepartamentoRepository : IRepository<Departamento> {
        IEnumerable<Departamento> GetDepartamentosOrderedByName();
    }
}
