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
using Hercules.CommonsEDMA.ServicioExterno.Models;
using System.IO;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class RosVinculadosController : ControllerBase
    {
        readonly ConfigService _Configuracion;

        public RosVinculadosController(ConfigService pConfig)
        {
            _Configuracion = pConfig;

        }

        /// <summary>
        /// Borra un vínculo
        /// </summary>
        /// <param name="resourceRO">Id (Guid) del RO relacionado.</param>
        /// <param name="pIdROId">Id (Guid) del RO a eliminar de vinculados.</param>
        /// <param name="pIdGnossUser">Id del usuario que realiza la acción.</param>
        /// <returns>Un booleano si ha sido borrado.</returns>
        [HttpPost("DeleteLinked")]
        public IActionResult DeleteLinked([FromForm] string resourceRO, [FromForm] string pIdROId, [FromForm] Guid pIdGnossUser)
        {
            if (!Security.CheckUser(pIdGnossUser, Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            bool borrado;
            try
            {
                AccionesRosVinculados accionesRosVinculados = new();
                borrado = accionesRosVinculados.DeleteLinked(resourceRO, pIdROId, pIdGnossUser, _Configuracion);
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(borrado);
        }


        /// <summary>
        /// Controlador para Obtener los ROs vinculados de un RO en concreto
        /// </summary>
        /// <param name="pIdROId">ID del RO a obtener las relaciones.</param>
        /// <param name="lang">Idioma de los literales para la consulta</param>
        /// <returns>Listado de los RO vinculados.</returns>
        [HttpGet("LoadRosLinked")]
        public IActionResult LoadRosLinked(string pIdROId, string lang = "es")
        {
            try
            {
                AccionesRosVinculados accionesRosVinculados = new();
                return Ok(accionesRosVinculados.LoadRosLinked(pIdROId, lang));
            }
            catch (Exception)
            {
                throw;
            }
        }




        /// <summary>
        /// Controlador para Obtener los ROs vinculados de un RO en concreto
        /// </summary>
        /// <param name="text">String a buscar</param>
        /// <param name="pIdGnossUser">Id del usuario que modifica el estado, necesario para actualizar el historial</param>
        /// <param name="listItemsRelated">Ids de ROs seleccionados</param>
        /// <returns>Listado de los RO vinculados.</returns>
        [HttpPost("SearchROs")]
        [Produces("application/json")]
        public IActionResult SearchROs([FromForm] string text, [FromForm] string pIdGnossUser, [FromForm] List<string> listItemsRelated)
        {
            if (!Security.CheckUser(new Guid(pIdGnossUser), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            try
            {
                AccionesRosVinculados accionesRosVinculados = new();
                return Ok(accionesRosVinculados.SearchROs(text, pIdGnossUser, listItemsRelated));
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Controlador para crear una vinculación 
        /// </summary>
        /// <param name="resourceRO">Id (Guid) del RO relacionado.</param>
        /// <param name="pIdROId">Id (Guid) del RO a añadir a vinculados.</param>
        /// <param name="pIdGnossUser">Id del usuario que realiza la acción.</param>
        /// <returns>Id de la oferta creada o modificada.</returns>
        [HttpPost("AddLink")]
        [Produces("application/json")]
        public IActionResult AddLink([FromForm] string resourceRO, [FromForm] string pIdROId, [FromForm] Guid pIdGnossUser)
        {

            if (!Security.CheckUser(pIdGnossUser, Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            try
            {
                AccionesRosVinculados accionesRosVinculados = new AccionesRosVinculados();
                return Ok(accionesRosVinculados.AddLink(resourceRO, pIdROId, pIdGnossUser, _Configuracion));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("Addtripe")]
        public IActionResult Addtripe()
        {

            return null;

            Dictionary<Guid, bool> result = new();


            string RUTA_OAUTH = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config";
            Gnoss.ApiWrapper.ResourceApi mResourceApi = new Gnoss.ApiWrapper.ResourceApi(RUTA_OAUTH);

            // Añadir cambio en el historial de la disponibilidad
            // Comprueba si el id del recuro no está vacío
            mResourceApi.ChangeOntoly("patent");

            // Añado el vículo
            try
            {
                String select = @"select ?s ?authorList ";
                String where = @$"where{{
                    ?s a <http://purl.org/ontology/bibo/Patent>.
                    ?s <http://purl.org/ontology/bibo/authorList> ?authorList.
                    FILTER(?s=<http://gnoss.com/items/Patent_6075306e-9462-4fde-9878-15b734af1452_d04af817-ffe7-4996-b27a-0f88ade7f068>)
                }}";
                Gnoss.ApiWrapper.ApiModel.SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "patent");
                foreach (Dictionary<string, Gnoss.ApiWrapper.ApiModel.SparqlObject.Data> fila in resultado.results.bindings)
                {
                    string patent = fila["s"].value;
                    string authorList = fila["authorList"].value;
                    Gnoss.ApiWrapper.Model.TriplesToInclude t = new();
                    t.Predicate = "http://purl.org/ontology/bibo/authorList|http://www.w3.org/1999/02/22-rdf-syntax-ns#member";
                    t.NewValue = authorList+"|"+ "http://gnoss.com/items/Person_0feb1bbb-baa8-4735-a278-f5aae6ea26d9_8c18036f-348f-4e0b-a2ef-4d96c1db24a8";
                    result = mResourceApi.InsertPropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.TriplesToInclude>>()
                    {
                        {
                            mResourceApi.GetShortGuid(patent), new List<Gnoss.ApiWrapper.Model.TriplesToInclude>() { t }
                        }

                    });
                }
            }
            catch (Exception ex)
            {
                mResourceApi.Log.Error("Excepcion: " + ex.Message);
            }

            return Ok(result);
        }

    }
}
