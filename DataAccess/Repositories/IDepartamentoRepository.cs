using System.Collections.Generic;

namespace DataAccess.Repositories {
    public interface IDepartamentoRepository : IRepository<Departamento> {
        IEnumerable<Departamento> GetDepartamentosOrderedByName();
        IEnumerable<Departamento> GetDepartamentosByPartialDesc(string desc);
    }
}
