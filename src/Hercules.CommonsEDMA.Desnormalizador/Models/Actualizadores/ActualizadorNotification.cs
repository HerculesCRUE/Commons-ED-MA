using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gnoss.ApiWrapper.ApiModel.SparqlObject;

namespace Hercules.CommonsEDMA.Desnormalizador.Models.Actualizadores
{
    class ActualizadorNotification : ActualizadorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pResourceApi">API Wrapper de GNOSS</param>
        public ActualizadorNotification(ResourceApi pResourceApi) : base(pResourceApi)
        {
        }

        public void ActualizarNotificaciones()
        {
            int limit = 500;
            //Fecha en formato GNOSS de hace 7 días.
            string dateNow = DateTime.Now.AddDays(-7).ToString("yyyyMMdd");
            dateNow += "000000";
            while (true)
            {
                //Consulta.
                string select = "SELECT distinct ?s ";
                string where = $@"WHERE {{
						                ?s a <http://w3id.org/roh/Notification> . 
						                ?s <http://purl.org/dc/terms/issued> ?o . 
                                        FILTER(xsd:long(?o) < {dateNow} )
					                }}limit {limit}";

                SparqlObject resultData = mResourceApi.VirtuosoQuery(select, where, "notification");

                Parallel.ForEach(resultData.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = 5 }, fila =>
                {

                    //Eliminar el recurso
                    try
                    {
                        mResourceApi.PersistentDelete(mResourceApi.GetShortGuid(fila["s"].value));

                    }
                    catch (Exception e)
                    {
                        mResourceApi.Log.Error(e.Message);
                    }
                });

                if (resultData.results.bindings.Count < limit)
                {
                    break;
                }
            }
        }
    }
}
