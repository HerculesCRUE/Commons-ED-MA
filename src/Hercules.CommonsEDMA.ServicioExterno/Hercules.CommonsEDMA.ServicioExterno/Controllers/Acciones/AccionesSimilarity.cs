using System;
using System.Collections.Generic;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Microsoft.AspNetCore.Cors;
using Hercules.CommonsEDMA.ServicioExterno.Models.Similarity;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    [EnableCors("_myAllowSpecificOrigins")]
    public class AccionesSimilarity : GnossGetMainResourceApiDataBase
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId">ID del elemento al que buscar similares</param>
        /// <param name="pConfig">Config</param>
        /// <param name="pType">Tipo: 'research_paper' o 'code_project'</param>
        /// <returns></returns>
        public List<KeyValuePair<Guid, Dictionary<string, float>>> GetSimilarities(string pId, ConfigService pConfig, string pType)
        {



            if (!string.IsNullOrEmpty(pConfig.GetUrlSimilarity()))
            {
                UtilsSimilarity utilsSimilarity = new(pConfig.GetUrlSimilarity(), resourceApi, pType);
                return utilsSimilarity.GetSimilars(pId);
            }
            else
            {
                return new List<KeyValuePair<Guid, Dictionary<string, float>>>();
            }
        }
    }
}
