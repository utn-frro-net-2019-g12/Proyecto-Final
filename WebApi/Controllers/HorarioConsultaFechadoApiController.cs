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

    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/horariosConsultaFechados")]
    public class HorarioConsultaFechadoApiController : ApiController {
        private readonly IUnitOfWork _unitOfWork;

        public HorarioConsultaFechadoApiController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrives all horarioConsulta instances
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll() {
            var horariosConsultaFechados = _unitOfWork.HorariosConsultaFechados
                .GetHorariosConsultaFechadosWithProfesorAndMateria();

            return Ok(horariosConsultaFechados);
            // API Special Endpoint (No-Rest) --> [Route("profesor_materia")] (Unused)
        }


        // GET api/horarioConsulta/5
        /// <summary>
        /// Retrives a specific horarioConsulta
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(HorarioConsultaFechado))]
        public IHttpActionResult Get(int id) {
            var horarioConsultaFechado = _unitOfWork.HorariosConsultaFechados
                .GetHorarioConsultaFechadoWithProfesorAndMateria(id);

            if (horarioConsultaFechado == null)
            {
                return NotFound();
            }

            return Ok(horarioConsultaFechado);
        }

        // Remember to include { Content-Type: application/json } in Request Body when consuming
        [HttpPost]
        [Route("", Name = "PostHorarioConsultaFechado")]
        [ResponseType(typeof(HorarioConsultaFechado))]
        public IHttpActionResult Post([FromBody] HorarioConsultaFechado horarioConsultaFechado) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _unitOfWork.HorariosConsultaFechados.Insert(horarioConsultaFechado);
                _unitOfWork.Complete();

                return CreatedAtRoute("PostHorarioConsultaFechado", new { id = horarioConsultaFechado.Id },
                    horarioConsultaFechado);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(HorarioConsultaFechado))]
        public IHttpActionResult Delete(int id) {
            try {
                var horarioConsultaFechado = _unitOfWork.HorariosConsultaFechados.GetById(id);
                if (horarioConsultaFechado == null) {
                    return NotFound();
                }

                _unitOfWork.HorariosConsultaFechados.Delete(horarioConsultaFechado);
                _unitOfWork.Complete();

                return Ok(horarioConsultaFechado);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        // Remember to include { Content-Type: application/json } and state the Id in in Request Body when consuming
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(HorarioConsultaFechado))]
        public IHttpActionResult Put(int id, [FromBody] HorarioConsultaFechado sentHorarioConsultaFechado) {
            if (id != sentHorarioConsultaFechado.Id) {
                return BadRequest();
            }

            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _unitOfWork.HorariosConsultaFechados.Update(sentHorarioConsultaFechado);
                _unitOfWork.Complete();
            }
            catch(Exception) {
                if (_unitOfWork.HorariosConsultaFechados.GetById(id) == null) {
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
