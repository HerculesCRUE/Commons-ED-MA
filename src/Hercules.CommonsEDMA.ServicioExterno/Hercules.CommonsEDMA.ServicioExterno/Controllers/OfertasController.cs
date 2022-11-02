using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Hercules.CommonsEDMA.ServicioExterno.Models.Offer;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hercules.CommonsEDMA.ServicioExterno.Models.Cluster;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class OfertasController : ControllerBase
    {


        /// <summary>
        /// Controlador para obtener los thesaurus usados por las ofertas.
        /// </summary>
        /// <param name="listThesaurus">Elemento padre que define el thesaurus</param>
        /// <param name="lang">Idioma para los thesaurus multiidioma </param>
        /// <returns>*Object* Diccionario con los datos. (Diccionario clave -> listado de thesaurus).</returns>
        [HttpPost("GetThesaurus")]
        public IActionResult GetThesaurus([FromForm] List<string> listThesaurus, [FromForm] string lang = "es")
        {
            AccionesOferta cluster = new AccionesOferta();
            Dictionary<string, List<ThesaurusItem>> datosThesaurus = cluster.GetListThesaurus(listThesaurus, lang);

            return Ok(datosThesaurus);
        }


        /// <summary>
        /// Borra una oferta
        /// </summary>
        /// <param name="pIdOfferId">Id de la oferta a borrar.</param>
        /// <param name="pIdGnossUser">Id del usuario que realiza la acción.</param>
        /// <returns>Un booleano si ha sido borrado.</returns>
        [HttpPost("BorrarOferta")]
        public IActionResult BorrarOferta([FromForm] string pIdOfferId, [FromForm] Guid pIdGnossUser)
        {
            AccionesOferta accionCluster = new AccionesOferta();
            bool borrado = accionCluster.BorrarOferta(pIdOfferId, pIdGnossUser);

            return Ok(borrado);
        }


        /// <summary>
        /// Cambiar el estado de una oferta
        /// </summary>
        /// <param name="pIdOfferId">Id(GUID) de la oferta a modificar.</param>
        /// <param name="estado">Id del estado al que se quiere establecer.</param>
        /// <param name="estadoActual">Id del estado que tiene actualmente (Necesario para la modificación del mismo).</param>
        /// <param name="pIdGnossUser">Id del usuario que modifica el estado, necesario para actualizar el historial.</param>
        /// <param name="texto">Texto de la notificación (Opcional) que contiene el mensaje personalizado para la notificación.</param>
        /// <returns>String con el id del nuevo estado.</returns>
        [HttpPost("CambiarEstado")]
        public IActionResult CambiarEstado([FromForm] string pIdOfferId, [FromForm] string estado, [FromForm] string estadoActual, [FromForm] Guid pIdGnossUser, [FromForm] string texto = "")
        {
            AccionesOferta accionCluster = new AccionesOferta();
            string cambiado = accionCluster.CambiarEstado(pIdOfferId, estado, estadoActual, pIdGnossUser, texto);

            return Ok(cambiado);
        }

        /// <summary>
        /// Cambiar el estado de un listado de ofertas
        /// </summary>
        /// <param name="pIdOfferIds">Ids (array de GUIDs) de las ofertas a modificar.</param>
        /// <param name="estado">Id del estado al que se quiere establecer.</param>
        /// <param name="estadoActual">Id del estado que tiene actualmente (Necesario para la modificación del mismo).</param>
        /// <param name="pIdGnossUser">Id del usuario que modifica el estado, necesario para actualizar el historial.</param>
        /// <param name="texto">Texto de la notificación.</param>
        /// <returns>bool indicando si se han hecho los cambios o no.</returns>
        [HttpPost("CambiarEstadoAll")]
        public IActionResult CambiarEstadoAll([FromForm] Guid[] pIdOfferIds, [FromForm] string estado, [FromForm] string estadoActual, [FromForm] Guid pIdGnossUser, [FromForm] string texto = "")
        {

            bool cambiado = true;
            AccionesOferta accionCluster = new AccionesOferta();
            foreach (var pIdOfferId in pIdOfferIds)
            {
                cambiado = cambiado && accionCluster.CambiarEstado(pIdOfferId.ToString(), estado, estadoActual, pIdGnossUser, texto) != String.Empty;
            }

            return Ok(cambiado);
        }

        /// <summary>
        /// Controlador para cargar los datos de la oferta.
        /// </summary>
        /// <param name="pIdOfertaId">Id de la oferta.</param>
        /// <returns>Objeto "leible" de la oferta.</returns>
        [HttpGet("LoadOffer")]
        public IActionResult LoadOffer([Required] string pIdOfertaId)
        {
            AccionesOferta accionCluster = new AccionesOferta();
            Offer Oferta = accionCluster.LoadOffer(pIdOfertaId);

            return Ok(Oferta);
        }

        /// <summary>
        /// Controlador para Obtener los usuarios del/los grupos de un investigador
        /// </summary>
        /// <param name="pIdUserId">Usuario investigador.</param>
        /// <returns>Diccionario con los datos necesarios para cada persona.</returns>
        [HttpGet("LoadUsersGroup")]
        public IActionResult LoadUsers([Required] string pIdUserId)
        {
            AccionesOferta accionOferta = new AccionesOferta();
            return Ok(accionOferta.LoadUsers(pIdUserId));
        }

        /// <summary>
        /// Controlador para Obtener las líneas de invetigación de los grupos de los usuarios investigadores dados
        /// </summary>
        /// <param name="pIdUsersId">Usuarios investigadores.</param>
        /// <returns>Listado de las líneas de investigación.</returns>
        [HttpPost("LoadLineResearchs")]
        public IActionResult LoadLineResearchs([FromForm] string[] pIdUsersId)
        {
            AccionesOferta accionOferta = new AccionesOferta();
            return Ok(accionOferta.LoadLineResearchs(pIdUsersId));
        }

        /// <summary>
        /// Controlador para Obtener los sectores de encuadre
        /// </summary>
        /// <param name="lang">Idioma a cargar.</param>
        /// <returns>Listado de las líneas de investigación.</returns>
        [HttpGet("LoadFramingSectors")]
        public IActionResult LoadFramingSectors(string lang)
        {
            AccionesOferta accionOferta = new AccionesOferta();
            return Ok(accionOferta.LoadFramingSectors(lang));
        }

        /// <summary>
        /// Controlador para Obtener los estados de madurez de las ofertas tecnológicas
        /// </summary>
        /// <param name="lang">Idioma a cargar.</param>
        /// <returns>Listado de los estados de madurez.</returns>
        [HttpGet("LoadMatureStates")]
        public IActionResult LoadMatureStates(string lang)
        {
            AccionesOferta accionOferta = new AccionesOferta();
            return Ok(accionOferta.LoadMatureStates(lang));
        }

        /// <summary>
        /// Controlador para crear/actualizar los datos de la oferta 
        /// </summary>
        /// <param name="pIdGnossUser">Usuario de gnoss que realiza la acción.</param>
        /// <param name="oferta">Objeto con la oferta tecnológica a crear/actualizar.</param>
        /// <returns>Id de la oferta creada o modificada.</returns>
        [HttpPost("SaveOffer")]
        [Produces("application/json")]
        public IActionResult SaveOffer([FromForm] Guid pIdGnossUser, [FromForm] Offer oferta)
        {
            AccionesOferta accionOferta = new AccionesOferta();
            return Ok(accionOferta.SaveOffer(pIdGnossUser, oferta));
        }

        /// <summary>
        /// Controlador que lista el perfil de usuarios al que pertenece el usuario actual respecto a una oferta tecnológica dada 
        /// </summary>
        /// <param name="pIdOfertaId">Id de la oferta tecnológica.</param>
        /// <param name="userId">Usuario de gnoss que realiza la acción.</param>
        /// <returns>Objeto json.</returns>
        [HttpPost("GetUserProfileInOffer")]
        public IActionResult GetUserProfileInOffer([FromForm] string pIdOfertaId, [FromForm] Guid userId)
        {
            AccionesOferta accionOferta = new AccionesOferta();
            return Ok(accionOferta.CheckUpdateActionsOffer(pIdOfertaId, userId));
        }

        /// <summary>
        /// Controlador para comprobar si es un usuario otri 
        /// </summary>
        /// <param name="pIdGnossUser">Usuario de gnoss que realiza la acción.</param>
        /// <returns>Bool si es otri.</returns>
        [HttpGet("CheckIfIsOtri")]
        public IActionResult CheckIfIsOtri([Required] Guid pIdGnossUser)
        {
            AccionesOferta accionOferta = new AccionesOferta();
            return Ok(accionOferta.CheckIfIsOtri(pIdGnossUser));
        }
    }
}
