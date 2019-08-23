using System.Collections.Generic;

namespace DataAccessLayer {
    public class ConsultaUTNInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ConsultaUTNContext> {
        // For Test --> System.Data.Entity.DropCreateDatabaseAlways<ConsultaUTNContext> {
        protected override void Seed(ConsultaUTNContext context) {

            var unitOfWork = new UnitOfWork(new ConsultaUTNContext());

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
                new Materia{ Name = "Algoritmos Genéticos", Year = 3, IsElectiva = true, DepartamentoId = departamentos[0].Id, Departamento = departamentos[0] },
                new Materia{ Name = "Diseño de Sistemas", Year = 3, IsElectiva = false, DepartamentoId = departamentos[0].Id, Departamento = departamentos[0] },
                new Materia{ Name = "Análisis Matemático 2", Year = 2, IsElectiva = false, DepartamentoId = departamentos[5].Id, Departamento = departamentos[5] },
            };

            unitOfWork.Materias.InsertRange(materias);
            unitOfWork.Complete();

            var usuarios = new List<Usuario> {
                // Users Test: 1 Admin Full, 2 Alumno-Admin, 3 Profesor-Admin, 4 Alumno-Profesor, 5 Alumno Only, 6 Profesor Only, 7 Admin Only
                new Usuario{ Id = 1, Username = "ale@example.com", Legajo = 44176, Matricula = "JOAQ-120", IsAdmin = true, Firstname = "Alejandro Pedro", Surname = "Recalde", Email = "alereca@gmail.com", Phone = 1502030 },
                new Usuario{ Id = 2, Username = "nico@example.com", Legajo = 44852, IsAdmin = true, Firstname = "Nicolás Agustín", Surname = "Antonelli", Email = "niconelli2@gmail.com", Phone = 1530012 },
                new Usuario{ Id = 3, Username = "RetroVitto", Matricula = "MECA-800", IsAdmin = true, Firstname = "Vittorio", Surname = "Retrivi", Email = "retrovitto@gmail.com", Phone = 1510911 },
                new Usuario{ Id = 4, Username = "alumnoProfe", Legajo = 30755, Matricula = "CBOL-555", IsAdmin = false, Firstname = "AluProf", Surname = "NoAdm", Email = "aluprofnoadm@gmail.com", Phone = 1503030 },
                new Usuario{ Id = 5, Username = "soloAlumno", Legajo = 40123, IsAdmin = false, Firstname = "Alumno", Surname = "Solo", Email = "aluonly@gmail.com", Phone = 1591111 },
                new Usuario{ Id = 6, Username = "soloProfe",  Matricula = "BRES-001", IsAdmin = false, Firstname = "Profesor", Surname = "Solo", Email = "profonly@gmail.com", Phone = 1592222 },
                new Usuario{ Id = 7, Username = "soloAdmin", IsAdmin = true, Firstname = "Admin", Surname = "Solo", Email = "adminonly@gmail.com", Phone = 1593333 },
                // The values that were not explicited, will have a Null value
            };

            // For the Users --> Remember Fix the username (Make a FK from IdentityFramework), and add "Photo" Attribue

            unitOfWork.Usuarios.InsertRange(usuarios);
            unitOfWork.Complete();

            var horariosConsulta = new List<HorarioConsulta> {
                new HorarioConsulta { Id = 1, Weekday = "Lunes", StartHour = "11:00", EndHour = "11:45", Place = "Aula 301", ProfesorId = usuarios[2].Id, Profesor = usuarios[2], MateriaId = materias[2].Id, Materia = materias[2] },
                new HorarioConsulta { Id = 2, Weekday = "Martes", StartHour = "09:30", EndHour = "10:15", Place = "Sala de Profesores", ProfesorId = usuarios[2].Id, Profesor = usuarios[3], MateriaId = materias[1].Id, Materia = materias[1] },
                // EliminationDate Must be only added if the HorarioConsulta were marked as "Deleted"
            };

            unitOfWork.HorariosConsulta.InsertRange(horariosConsulta);
            unitOfWork.Complete();
        }
    }
}
