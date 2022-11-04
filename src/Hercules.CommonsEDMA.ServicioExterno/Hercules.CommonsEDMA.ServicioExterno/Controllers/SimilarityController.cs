using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
            AccionesSimilarity accionesSimilarity = new();
            List<KeyValuePair<Guid, Dictionary<string, float>>> listID = accionesSimilarity.GetSimilarities(pIdDocument, _Configuracion, "research_paper");

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
            AccionesSimilarity accionesSimilarity = new();
            List<KeyValuePair<Guid, Dictionary<string, float>>> listID = accionesSimilarity.GetSimilarities(pIdRO, _Configuracion, "code_project");

            return Ok(listID);
        }
    }
}
