using System.Collections.Generic;

namespace DataAccessLayer {
    public class ConsultaUTNInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ConsultaUTNContext> {
        // For Test --> System.Data.Entity.DropCreateDatabaseAlways<ConsultaUTNContext> {
        protected override void Seed(ConsultaUTNContext context) {

            var unitOfWork = new UnitOfWork(new ConsultaUTNContext());
            // WARNING: PROBLEM WITH INITIALIZATION

            var departamentos = new List<Departamento> {
                new Departamento{ Name = "Ingeniería en Sistemas de Información" },
                new Departamento{ Name = "Ingeniería Mecánica" },
                new Departamento{ Name = "Ingeniería Química" },
                new Departamento{ Name = "Ingeniería Civil" },
                new Departamento{ Name = "Ingeniería Eléctrica" },
                new Departamento{ Name = "Ciencias Básicas" },
            };

            unitOfWork.Departamentos.InsertRange(departamentos);
            unitOfWork.Complete();

            var materias = new List<Materia> {
                new Materia{ Name = "Algoritmos Genéticos", Year = 3, IsElectiva = true, DepartamentoId = departamentos[0].Id, Departamento=departamentos[0] },
                new Materia{ Name = "Diseño de Sistemas", Year = 3, IsElectiva = false, DepartamentoId = departamentos[0].Id, Departamento=departamentos[0] },
                new Materia{ Name = "Análisis Matemático 2", Year = 2, IsElectiva = false, DepartamentoId = departamentos[5].Id, Departamento=departamentos[5] },
            };

            unitOfWork.Materias.InsertRange(materias);
            unitOfWork.Complete();

            /*
            // NO ANDA :(
            var usuarios = new List<Usuario> {
                // Usuarios Test: 1 es Admin Full, 2 es Alumno-Admin, 3 es Profesor-Admin, 4 es Solo Alumno, 5 es Solo Profesor, 6 es Solo Admin
                new Usuario{ UserId = 1, Username = "AleReca", Legajo = 44176, Matricula = "JOAQ-120", IsAdmin = true, Firstname = "Alejandro Pedro", Surname = "Recalde", Email = "alereca@gmail.com", Phone = 1502030 },
                new Usuario{ UserId = 2, Username = "NicoAntonelli", Legajo = 44852, Matricula = null, IsAdmin = true, Firstname = "Nicolás Agustín", Surname = "Antonelli", Email = "niconelli2@gmail.com", Phone = 1530012 },
                new Usuario{ UserId = 3, Username = "RetroVitto", Legajo = null, Matricula = "MECA-800", IsAdmin = true, Firstname = "Vittorio", Surname = "Retrivi", Email = "retrovitto@gmail.com", Phone = 1510911 },
                new Usuario{ UserId = 4, Username = "soloAlumno", Legajo = 40123, Matricula = null, IsAdmin = false, Firstname = "Alumno", Surname = "Solo", Email = "aluonly@gmail.com", Phone = 1591111 },
                new Usuario{ UserId = 5, Username = "soloProfe", Legajo = null, Matricula = "BRES-001", IsAdmin = false, Firstname = "Profesor", Surname = "Solo", Email = "profonly@gmail.com", Phone = 1592222 },
                new Usuario{ UserId = 6, Username = "soloAdmin", Legajo = null, Matricula = null, IsAdmin = true, Firstname = "Admin", Surname = "Solo", Email = "adminonly@gmail.com", Phone = 1593333 },
            };

            // For the Users --> Remember Fix the username (Make a FK from IdentityFramework), and add "Photo" (Consult about the image format file... Blob?)

            unitOfWork.Usuarios.InsertRange(usuarios);
            unitOfWork.Complete();
            */

        }
    }
}
