using Gnoss.ApiWrapper.ApiModel;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesFuentesExternas : GnossGetMainResourceApiDataBase
    {


        /// <summary>
        /// Obtiene el ORCID del usuario.
        /// </summary>
        /// <param name="pUserId">ID del usuario conectado.</param>
        /// <returns></returns>
        public static string GetORCID(string pUserId)
        {
            string selectOrcid = mPrefijos;
            selectOrcid += "SELECT ?orcid ";
            string whereOrcid = $@"WHERE {{ 
                            ?s roh:gnossUser <http://gnoss/{pUserId.ToUpper()}> . 
                            OPTIONAL{{?s roh:ORCID ?orcid. }}
                        }} ";

            SparqlObject resultadoQueryOrcid = resourceApi.VirtuosoQuery(selectOrcid.ToString(), whereOrcid.ToString(), idComunidad);
            if (resultadoQueryOrcid != null && resultadoQueryOrcid.results != null && resultadoQueryOrcid.results.bindings != null && resultadoQueryOrcid.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryOrcid.results.bindings)
                {
                    if (fila.ContainsKey("orcid"))
                    {
                        return fila["orcid"].value;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Obtiene el IDGNOSS del usuario.
        /// </summary>
        /// <param name="pUserId">ID del usuario conectado.</param>
        /// <returns></returns>
        public static string GetIdGnoss(string pUserId)
        {
            string idBusqueda = "";

            string selectGnossUser = mPrefijos;
            selectGnossUser += "SELECT ?s ";
            string whereGnossUser = $@"WHERE {{
                                ?s roh:gnossUser <http://gnoss/{pUserId.ToUpper()}>. 
                            }} ";

            SparqlObject resultadoQueryGnossUser = resourceApi.VirtuosoQuery(selectGnossUser, whereGnossUser, idComunidad);
            if (resultadoQueryGnossUser != null && resultadoQueryGnossUser.results != null &&
                resultadoQueryGnossUser.results.bindings != null && resultadoQueryGnossUser.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryGnossUser.results.bindings)
                {
                    if (fila.ContainsKey("s"))
                    {
                        idBusqueda = fila["s"].value.ToLower().Replace("gnoss", "gnoss.com");
                    }
                }
            }

            if (!string.IsNullOrEmpty(idBusqueda))
            {
                string selectPersona = mPrefijos;
                selectPersona += "SELECT ?s ";
                string wherePersona = $@"WHERE {{ 
                                graph ?g{{ <{idBusqueda}> <http://gnoss/hasEntidad> ?s. }}
                                ?s ?p <http://xmlns.com/foaf/0.1/Person>.
                            }} ";

                SparqlObject resultadoQueryPersona = resourceApi.VirtuosoQuery(selectPersona, wherePersona, "person");
                if (resultadoQueryPersona != null && resultadoQueryPersona.results != null &&
                    resultadoQueryPersona.results.bindings != null && resultadoQueryPersona.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryPersona.results.bindings)
                    {
                        if (fila.ContainsKey("s"))
                        {
                            return fila["s"].value;
                        }
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Obtiene la fecha de la última actualización.
        /// </summary>
        /// <param name="pUserId">ID del usuario conectado.</param>
        /// <returns></returns>
        public static string GetLastUpdatedDate(string pUserId)
        {
            string selectLastUpd = mPrefijos;
            selectLastUpd += "SELECT ?fecha ";
            string whereLastUpd = $@"WHERE {{ 
                                ?s roh:gnossUser <http://gnoss/{pUserId.ToUpper()}>. 
                                OPTIONAL{{?s roh:lastUpdatedDate ?fecha. }}
                            }} ";

            SparqlObject resultadoQueryLastUpd = resourceApi.VirtuosoQuery(selectLastUpd, whereLastUpd, idComunidad);
            if (resultadoQueryLastUpd != null && resultadoQueryLastUpd.results != null && resultadoQueryLastUpd.results.bindings != null && resultadoQueryLastUpd.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryLastUpd.results.bindings)
                {
                    if (fila.ContainsKey("fecha"))
                    {
                        string fechaGnoss = fila["fecha"].value;
                        string anio = fechaGnoss.Substring(0, 4);
                        string mes = fechaGnoss.Substring(4, 2);
                        string dia = fechaGnoss.Substring(6, 2);

                        return $@"{anio}-{mes}-{dia}";
                    }
                }
            }

            return "1500-01-01";
        }

        /// <summary>
        /// Obtiene los IDs y Token del usuario para FigShare y GitHub.
        /// </summary>
        /// <param name="pUserId">ID del usuario.</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetUsersIDs(string pUserId)
        {
            Dictionary<string, string> dicResultados = new();

            string selectUserId = mPrefijos;
            selectUserId += "SELECT ?usuarioFigshare ?tokenFigshare ?usuarioGitHub ?tokenGitHub ";
            string whereUserId = $@"WHERE {{ 
                                ?s roh:gnossUser <http://gnoss/{pUserId.ToUpper()}>. 
                                OPTIONAL{{?s roh:usuarioFigShare ?usuarioFigshare. }}
                                OPTIONAL{{?s roh:tokenFigShare ?tokenFigshare. }}
                                OPTIONAL{{?s roh:usuarioGitHub ?usuarioGitHub. }} 
                                OPTIONAL{{?s roh:tokenGitHub ?tokenGitHub. }} 
                            }} ";

            SparqlObject resultadoQueryUserId = resourceApi.VirtuosoQuery(selectUserId, whereUserId, idComunidad);
            if (resultadoQueryUserId != null && resultadoQueryUserId.results != null && resultadoQueryUserId.results.bindings != null && resultadoQueryUserId.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryUserId.results.bindings)
                {
                    if (fila.ContainsKey("usuarioFigshare"))
                    {
                        dicResultados.Add("usuarioFigshare", fila["usuarioFigshare"].value);
                    }
                    if (fila.ContainsKey("tokenFigshare"))
                    {
                        dicResultados.Add("tokenFigshare", fila["tokenFigshare"].value);
                    }
                    if (fila.ContainsKey("usuarioGitHub"))
                    {
                        dicResultados.Add("usuarioGitHub", fila["usuarioGitHub"].value);
                    }
                    if (fila.ContainsKey("tokenGitHub"))
                    {
                        dicResultados.Add("tokenGitHub", fila["tokenGitHub"].value);
                    }
                }
            }

            return dicResultados;
        }
    }
}
