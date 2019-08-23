using System.Collections.Generic;

namespace DataAccessLayer.Repositories {
    public interface IHorarioConsultaRepository : IRepository<HorarioConsulta> {
        IEnumerable<HorarioConsulta> GetHorariosConsultasOrderedByMateria();
        IEnumerable<HorarioConsulta> GetHorariosConsultasOrderedByProfesor();
        IEnumerable<HorarioConsulta> GetHorariosConsultaWithProfesorAndMateria();
        HorarioConsulta GetHorarioConsultaWithProfesorAndMateria(int id);
    }
}
