using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using DataAccess;
using Microsoft.AspNet.Identity;

namespace Service.Controllers {

    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/horariosConsulta")]
    public class HorarioConsultaApiController : ApiController {
        private readonly IUnitOfWork _unitOfWork;

        public HorarioConsultaApiController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrives all horarioConsulta instances
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll() {
            var horariosConsulta = _unitOfWork.HorariosConsulta.GetHorariosConsultaWithProfesorAndMateria();

            return Ok(horariosConsulta);
            // API Special Endpoint (No-Rest) --> [Route("profesor_materia")] (Unused)
        }

        /// <summary>
        /// Retrives horariosConsulta instances that matches a partial description
        /// </summary>
        [HttpGet]
        [Route("search")]
        public IHttpActionResult GetByPartialDescription(string desc) {
            var horariosConsulta = _unitOfWork.HorariosConsulta.GetHorariosConsultaByPartialDesc(desc);
            return Ok(horariosConsulta);
        }

        /// <summary>
        /// Retrives horarioConsulta instances that matches with an id_profesor
        /// </summary>
        [HttpGet]
        [Route("profesores/{id_profesor:int}")]
        public IHttpActionResult GetByProfesor(int id_profesor) {
            var horariosConsulta = _unitOfWork.HorariosConsulta.GetHorariosConsultaByProfesor(id_profesor);
            return Ok(horariosConsulta);
        }


        /// <summary>
        /// Retrives horarioConsulta instances that matches with current profesor
        /// </summary>
        [HttpGet]
        [Route("profesores/current")]
        public IHttpActionResult GetByCurrentProfesor()
        {
            string username = RequestContext.Principal.Identity.GetUserName();

            var usuario = _unitOfWork.Usuarios.GetUsuarioByUsername(username);

            var horariosConsulta = _unitOfWork.HorariosConsulta.GetHorariosConsultaByProfesor(usuario.Id);
            return Ok(horariosConsulta);
        }

        /// <summary>
        /// Retrives horarioConsulta instances that matches with an id_materia
        /// </summary>
        [HttpGet]
        [Route("materias/{id_materia:int}")]
        public IHttpActionResult GetByMateriaOrderByProfesor(int id_materia) {
            var horariosConsulta = _unitOfWork.HorariosConsulta.GetHorariosConsultaByMateriaOrderByProfesor(id_materia);
            return Ok(horariosConsulta);
        }

        // GET api/horarioConsulta/5
        /// <summary>
        /// Retrives a specific horarioConsulta
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(HorarioConsulta))]
        public IHttpActionResult Get(int id) {
            var horarioConsulta = _unitOfWork.HorariosConsulta.GetHorarioConsultaWithProfesorAndMateria(id);

            if (horarioConsulta == null)
            {
                return NotFound();
            }

            return Ok(horarioConsulta);
        }

        // Remember to include { Content-Type: application/json } in Request Body when consuming
        [HttpPost]
        [Route("", Name = "PostHorarioConsulta")]
        [ResponseType(typeof(HorarioConsulta))]
        public IHttpActionResult Post([FromBody] HorarioConsulta horarioConsulta) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _unitOfWork.HorariosConsulta.Insert(horarioConsulta);
                _unitOfWork.Complete();

                return CreatedAtRoute("PostHorarioConsulta", new { id = horarioConsulta.Id }, horarioConsulta);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(HorarioConsulta))]
        public IHttpActionResult Delete(int id) {
            try {
                var horarioConsulta = _unitOfWork.HorariosConsulta.GetById(id);
                if (horarioConsulta == null) {
                    return NotFound();
                }

                _unitOfWork.HorariosConsulta.Delete(horarioConsulta);
                _unitOfWork.Complete();

                return Ok(horarioConsulta);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        // Remember to include { Content-Type: application/json } and state the Id in in Request Body when consuming
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(HorarioConsulta))]
        public IHttpActionResult Put(int id, [FromBody] HorarioConsulta sentHorarioConsulta) {
            if (id != sentHorarioConsulta.Id) {
                return BadRequest();
            }

            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _unitOfWork.HorariosConsulta.Update(sentHorarioConsulta);
                _unitOfWork.Complete();
            }
            catch(Exception) {
                if (_unitOfWork.HorariosConsulta.GetById(id) == null) {
                    return NotFound();
                }
                else {
                    throw;
                } 
            }
            
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
