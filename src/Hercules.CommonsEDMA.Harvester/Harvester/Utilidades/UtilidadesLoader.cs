using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Models.NotificationOntology;
using OAI_PMH.Models.SGI.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gnoss.ApiWrapper.ApiModel.SparqlObject;

namespace Utilidades
{
    class UtilidadesLoader
    {
        private static readonly ResourceApi mResourceApi = new ResourceApi($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config/ConfigOAuth/OAuthV3.config");

        /// <summary>
        /// Crea las entidades gestoras, convocantes y financiadoras auxiliares de <paramref name="proyecto"/>.
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="dicProyectos"></param>
        /// <param name="dicPersonas"></param>
        /// <param name="dicOrganizaciones"></param>
        public static void CreacionAuxiliaresProyecto(Proyecto proyecto, Dictionary<string, Tuple<string, string>> dicProyectos,
            Dictionary<string, Tuple<string, string>> dicPersonas, Dictionary<string, Tuple<string, string>> dicOrganizaciones,
            ResourceApi mResourceApi)
        {
            List<string> listaOrganizaciones = new List<string>();
            //TODO
            //listaOrganizaciones.AddRange(proyecto.EntidadesGestoras.Select(x => x.EntidadRef).ToList());
            //listaOrganizaciones.AddRange(proyecto.EntidadesConvocantes.Select(x => x.EntidadRef).ToList());
            listaOrganizaciones.AddRange(proyecto.EntidadesFinanciadoras.Select(x => x.EntidadRef).ToList());

            dicProyectos = GetEntityByCRIS("http://vivoweb.org/ontology/core#Project", "project", new List<string>() { proyecto.Id.ToString() }, mResourceApi);
            dicPersonas = GetEntityByCRIS("http://xmlns.com/foaf/0.1/Person", "person", proyecto.Equipo.Select(x => x.PersonaRef).ToList(), mResourceApi);
            dicOrganizaciones = GetEntityByCRIS("http://xmlns.com/foaf/0.1/Organization", "organization", listaOrganizaciones, mResourceApi);
        }

        /// <summary>
        /// Obtiene el ID del recurso junto a su CrisIdentifier.
        /// </summary>
        /// <param name="pRdfType"></param>
        /// <param name="pOntology"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetEntityBBDD(string pRdfType, string pOntology, ResourceApi mResourceApi)
        {
            Dictionary<string, string> dicResultados = new Dictionary<string, string>();

            int limit = 10000;
            int offset = 0;
            while (true)
            {
                SparqlObject resultadoQuery = null;
                StringBuilder select = new StringBuilder(), where = new StringBuilder();

                // Consulta sparql.
                select.Append("SELECT ?s ?crisIdentifier ");
                where.Append("WHERE { ");
                where.Append($@"?s a <{pRdfType}>. ");
                where.Append("?s <http://w3id.org/roh/crisIdentifier> ?crisIdentifier. ");
                where.Append("} ");

                resultadoQuery = mResourceApi.VirtuosoQuery(select.ToString(), where.ToString(), pOntology);

                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    offset += limit;

                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        string id = fila["crisIdentifier"].value;
                        string nombre = fila["s"].value;
                        dicResultados.Add(id, nombre);
                    }

                    if (resultadoQuery.results.bindings.Count < limit)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return dicResultados;
        }

        /// <summary>
        /// Obtiene el ID del recurso junto a su CrisIdentifier.
        /// </summary>
        /// <param name="pRdfType"></param>
        /// <param name="pOntology"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        private static Dictionary<string, Tuple<string, string>> GetEntityByCRIS(string pRdfType, string pOntology, List<string> lista, ResourceApi mResourceApi)
        {
            Dictionary<string, Tuple<string, string>> dicResultados = new Dictionary<string, Tuple<string, string>>();

            int limit = 10000;
            int offset = 0;
            while (true)
            {
                string select = "SELECT DISTINCT ?s ?crisIdentifier ?nombrePersona";
                string filtroProject = $@"strends(?crisIdentifier, '|{string.Join("') OR  strends(?crisIdentifier , '|", lista)}')";//TODO - eliminar por el general
                string filtroGeneral = $@"?crisIdentifier in ('{string.Join("', '", lista)}') ";

                string filtro = pOntology.Equals("project") ? filtroProject : filtroGeneral;
                string where = $@"WHERE {{
    ?s a <{pRdfType}> .
    ?s <http://w3id.org/roh/crisIdentifier> ?crisIdentifier . 
    OPTIONAL{{ ?s <http://xmlns.com/foaf/0.1/name> ?nombrePersona }}
    FILTER(
        {filtro}
    )
}} ";

                SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, pOntology);

                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    offset += limit;

                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        string crisId = fila["crisIdentifier"].value;
                        string identificador = fila["s"].value;
                        string nombrePersona = "";
                        if (fila.ContainsKey("nombrePersona"))
                        {
                            nombrePersona = fila["nombrePersona"].value;
                        }
                        dicResultados.Add(crisId, new Tuple<string, string>(identificador, nombrePersona));
                    }

                    if (resultadoQuery.results.bindings.Count < limit)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

            }

            return dicResultados;
        }

        /// <summary>
        /// Envia una notificación.
        /// </summary>
        /// <param name="owner">Persona a la que enviar la notificación</param>
        /// <param name="rohType">Tipo de notificación</param>
        /// <param name="textoExtra">Texto extra de la notificación</param>
        public static void EnvioNotificacion(string owner, string rohType, string mensaje)
        {
            Notification notificacion = new Notification();
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
                    //Busco los miembros en BBDD (solo los que tengan CV)
                    string select = "SELECT ?person";
                    string where = $@"WHERE {{
                                    ?cv <http://w3id.org/roh/cvOf> ?person .
                                    ?person <http://w3id.org/roh/crisIdentifier> ?crisID .
                                    FILTER(?crisID in ('{string.Join("','", listadoMiembros)}'))
                                }}";
                    SparqlObject resultData = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "curriculumvitae", "person" });
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
            catch (Exception)
            {

            }
        }

        public static List<string> ConseguirMiembrosPRC(string idRecurso)
        {
            List<string> listadoMiembros = new List<string>();
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
            catch (Exception) { 
                return listadoMiembros; 
            }

            return listadoMiembros;
        }

    }
}
