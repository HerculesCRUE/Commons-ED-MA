using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Hercules.CommonsEDMA.ServicioExterno.Models.Cluster;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class SimilarityController : ControllerBase
    {
        readonly ConfigService _Configuracion;

        public SimilarityController(ConfigService pConfig)
        {
            _Configuracion = pConfig;
        }

        /// <summary>
        /// Obtiene los documentos similares
        /// </summary>
        /// <param name="pIdDocument">ID del documento</param>
        /// <returns></returns>
        [HttpGet("GetSimilaritiesDocument")]
        public IActionResult GetSimilaritiesDocument(string pIdDocument)
        {
            List<KeyValuePair<Guid, Dictionary<string, float>>> listID = new List<KeyValuePair<Guid, Dictionary<string, float>>>();
            try
            {
                AccionesSimilarity accionesSimilarity = new AccionesSimilarity();
                listID = accionesSimilarity.GetSimilarities(pIdDocument, _Configuracion, "research_paper");
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(listID);
        }

        /// <summary>
        /// Obtiene los research object similares.
        /// </summary>
        /// <param name="pIdRO">ID del researchobject</param>
        /// <returns></returns>
        [HttpGet("GetSimilaritiesResearchObject")]
        public IActionResult GetSimilaritiesResearchObject(string pIdRO)
        {
            List<KeyValuePair<Guid, Dictionary<string, float>>> listID = new List<KeyValuePair<Guid, Dictionary<string, float>>>();

            try
            {
                AccionesSimilarity accionesSimilarity = new AccionesSimilarity();
                listID = accionesSimilarity.GetSimilarities(pIdRO, _Configuracion, "code_project");
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(listID);
        }
    }
}
