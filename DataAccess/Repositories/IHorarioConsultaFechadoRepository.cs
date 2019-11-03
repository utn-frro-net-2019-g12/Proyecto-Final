using System.Collections.Generic;

namespace DataAccess.Repositories {
    public interface IHorarioConsultaFechadoRepository : IRepository<HorarioConsultaFechado> {
        IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosOrderedByMateria();
        IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosOrderedByProfesor();
        IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosWithProfesorAndMateria();
        IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosByHorarioConsulta(int id_horario_consulta);
        IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosByProfesor(int id_profesor);
        IEnumerable<HorarioConsultaFechado> GetHorariosConsultaFechadosByNewSearch(string descMateria, string descProfesor);
        HorarioConsultaFechado GetHorarioConsultaFechadoWithProfesorAndMateria(int id);
    }
}
