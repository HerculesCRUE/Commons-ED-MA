using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Hercules.CommonsEDMA.ServicioExterno.Models.Cluster;
using ClusterOntology;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hercules.CommonsEDMA.ServicioExterno.Models.Cluster.Cluster;
using Hercules.CommonsEDMA.ServicioExterno.Models;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataItemRelacion;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags;
using Microsoft.AspNetCore.Cors;
using System.Net.Http;
using Hercules.CommonsEDMA.ServicioExterno.Models.Similarity;
using System.Threading;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    [EnableCors("_myAllowSpecificOrigins")]
    public class AccionesSimilarity: GnossGetMainResourceApiDataBase
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
