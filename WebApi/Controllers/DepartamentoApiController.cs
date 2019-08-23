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
    [RoutePrefix("api/departamentos")]
    public class DepartamentoApiController : ApiController {
        private UnitOfWork _unitOfWork = new UnitOfWork(new ConsultaUTNContext());

        /// <summary>
        /// Retrives all departamento instances
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll() {
            var departamentos = _unitOfWork.Departamentos.Get();

            return Ok(departamentos);
        }

        // GET api/departamento/5
        /// <summary>
        /// Retrives a specific departamento
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(Departamento))]
        public IHttpActionResult Get(int id) {
            var departamento = _unitOfWork.Departamentos.GetById(id);

            if (departamento == null) {
                return NotFound();
            }

            return Ok(departamento);
        }

        // Remember to include { Content-Type: application/json } in Request Body when consuming
        [HttpPost]
        [Route("", Name = "PostDepartamento")]
        [ResponseType(typeof(Departamento))]
        public IHttpActionResult Post([FromBody] Departamento departamento) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Departamentos.Insert(departamento);
                _unitOfWork.Complete();

                return CreatedAtRoute("PostDepartamento", new { id = departamento.Id }, departamento);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(Departamento))]
        public IHttpActionResult Delete(int id) {
            try {
                var departamento = _unitOfWork.Departamentos.GetById(id);
                if (departamento == null) {
                    return NotFound();
                }

                _unitOfWork.Departamentos.Delete(departamento);
                _unitOfWork.Complete();

                return Ok(departamento);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        // Remember to include { Content-Type: application/json } and state the Id in in Request Body when consuming
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(Departamento))]
        public IHttpActionResult Put(int id, [FromBody] Departamento sentDepartamento) {
            if (id != sentDepartamento.Id) {
                return BadRequest();
            }

            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Departamentos.Update(sentDepartamento);
                _unitOfWork.Complete();
            }
            catch(Exception) {
                if (_unitOfWork.Departamentos.GetById(id) == null) {
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
