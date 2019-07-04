using System.Collections.Generic;

namespace DataAccessLayer.Repositories
{
    public interface IMateriaRepository : IRepository<Materia>
    {
        IEnumerable<Materia> GetMateriasOrderedByName();
    }
}
