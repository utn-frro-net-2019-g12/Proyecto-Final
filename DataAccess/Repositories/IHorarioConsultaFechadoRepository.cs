using System.Collections.Generic;

namespace DataAccess.Repositories {
    public interface IHorarioConsultaFechadoRepository : IRepository<HorarioConsultaFechado> {
        IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosOrderedByMateria();
        IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosOrderedByProfesor();
        IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosWithProfesorAndMateria();
        HorarioConsultaFechado GetHorarioConsultaFechadoWithProfesorAndMateria(int id);
    }
}
