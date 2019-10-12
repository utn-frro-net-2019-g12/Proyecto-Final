using System.Collections.Generic;

namespace DataAccessLayer.Repositories {
    public interface IHorarioConsultaRepository : IRepository<HorarioConsulta> {
        IEnumerable<HorarioConsulta> GetHorariosConsultaOrderedByMateria();
        IEnumerable<HorarioConsulta> GetHorariosConsultaOrderedByProfesor();
        IEnumerable<HorarioConsulta> GetHorariosConsultaWithProfesorAndMateria();
        HorarioConsulta GetHorarioConsultaWithProfesorAndMateria(int id);
    }
}
