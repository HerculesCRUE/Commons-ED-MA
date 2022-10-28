using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Models.NotificationOntology;
using OAI_PMH.Models.SGI.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gnoss.ApiWrapper.ApiModel.SparqlObject;

namespace Utilidades
{
    class UtilidadesLoader
    {
        private static readonly ResourceApi mResourceApi = new ($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config");
                
        
        /// <summary>
        /// Envia una notificación.
        /// </summary>
        /// <param name="owner">Persona a la que enviar la notificación</param>
        /// <param name="rohType">Tipo de notificación</param>
        /// <param name="textoExtra">Texto extra de la notificación</param>
        public static void EnvioNotificacion(string owner, string rohType, string mensaje)
        {
            Notification notificacion = new ();
            notificacion.Roh_text = mensaje;
            notificacion.IdRoh_owner = owner;
            notificacion.Dct_issued = DateTime.UtcNow;
            notificacion.Roh_type = rohType;

            mResourceApi.ChangeOntoly("notification");
            ComplexOntologyResource recursoCargar = notificacion.ToGnossApiResource(mResourceApi);
            int numIntentos = 0;
            while (!recursoCargar.Uploaded)
            {
                numIntentos++;

                if (numIntentos > 5)
                {
                    break;
                }
                mResourceApi.LoadComplexSemanticResource(recursoCargar);
            }
        }

        public static void EnvioNotificacionesMiembros(List<string> listadoMiembros, string rohType, string mensaje)
        {
            try
            {
                if (listadoMiembros.Any())
                {
                    //Busco los miembros en BBDD (solo los investigadores)
                    string select = "SELECT ?person";
                    string where = $@"WHERE {{
                                    ?person <http://w3id.org/roh/isActive> 'true' .
                                    ?person <http://w3id.org/roh/crisIdentifier> ?crisID .
                                    FILTER(?crisID in ('{string.Join("','", listadoMiembros)}'))
                                }}";
                    SparqlObject resultData = mResourceApi.VirtuosoQuery(select, where, "person");
                    foreach (Dictionary<string, Data> fila in resultData.results.bindings)
                    {
                        if (fila.ContainsKey("person"))
                        {
                            //Notifico a los miembros
                            EnvioNotificacion(fila["person"].value, rohType, mensaje);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mResourceApi.Log.Error(ex.Message);
            }
        }

        public static List<string> ConseguirMiembrosPRC(string idRecurso)
        {
            List<string> listadoMiembros = new ();
            try
            {
                //Busco los miembros en BBDD (solo los que tengan CV)
                string select = "SELECT distinct ?person";
                string where = $@"where{{
                                ?s <http://purl.org/ontology/bibo/authorList> ?authorList .
                                ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person .
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                FILTER (?s=<{idRecurso}>)
                            }}";
                SparqlObject resultData = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "curriculumvitae", "person", "document" });
                foreach (Dictionary<string, Data> fila in resultData.results.bindings)
                {
                    if (fila.ContainsKey("person"))
                    {
                        listadoMiembros.Add(fila["person"].value);
                    }
                }
            }
            catch (Exception ex)
            {
                mResourceApi.Log.Error(ex.Message);
                return listadoMiembros; 
            }

            return listadoMiembros;
        }

    }
}
