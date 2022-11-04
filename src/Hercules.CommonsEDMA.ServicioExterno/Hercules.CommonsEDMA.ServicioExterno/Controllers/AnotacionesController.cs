using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Hercules.CommonsEDMA.ServicioExterno.Models.Offer;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class AnotacionesController : ControllerBase
    {
        /// <summary>
        /// Controlador para obtener las anotaciones de un investigador en un RO concreto.
        /// Los valores posibles de la ontología serían actualmente:
        /// "http://purl.org/ontology/bibo/Document", "document"
        /// "http://w3id.org/roh/ResearchObject", "researchobject"
        /// </summary>
        /// <param name="idRO">Id del RO</param>
        /// <param name="idUser">Id del usuario </param>
        /// <param name="rdfType">rdfType de la ontología </param>
        /// <param name="ontology">Nombre de la ontología </param>
        /// <returns>Diccionario con los datos.</returns>
        [HttpPost("GetOwnAnnotationsInRO")]
        public IActionResult GetOwnAnnotationsInRO([FromForm] string idRO, [FromForm] string idUser, [FromForm] string rdfType, [FromForm] string ontology)
        {
            //Solo puede obtener duplicados el propietario del CV
            if (!Security.CheckUser(new Guid(idUser), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            AccionesAnotaciones annotations = new();
            List<Dictionary<string, string>> anotaciones = annotations.GetOwnAnnotationsInRO(idRO, idUser, rdfType, ontology);

            return Ok(anotaciones);
        }



        /// <summary>
        /// Controlador para crear una nueva anotación.
        /// Los valores posibles de la ontología serían actualmente:
        /// "http://purl.org/ontology/bibo/Document", "document"
        /// "http://w3id.org/roh/ResearchObject", "researchobject"
        /// </summary>
        /// <param name="idRO">Id del RO</param>
        /// <param name="idUser">Id (GUID) del usuario </param>
        /// <param name="rdfType">rdfType de la ontología </param>
        /// <param name="ontology">Nombre de la ontología </param>
        /// <param name="texto">Texto de la anotación</param>
        /// <param name="idAnnotation">Id de la anotación (si se guarda)</param>
        /// <returns>Diccionario con los datos.</returns>
        [HttpPost("CreateNewAnnotation")]
        public IActionResult CreateNewAnnotation([FromForm] string idRO, [FromForm] string idUser, [FromForm] string rdfType, [FromForm] string ontology, [FromForm] string texto, [FromForm] string idAnnotation = null)
        {
            if (!Security.CheckUser(new Guid(idUser), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            AccionesAnotaciones annotations = new();
            string anotacionesId = annotations.CreateNewAnnotation(idRO, idUser, rdfType, ontology, texto, idAnnotation);

            return Ok(anotacionesId);
        }

        /// <summary>
        /// Metodo para eliminar una anotacion 
        /// </summary>
        /// <param name="idAnnotation">Id de la anotacion a eliminar</param>
        /// <returns>Bool si ha sido eliminada</returns>
        [HttpPost("DeleteAnnotation")]
        public IActionResult DeleteAnnotation([FromForm] string idAnnotation)
        {
            AccionesAnotaciones annotations = new();
            if (!Security.CheckUser(new Guid(annotations.getUserFromAnnotation(idAnnotation)), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            return Ok(annotations.DeleteAnnotation(idAnnotation));
        }

    }
}
