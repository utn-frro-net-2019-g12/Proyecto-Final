using System.Collections.Generic;

namespace DataAccess {
    public class ConsultaUTNInitializer : System.Data.Entity.DropCreateDatabaseAlways<ConsultaUTNContext> {
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
                new Materia{ Name = "Algoritmos Genéticos", Year = 3, IsElectiva = true, DepartamentoId = departamentos[0].Id },
                new Materia{ Name = "Diseño de Sistemas", Year = 3, IsElectiva = false, DepartamentoId = departamentos[0].Id },
                new Materia{ Name = "Análisis Matemático 2", Year = 2, IsElectiva = false, DepartamentoId = departamentos[5].Id },
            };

            unitOfWork.Materias.InsertRange(materias);
            unitOfWork.Complete();

            var usuarios = new List<Usuario> {
                // Users Test: 1 Admin Full, 2 Alumno-Admin, 3 Profesor-Admin, 4 Alumno-Profesor, 5 Alumno Only, 6 Profesor Only, 7 Admin Only
                new Usuario{ Username = "ale@example.com", Legajo = 44176, Matricula = "JOAQ-120", IsAdmin = true, Firstname = "Alejandro Pedro", Surname = "Recalde", Email = "alereca@gmail.com", Phone1 = 1502030 },
                new Usuario{ Username = "nico@example.com", Legajo = 44852, IsAdmin = true, Firstname = "Nicolás Agustín", Surname = "Antonelli", Email = "niconelli2@gmail.com", Phone1 = 1530012 },
                new Usuario{ Username = "RetroVitto", Matricula = "MECA-800", IsAdmin = true, Firstname = "Vittorio", Surname = "Retrivi", Email = "retrovitto@gmail.com", Phone1 = 1510911 },
                new Usuario{ Username = "alumnoProfe", Legajo = 30755, Matricula = "PORT-555", IsAdmin = false, Firstname = "AluProf", Surname = "NoAdm", Email = "aluprofnoadm@gmail.com", Phone1 = 1503030 },
                new Usuario{ Username = "soloAlumno", Legajo = 40123, IsAdmin = false, Firstname = "Alumno", Surname = "Solo", Email = "aluonly@gmail.com", Phone1 = 1591111 },
                new Usuario{ Username = "soloProfe",  Matricula = "BRES-001", IsAdmin = false, Firstname = "Profesor", Surname = "Solo", Email = "profonly@gmail.com", Phone1 = 1592222 },
                new Usuario{ Username = "soloAdmin", IsAdmin = true, Firstname = "Admin", Surname = "Solo", Email = "adminonly@gmail.com", Phone1 = 1593333, Phone2 = 0800999 },
            };

            // For the Users --> Remember Fix the username (Make a FK from IdentityFramework), and add "Photo" Attribue

            unitOfWork.Usuarios.InsertRange(usuarios);
            unitOfWork.Complete();

            var horariosConsulta = new List<HorarioConsulta> {
                new HorarioConsulta { Weekday = "Lunes", StartHour = "11:00", EndHour = "11:45", Place = "Aula 301", ProfesorId = usuarios[2].Id, MateriaId = materias[2].Id },
                new HorarioConsulta { Weekday = "Martes", StartHour = "09:30", EndHour = "10:15", Place = "Sala de Profesores", ProfesorId = usuarios[0].Id, MateriaId = materias[1].Id },
                // EliminationDate Must be only added if the HorarioConsulta were marked as "Deleted"
            };

            unitOfWork.HorariosConsulta.InsertRange(horariosConsulta);
            unitOfWork.Complete();

            var horariosConsultaFechados = new List<HorarioConsultaFechado> {
                new HorarioConsultaFechado { HorarioConsultaId = horariosConsulta[0].Id, Date = new System.DateTime(2019, 11, 04), State = HorarioConsultaFechado.HCFStates.active },
                new HorarioConsultaFechado { HorarioConsultaId = horariosConsulta[0].Id, Date = new System.DateTime(2019, 11, 11), State = HorarioConsultaFechado.HCFStates.active },
                new HorarioConsultaFechado { HorarioConsultaId = horariosConsulta[1].Id, Date = new System.DateTime(2019, 11, 05), State = HorarioConsultaFechado.HCFStates.active },
                new HorarioConsultaFechado { HorarioConsultaId = horariosConsulta[1].Id, Date = new System.DateTime(2019, 11, 12), State = HorarioConsultaFechado.HCFStates.active },
            };

            unitOfWork.HorariosConsultaFechados.InsertRange(horariosConsultaFechados);
            unitOfWork.Complete();

            var inscripciones = new List<Inscripcion> {
                new Inscripcion { Topic = "Derivadas", State = Inscripcion.InscripcionStates.active, AlumnoId = usuarios[1].Id, Alumno = usuarios[1], HorarioConsultaFechadoId = horariosConsultaFechados[1].Id },
                new Inscripcion { Topic = "Axure", State = Inscripcion.InscripcionStates.active, AlumnoId = usuarios[3].Id, Alumno = usuarios[3], HorarioConsultaFechadoId = horariosConsultaFechados[3].Id },
                // State = Active/Deleted/Finalized, Answer = Fast Response Optional for a Profersor, Observation = Also Optional
            };

            unitOfWork.Inscripciones.InsertRange(inscripciones);
            unitOfWork.Complete();

        }
    }
}
