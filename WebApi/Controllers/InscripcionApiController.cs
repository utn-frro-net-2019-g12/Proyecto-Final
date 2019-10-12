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
using DataAccessLayer;

namespace WebApi.Controllers {

    [Authorize]
    [RoutePrefix("api/inscripciones")]
    public class InscripcionApiController : ApiController {
        private readonly IUnitOfWork _unitOfWork;

        public InscripcionApiController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrives all inscripcion instances
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll() {
            var inscripciones = _unitOfWork.Inscripciones.GetInscripcionesWithAlumnoAndHorario();

            return Ok(inscripciones);
            // API Special Endpoint (No-Rest) --> [Route("alumno_horario")] (Unused)
        }


        // GET api/inscripcion/5
        /// <summary>
        /// Retrives a specific inscripcion
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(Inscripcion))]
        public IHttpActionResult Get(int id) {
            var inscripcion = _unitOfWork.Inscripciones.GetInscripcionWithAlumnoAndHorario(id);

            if (inscripcion == null)
            {
                return NotFound();
            }

            return Ok(inscripcion);
        }

        // Remember to include { Content-Type: application/json } in Request Body when consuming
        [HttpPost]
        [Route("", Name = "PostInscripcion")]
        [ResponseType(typeof(Inscripcion))]
        public IHttpActionResult Post([FromBody] Inscripcion inscripcion) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Inscripciones.Insert(inscripcion);
                _unitOfWork.Complete();

                return CreatedAtRoute("PostInscripcion", new { id = inscripcion.Id }, inscripcion);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(Inscripcion))]
        public IHttpActionResult Delete(int id) {
            try {
                var inscripcion = _unitOfWork.Inscripciones.GetById(id);
                if (inscripcion == null) {
                    return NotFound();
                }

                _unitOfWork.Inscripciones.Delete(inscripcion);
                _unitOfWork.Complete();

                return Ok(inscripcion);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        // Remember to include { Content-Type: application/json } and state the Id in in Request Body when consuming
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(Inscripcion))]
        public IHttpActionResult Put(int id, [FromBody] Inscripcion sentInscripcion) {
            if (id != sentInscripcion.Id) {
                return BadRequest();
            }

            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Inscripciones.Update(sentInscripcion);
                _unitOfWork.Complete();
            }
            catch(Exception) {
                if (_unitOfWork.Inscripciones.GetById(id) == null) {
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
