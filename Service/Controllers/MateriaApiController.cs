﻿using System;
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

namespace Service.Controllers {

    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/materias")]
    public class MateriaApiController : ApiController {
        private readonly IUnitOfWork _unitOfWork;

        public MateriaApiController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Retrives all materia instances
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll() {
            // Test of GetOrdered repository method
            var materias = _unitOfWork.Materias.GetOrdered(e => e.Year, e => e.Year > 1);

            return Ok(materias);
        }

        /// <summary>
        /// Retrives all materia intances
        /// </summary>
        [HttpGet]
        [Route("departamentos")]
        public IHttpActionResult GetMateriasWithDepto() {
            var materias = _unitOfWork.Materias.GetMateriasWithDepto();

            return Ok(materias);
        }

        /// <summary>
        /// Retrives materia instances that matches a partial description
        /// </summary>
        [HttpGet]
        [Route("search")]
        public IHttpActionResult GetByPartialDescription(string desc) {
            var materias = _unitOfWork.Materias.GetMateriasByPartialDesc(desc);
            return Ok(materias);
        }
        
        /// <summary>
        /// Retrives materia instances that matches with an id_departamento
        /// </summary>
        [HttpGet]
        [Route("departamentos/{id_departamento:int}")]
        public IHttpActionResult GetByDepartamento(int id_departamento) {
            var materias = _unitOfWork.Materias.GetMateriasByDepto(id_departamento);
            return Ok(materias);
        }

        // GET api/materia/5
        /// <summary>
        /// Retrives a specific materia
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(Materia))]
        public IHttpActionResult Get(int id) {
            var materia = _unitOfWork.Materias.GetById(id);

            if (materia == null)
            {
                return NotFound();
            }

            return Ok(materia);
        }

        // GET api/materia/5/Departamento
        /// <summary>
        /// Retrives a specific materia
        /// </summary>
        [HttpGet]
        [Route("{id:int}/departamento")]
        [ResponseType(typeof(Materia))]
        public IHttpActionResult GetMateriaWithDepto(int id) {
            var materia = _unitOfWork.Materias.GetMateriaWithDepto(id);

            if (materia == null)
            {
                return NotFound();
            }

            return Ok(materia);
        }

        // Remember to include { Content-Type: application/json } in Request Body when consuming
        [HttpPost]
        [Route("", Name = "PostMateria")]
        [ResponseType(typeof(Materia))]
        public IHttpActionResult Post([FromBody] Materia materia) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Materias.Insert(materia);
                _unitOfWork.Complete();

                return CreatedAtRoute("PostMateria", new { id = materia.Id }, materia);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(Materia))]
        public IHttpActionResult Delete(int id) {
            try {
                var materia = _unitOfWork.Materias.GetById(id);
                if (materia == null) {
                    return NotFound();
                }

                _unitOfWork.Materias.Delete(materia);
                _unitOfWork.Complete();

                return Ok(materia);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        // Remember to include { Content-Type: application/json } and state the Id in in Request Body when consuming
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(Materia))]
        public IHttpActionResult Put(int id, [FromBody] Materia sentMateria) {
            if (id != sentMateria.Id) {
                return BadRequest();
            }

            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Materias.Update(sentMateria);
                _unitOfWork.Complete();
            }
            catch(Exception) {
                if (_unitOfWork.Materias.GetById(id) == null) {
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
