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
using AutoMapper;
using DataAccessLayer;
using WebApi.DataTransferObjects;

namespace WebApi.Controllers
{
    [RoutePrefix("api/materias")]
    public class MateriasApiController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MateriasApiController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrives all materia intances
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            // Test of GetOrdered repository method
            var materias = _unitOfWork.Materias.GetOrdered(e => e.Year, e => e.Year > 1);

            return Ok(materias);
        }

        /// <summary>
        /// Retrives all materia intances
        /// </summary>
        [HttpGet]
        [Route("departamento")]
        public IHttpActionResult GetMateriasWithDepto()
        {
            var materias = _unitOfWork.Materias.GetMateriasWithDepto();

            return Ok(materias);
        }

        // GET api/Materia/5
        /// <summary>
        /// Retrives a specific materia
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(Materia))]
        public IHttpActionResult Get(int id)
        {
            var materia = _unitOfWork.Materias.GetById(id);

            if (materia == null)
            {
                return NotFound();
            }

            return Ok(materia);
        }

         // GET api/Materia/5/Departamento
        /// <summary>
        /// Retrives a specific materia
        /// </summary>
        [HttpGet]
        [Route("{id:int}/departamento")]
        [ResponseType(typeof(Materia))]
        public IHttpActionResult GetMateriaWithDepto(int id)
        {
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
        public IHttpActionResult Post([FromBody] CreateMateriaDTO materiaDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var materiaToInsert = _mapper.Map<CreateMateriaDTO, Materia>(materiaDTO);

                _unitOfWork.Materias.Insert(materiaToInsert);
                _unitOfWork.Complete();

                return CreatedAtRoute("PostMateria", new { id = materiaToInsert.Id }, materiaToInsert);
            }
            catch (Exception ex)
            {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }


        [Authorize]
        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(Materia))]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var materia = _unitOfWork.Materias.GetById(id);
                if (materia == null)
                {
                    return NotFound();
                }

                _unitOfWork.Materias.Delete(materia);
                _unitOfWork.Complete();

                return Ok(materia);
            }
            catch (Exception ex)
            {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        // Remember to include { Content-Type: application/json } and state the Id in in Request Body when consuming
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(Materia))]
        public IHttpActionResult Put(int id, [FromBody] Materia sentMateria)
        {
            if (id != sentMateria.Id)
            {
                return BadRequest();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Materias.Update(sentMateria);
                _unitOfWork.Complete();
            }
            catch(Exception)
            {
                if (_unitOfWork.Materias.GetById(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                } 
            }
            

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
