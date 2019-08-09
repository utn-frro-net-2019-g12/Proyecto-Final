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
    [RoutePrefix("api/usuarios")]
    public class UsuariosApiController : ApiController {
        private readonly IUnitOfWork _unitOfWork;

        public UsuariosApiController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrives all usuario instances
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll() {
            // Test of GetOrdered repository method
            var usuarios = _unitOfWork.Usuarios.Get();

            return Ok(usuarios);
        }

        // GET api/Usuario/5
        /// <summary>
        /// Retrives a specific usuario
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult Get(int id) {
            var usuario = _unitOfWork.Usuarios.GetById(id);

            if (usuario == null) {
                return NotFound();
            }

            return Ok(usuario);
        }

        // Remember to include { Content-Type: application/json } in Request Body when consuming
        [HttpPost]
        [Route("", Name = "PostUsuario")]
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult Post([FromBody] Usuario usuario) {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Usuarios.Insert(usuario);
                _unitOfWork.Complete();

                return CreatedAtRoute("PostUsuario", new { id = usuario.UserId }, usuario);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult Delete(int id) {
            try {
                var usuario = _unitOfWork.Usuarios.GetById(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                _unitOfWork.Usuarios.Delete(usuario);
                _unitOfWork.Complete();

                return Ok(usuario);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        // Remember to include { Content-Type: application/json } and state the Id in in Request Body when consuming
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult Put(int id, [FromBody] Usuario sentUsuario) {
            if (id != sentUsuario.UserId) {
                return BadRequest();
            }

            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Usuarios.Update(sentUsuario);
                _unitOfWork.Complete();
            }
            catch(Exception) {
                if (_unitOfWork.Usuarios.GetById(id) == null) {
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
