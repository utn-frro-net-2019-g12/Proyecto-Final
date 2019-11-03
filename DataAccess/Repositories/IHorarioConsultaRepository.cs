using System.Collections.Generic;

namespace DataAccess.Repositories {
    public interface IHorarioConsultaRepository : IRepository<HorarioConsulta> {
        IEnumerable<HorarioConsulta> GetHorariosConsultaOrderedByMateria();
        IEnumerable<HorarioConsulta> GetHorariosConsultaOrderedByProfesor();
        IEnumerable<HorarioConsulta> GetHorariosConsultaWithProfesorAndMateria();
        IEnumerable<HorarioConsulta> GetHorariosConsultaByPartialDesc(string desc);
        IEnumerable<HorarioConsulta> GetHorariosConsultaByProfesor(int id_profesor);
        IEnumerable<HorarioConsulta> GetHorariosConsultaByMateriaOrderByProfesor(int id_materia);
        IEnumerable<HorarioConsulta> GetHorariosConsultaByDeptoSorted(int deptoId);
        HorarioConsulta GetHorarioConsultaWithProfesorAndMateria(int id);
    }
}
