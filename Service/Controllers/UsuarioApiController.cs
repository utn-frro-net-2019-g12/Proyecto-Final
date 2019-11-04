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
using Service.Models;

namespace Service.Controllers {

    [RoutePrefix("api/usuarios")]
    public class UsuarioApiController : ApiController {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioApiController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrives all usuario instances
        /// </summary>
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAll() {
            // Test of GetOrdered repository method
            var usuarios = _unitOfWork.Usuarios.Get();

            return Ok(usuarios);
        }

        /// <summary>
        /// Retrives All usuario (profesor) instances ordered by Full Name
        /// </summary>
        [HttpGet]
        [Route("profesores")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAllProfesores() {
            var profesores = _unitOfWork.Usuarios.GetUsuariosProfesoresOrderedByFullName();
            return Ok(profesores);
        }

        /// <summary>
        /// Retrives All usuario (alumno) instances ordered by Legajo
        /// </summary>
        [HttpGet]
        [Route("alumnos")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAllAlumnos() {
            var alumnos = _unitOfWork.Usuarios.GetUsuariosAlumnosOrderedByLegajo();
            return Ok(alumnos);
        }

        /// <summary>
        /// Retrives usuario instances that matches a partial description
        /// </summary>
        [HttpGet]
        [Route("search")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetByPartialDescription(string desc) {
            var usuarios = _unitOfWork.Usuarios.GetUsuariosByPartialDesc(desc);
            return Ok(usuarios);
        }

        /// <summary>
        /// Retrives usuario (profesor) instances that matches a partial description
        /// </summary>
        [HttpGet]
        [Route("profesores/search")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetProfesoresByPartialDescription(string desc) {
            var profesores = _unitOfWork.Usuarios.GetUsuariosProfesoresByPartialDesc(desc);
            return Ok(profesores);
        }

        /// <summary>
        /// Retrives usuario (alumno) instances that matches a partial description
        /// </summary>
        [HttpGet]
        [Route("alumnos/search")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAlumnosByPartialDescription(string desc) {
            var alumnos = _unitOfWork.Usuarios.GetUsuariosAlumnosByPartialDesc(desc);
            return Ok(alumnos);
        }

        // GET api/usuario/5
        /// <summary>
        /// Retrives a specific usuario
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult Get(int id) {
            var usuario = _unitOfWork.Usuarios.GetById(id);

            if (usuario == null) {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpGet]
        [Authorize]
        [Route("current")]
        public IHttpActionResult GetByRequestData()
        {
            string userName = RequestContext.Principal.Identity.GetUserName();

            var usuario = _unitOfWork.Usuarios.GetUsuarioByUsername(userName);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // Remember to include { Content-Type: application/json } in Request Body when consuming
        [HttpPost]
        [Route("", Name = "PostUsuario")]
        [ResponseType(typeof(Usuario))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Post([FromBody] Usuario usuario) {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Usuarios.Insert(usuario);
                _unitOfWork.Complete();

                return CreatedAtRoute("PostUsuario", new { id = usuario.Id }, usuario);
            }
            catch (Exception ex) {
                // Send the exception as parameter
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
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
            if (id != sentUsuario.Id) {
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

        [HttpPut]
        [Route("current")]
        [Authorize]
        public IHttpActionResult UpdateCurrent([FromBody] UpdateCurrentUsuarioModel user)
        {
            /*if(User.Identity.Name != current.Username)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }*/

            try
            {
                Usuario current = _unitOfWork.Usuarios.GetUsuarioByUsername(User.Identity.Name);

                current.Firstname = user.Firstname;
                current.Surname = user.Surname;
                current.Email = user.Email;
                current.Phone1 = user.Phone1;
                current.Phone2 = user.Phone2;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _unitOfWork.Usuarios.Update(current);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
