using System.Collections.Generic;

namespace DataAccess.Repositories {
    public interface IUsuarioRepository : IRepository<Usuario> {
        bool? IsAdmin(int userId);
        int? GetLegajo(int userId); // Null --> No es Alumno
        string GetMatricula(int userId); // Null --> No es Profesor

        IEnumerable<Usuario> GetUsuariosOrderedByUsername();
        IEnumerable<Usuario> GetUsuariosOrderedByFullname(); // By Surname, then Firstname
        IEnumerable<Usuario> GetUsuariosProfesoresOrderedByFullName();
        IEnumerable<Usuario> GetUsuariosAlumnosOrderedByLegajo();
        IEnumerable<Usuario> GetUsuariosByPartialDesc(string desc);
        IEnumerable<Usuario> GetUsuariosAlumnosByPartialDesc(string desc);
        IEnumerable<Usuario> GetUsuariosProfesoresByPartialDesc(string desc);

        Usuario GetUsuarioById(int userId);
        Usuario GetUsuarioByUsername(string username);
        Usuario GetUsuarioByFullname(string surname, string firstname);
        Usuario GetUsuarioAlumnoByLegajo(int legajo);
        Usuario GetUsuarioProfesorByMatricula(string matricula);
    }
}
