using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Repositories;

namespace DataAccess.Persistence {
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository {
        public UsuarioRepository(ConsultaUTNContext context) : base(context) { }

        public ConsultaUTNContext ConsultaUTNContext {
            get {
                return Context as ConsultaUTNContext;
            }
        }

        public bool? IsAdmin(int userId) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => e.Id == userId).Select(e => e.IsAdmin).SingleOrDefault();
        }

        // Null --> No es Alumno
        public int? GetLegajo(int userId) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => e.Id == userId).Select(e => e.Legajo != null ? e.Legajo : null).SingleOrDefault();
        }

        // Null --> No es Profesor
        public string GetMatricula(int userId) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => e.Id == userId).Select(e => e.Matricula != null ? e.Matricula : null).SingleOrDefault();
        }

        public IEnumerable<Usuario> GetUsuariosOrderedByUsername() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.OrderByDescending(e => e.Username).ToList();
        }

        // By Surname, then Firstname
        public IEnumerable<Usuario> GetUsuariosOrderedByFullname() { 
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.OrderByDescending(e => e.Surname).ThenByDescending(e => e.Firstname).ToList();
        }

        public IEnumerable<Usuario> GetUsuariosProfesoresOrderedByFullName() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => e.Matricula != null)
                .OrderByDescending(e => e.Surname).ThenByDescending(e => e.Firstname).ToList();
        }

        public IEnumerable<Usuario> GetUsuariosAlumnosOrderedByLegajo() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => e.Legajo != null).OrderByDescending(e => e.Legajo).ToList();
        }

        public IEnumerable<Usuario> GetUsuariosByPartialDesc(string desc) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => (e.Surname.ToLower() + " " + e.Firstname.ToLower()).Contains(desc.ToLower()))
                .OrderByDescending(e => e.Surname).ThenByDescending(e => e.Firstname).ToList();
        }

        public IEnumerable<Usuario> GetUsuariosAlumnosByPartialDesc(string desc) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => (e.Legajo != null) &&
                ((e.Surname.ToLower() + " " + e.Firstname.ToLower()).Contains(desc.ToLower())))
                .OrderByDescending(e => e.Surname).ThenByDescending(e => e.Firstname).ToList();
        }

        public IEnumerable<Usuario> GetUsuariosProfesoresByPartialDesc(string desc) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => (e.Matricula != null) &&
                ((e.Surname.ToLower() + " " + e.Firstname.ToLower()).Contains(desc.ToLower())))
                .OrderByDescending(e => e.Surname).ThenByDescending(e => e.Firstname).ToList();
        }

        public Usuario GetUsuarioById(int userId) { 
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => e.Id == userId).FirstOrDefault();
        }

        public Usuario GetUsuarioByUsername(string username) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => e.Username == username).FirstOrDefault();
        }

        public Usuario GetUsuarioByFullname(string surname, string firstname) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => e.Surname == surname & e.Firstname == firstname).FirstOrDefault();
        }

        public Usuario GetUsuarioAlumnoByLegajo(int legajo) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => e.Legajo == legajo).FirstOrDefault();
        }

        public Usuario GetUsuarioProfesorByMatricula(string matricula) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(e => e.Matricula == matricula).FirstOrDefault();
        }
    }
}
