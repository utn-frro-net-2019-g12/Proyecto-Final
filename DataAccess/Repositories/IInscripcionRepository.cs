using System.Collections.Generic;

namespace DataAccess.Repositories {
    public interface IInscripcionRepository : IRepository<Inscripcion> {
        IEnumerable<Inscripcion> GetInscripcionesOrderedByMateria();
        IEnumerable<Inscripcion> GetInscripcionesOrderedByProfesor();
        IEnumerable<Inscripcion> GetInscripcionesOrderedByAlumno();
        IEnumerable<Inscripcion> GetInscripcionesWithAlumnoAndHorario();
        IEnumerable<Inscripcion> GetInscripcionesByPartialDesc(string desc);
        IEnumerable<Inscripcion> GetInscripcionesByAlumno(int id_alumno);
        IEnumerable<Inscripcion> GetInscripcionesActivasByAlumno(int id_alumno);
        IEnumerable<Inscripcion> GetInscripcionesByProfesor(int id_profesor);
        Inscripcion GetInscripcionWithAlumnoAndHorario(int id);
    }
}
