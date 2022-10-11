using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesFuentesExternas
    {
        #region --- Constantes   
        private static string RUTA_OAUTH = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config";
        private static ResourceApi mResourceAPI = null;
        private static CommunityApi mCommunityAPI = null;
        private static Guid? mIDComunidad = null;
        private static string RUTA_PREFIJOS = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Models/JSON/prefijos.json";
        private static string mPrefijos = string.Join(" ", JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(RUTA_PREFIJOS)));
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

        private static CommunityApi communityApi
        {
            get
            {
                while (mCommunityAPI == null)
                {
                    try
                    {
                        mCommunityAPI = new CommunityApi(RUTA_OAUTH);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No se ha podido iniciar CommunityApi");
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mCommunityAPI;
            }
        }

        private static Guid idComunidad
        {
            get
            {
                while (!mIDComunidad.HasValue)
                {
                    try
                    {
                        mIDComunidad = communityApi.GetCommunityId();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No se ha podido obtener el ID de la comnunidad");
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mIDComunidad.Value;
            }
        }

        /// <summary>
        /// Obtiene el ORCID del usuario.
        /// </summary>
        /// <param name="pUserId">ID del usuario conectado.</param>
        /// <returns></returns>
        public static string GetORCID(string pUserId)
        {
            SparqlObject resultadoQuery = null;
            StringBuilder select = new StringBuilder(), where = new StringBuilder();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("SELECT ?orcid ");
            where.Append("WHERE { ");
            where.Append($@"?s roh:gnossUser <http://gnoss/{pUserId.ToUpper()}>. ");
            where.Append("OPTIONAL{?s roh:ORCID ?orcid. } ");
            where.Append("} ");

            resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), idComunidad);
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
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
            string idBusqueda = string.Empty;
            SparqlObject resultadoQuery = null;
            StringBuilder select = new StringBuilder(), where = new StringBuilder();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("SELECT ?s ");
            where.Append("WHERE { ");
            where.Append($@"?s roh:gnossUser <http://gnoss/{pUserId.ToUpper()}>. ");
            where.Append("} ");

            resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), idComunidad);
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    if (fila.ContainsKey("s"))
                    {
                        idBusqueda = fila["s"].value.ToLower().Replace("gnoss", "gnoss.com");
                    }
                }
            }

            if(!string.IsNullOrEmpty(idBusqueda))
            {
                resultadoQuery = null;
                select = new StringBuilder();
                where = new StringBuilder();

                // Consulta sparql.
                select.Append(mPrefijos);
                select.Append("SELECT ?s ");
                where.Append("WHERE { ");
                where.Append($@"graph ?g{{ <{idBusqueda}> <http://gnoss/hasEntidad> ?s. }}");
                where.Append($@"?s ?p <http://xmlns.com/foaf/0.1/Person>. ");
                where.Append("} ");

                resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), "person");
                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
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
            SparqlObject resultadoQuery = null;
            StringBuilder select = new StringBuilder(), where = new StringBuilder();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("SELECT ?fecha ");
            where.Append("WHERE { ");
            where.Append($@"?s roh:gnossUser <http://gnoss/{pUserId.ToUpper()}>. ");
            where.Append("OPTIONAL{?s roh:lastUpdatedDate ?fecha. } ");
            where.Append("} ");

            resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), idComunidad);
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
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
            Dictionary<string, string> dicResultados = new Dictionary<string, string>();
            SparqlObject resultadoQuery = null;
            StringBuilder select = new StringBuilder(), where = new StringBuilder();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("SELECT ?usuarioFigshare ?tokenFigshare ?usuarioGitHub ?tokenGitHub ");
            where.Append("WHERE { ");
            where.Append($@"?s roh:gnossUser <http://gnoss/{pUserId.ToUpper()}>. ");
            where.Append("OPTIONAL{?s roh:usuarioFigShare ?usuarioFigshare. } ");
            where.Append("OPTIONAL{?s roh:tokenFigShare ?tokenFigshare. } ");
            where.Append("OPTIONAL{?s roh:usuarioGitHub ?usuarioGitHub. } ");
            where.Append("OPTIONAL{?s roh:tokenGitHub ?tokenGitHub. } ");
            where.Append("} ");

            resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), idComunidad);
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
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
