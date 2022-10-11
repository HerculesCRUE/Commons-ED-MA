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
    public class AccionesSimilarity
    {
        #region --- Constantes   
        private static string RUTA_OAUTH = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config";
        private static ResourceApi mResourceAPI = null;
        #endregion

        private static ResourceApi resourceApi
        {
            get
            {
                while (mResourceAPI == null)
                {
                    try
                    {
                        mResourceAPI = new ResourceApi(RUTA_OAUTH);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No se ha podido iniciar ResourceApi");
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mResourceAPI;
            }
        }

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
                UtilsSimilarity utilsSimilarity = new UtilsSimilarity(pConfig.GetUrlSimilarity(), resourceApi, pType);
                return utilsSimilarity.GetSimilars(pId);
            }
            else
            {
                return new List<KeyValuePair<Guid, Dictionary<string, float>>>();
            }
        }
    }
}
