using System.Collections.Generic;

namespace DataAccess.Repositories {
    public interface IHorarioConsultaRepository : IRepository<HorarioConsulta> {
        IEnumerable<HorarioConsulta> GetSortedByMateria();
        IEnumerable<HorarioConsulta> GetSortedByProfesor();
        IEnumerable<HorarioConsulta> GetWithProfesorAndMateria();
        IEnumerable<HorarioConsulta> GetByPartialDescAndDeptoSorted(string desc, int? deptoId);
        IEnumerable<HorarioConsulta> GetByProfesor(int id_profesor);
        IEnumerable<HorarioConsulta> GetByMateriaOrderByProfesor(int id_materia);
        HorarioConsulta GetWithProfesorAndMateria(int id);
    }
}
