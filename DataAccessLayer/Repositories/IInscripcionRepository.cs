using System.Collections.Generic;

namespace DataAccessLayer.Repositories {
    public interface IInscripcionRepository : IRepository<Inscripcion> {
        IEnumerable<Inscripcion> GetInscripcionesOrderedByMateria();
        IEnumerable<Inscripcion> GetInscripcionesOrderedByProfesor();
        IEnumerable<Inscripcion> GetInscripcionesOrderedByAlumno();
        IEnumerable<Inscripcion> GetInscripcionesWithAlumnoAndHorario();
        Inscripcion GetInscripcionWithAlumnoAndHorario(int id);
    }
}
