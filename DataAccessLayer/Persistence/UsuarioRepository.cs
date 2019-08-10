using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.Repositories;

namespace DataAccessLayer.Persistence {
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository {
        public UsuarioRepository(ConsultaUTNContext context) : base(context) { }

        public ConsultaUTNContext ConsultaUTNContext {
            get {
                return Context as ConsultaUTNContext;
            }
        }

        public bool? IsAdmin(int userId) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(p => p.Id == userId).Select(p => p.IsAdmin).SingleOrDefault();
        }

        // Null --> No es Alumno
        public int? GetLegajo(int userId) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(p => p.Id == userId).Select(p => p.Legajo != null ? p.Legajo : null).SingleOrDefault();
        }

        // Null --> No es Profesor
        public string GetMatricula(int userId) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(p => p.Id == userId).Select(p => p.Matricula != null ? p.Matricula : null).SingleOrDefault();
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

        public IEnumerable<Usuario> GetUsuariosAlumnosOrderedByLegajo() {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(p => p.Legajo != null).OrderByDescending(e => e.Legajo).ToList();
        }


        public Usuario GetUsuarioById(int userId) { 
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(p => p.Id == userId).FirstOrDefault();
        }

        public Usuario GetUsuarioByUsername(string username) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(p => p.Username == username).FirstOrDefault();
        }

        public Usuario GetUsuarioByFullname(string surname, string firstname) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(p => p.Surname == surname & p.Firstname == firstname).FirstOrDefault();
        }

        public Usuario GetUsuarioAlumnoByLegajo(int legajo) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(p => p.Legajo == legajo).FirstOrDefault();
        }

        public Usuario GetUsuarioProfesorByMatricula(string matricula) {
            ConsultaUTNContext.Database.Log = message => Trace.Write(message);
            return ConsultaUTNContext.Usuarios.Where(p => p.Matricula == matricula).FirstOrDefault();
        }

    }
}
