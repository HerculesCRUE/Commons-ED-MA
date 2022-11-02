using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Hercules.CommonsEDMA.ServicioExterno.Models.RedesUsuario;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class RedesUsuarioController : ControllerBase
    {
        /// <summary>
        /// Controlador para obtener los datos de las fuentes de RO.
        /// </summary>
        /// <param name="pIdGnossUser">Usuario de gnoss.</param>
        /// <returns>Diccionario con los datos.</returns>
        [HttpGet("GetDatosRedesUsuario")]
        public IActionResult GetDatosRedesUsuario(string pIdGnossUser)
        {
            if (!Security.CheckUser(new Guid(pIdGnossUser), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            AccionesRedesUsuario accionDocumento = new AccionesRedesUsuario();
            List<DataUser> datosRedesUsuario = accionDocumento.GetDataRedesUsuario(pIdGnossUser);

            return Ok(datosRedesUsuario);
        }

        /// <summary>
        /// Controlador para modificar los datos de las fuentes de RO.
        /// </summary>
        /// <param name="pIdGnossUser">Usuario de gnoss.</param>
        /// <param name="pDataUser"></param>
        /// <returns>Diccionario con los datos.</returns>
        [HttpPost("SetDatosRedesUsuario")]
        public IActionResult SetDatosRedesUsuario([FromForm] string pIdGnossUser, [FromForm] User pDataUser)
        {
            if (!Security.CheckUser(new Guid(pIdGnossUser), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            AccionesRedesUsuario accionDocumento = new AccionesRedesUsuario();
            accionDocumento.SetDataRedesUsuario(pIdGnossUser, pDataUser);

            return Ok();
        }
    }
}
