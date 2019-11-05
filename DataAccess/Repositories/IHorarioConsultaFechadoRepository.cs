using System.Collections.Generic;

namespace DataAccess.Repositories {
    public interface IHorarioConsultaFechadoRepository : IRepository<HorarioConsultaFechado> {
        IEnumerable<HorarioConsultaFechado> GetOrderedByMateria();
        IEnumerable<HorarioConsultaFechado> GetOrderedByProfesor();
        IEnumerable<HorarioConsultaFechado> GetWithProfesorAndMateria();
        IEnumerable<HorarioConsultaFechado> GetByHorarioConsulta(int id_horario_consulta);
        IEnumerable<HorarioConsultaFechado> GetByProfesor(int id_profesor);
        IEnumerable<HorarioConsultaFechado> GetByDeptoAndMateriaAndProfe(int? deptoId, int? materiaId, int? profeId);
        HorarioConsultaFechado GetWithProfesorAndMateria(int id);
    }
}
