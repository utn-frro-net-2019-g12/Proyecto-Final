using System.Collections.Generic;

namespace DataAccess.Repositories {
    public interface IMateriaRepository : IRepository<Materia> {
        IEnumerable<Materia> GetMateriasOrderedByName();
        IEnumerable<Materia> GetMateriasWithDepto();
        IEnumerable<Materia> GetMateriasByDepto(int id_depto);
        IEnumerable<Materia> GetMateriasByPartialDesc(string desc);
        Materia GetMateriaWithDepto(int id);
    }
}
