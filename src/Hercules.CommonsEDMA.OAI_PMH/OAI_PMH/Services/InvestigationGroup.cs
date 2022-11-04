using Newtonsoft.Json;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.GruposInvestigacion;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAI_PMH.Services
{
    /// <summary>
    /// Clase principal del grupo de investigación.
    /// </summary>
    public class InvestigationGroup
    {
        /// <summary>
        /// Obtiene los IDs de los grupos modificads.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static Dictionary<string, DateTime> GetModifiedGrupos(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idListInvestigationGroup;
            RestClient clientInvestigationGroup = new(pConfig.GetConfigSGI() + "/api/sgicsp/grupos/modificados-ids?q=fechaModificacion=ge=\"" + from + "\""); // TODO: Revisar url petición.
            clientInvestigationGroup.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse responseInvestigationGroup = Token.httpCall(clientInvestigationGroup, request);

            if (!string.IsNullOrEmpty(responseInvestigationGroup.Content))
            {
                idListInvestigationGroup = responseInvestigationGroup.Content[1..^1].Split(',').ToList();
                foreach (string id in idListInvestigationGroup)
                {
                    string idMod = "Grupo_" + id.Replace("\"", "");
                    if (!idDictionary.ContainsKey(idMod))
                    {
                        idDictionary.Add(idMod, DateTime.UtcNow);
                    }
                }
            }
            return idDictionary;
        }

        /// <summary>
        /// Obtiene la información de un grupo mediante un ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static Grupo GetGrupos(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            string identifier = id.Split("_")[1];
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/grupos/" + identifier);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            Grupo grupo;
            List<GrupoEquipo> equipo;
            List<string> ingestigadoresPrincipales;
            List<string> investigadoresPrincipalesMaxParticipacion;
            List<GrupoPalabraClave> palabrasClave;
            List<LineaClasificacion> lineasClasificacion;
            List<LineaInvestigacion> lineasInvestigacion = new();

            grupo = JsonConvert.DeserializeObject<Grupo>(response.Content);

            equipo = GetGrupoEquipo(identifier, pConfig);

            ingestigadoresPrincipales = GetInvestigadoresPrincipales(identifier, pConfig);

            investigadoresPrincipalesMaxParticipacion = GetInvestigadoresPrincipalesMax(identifier, pConfig);

            palabrasClave = GetPalabrasClave(identifier, pConfig);

            lineasClasificacion = GetLineasClasificacion(identifier, pConfig);

            if (lineasClasificacion != null && lineasClasificacion.Any())
            {
                foreach (LineaClasificacion linea in lineasClasificacion)
                {
                    lineasInvestigacion.AddRange(GetLineasInvestigacion(linea.id.ToString(), pConfig));
                }
            }

            grupo.equipo = equipo;
            grupo.investigadoresPrincipales = ingestigadoresPrincipales;
            grupo.investigadoresPrincipalesMaxParticipacion = investigadoresPrincipalesMaxParticipacion;
            grupo.palabrasClave = palabrasClave;
            grupo.lineasClasificacion = lineasClasificacion;
            grupo.lineasInvestigacion = lineasInvestigacion;


            return grupo;
        }

        /// <summary>
        /// Obtiene las lineas de clasificación.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        private static List<LineaClasificacion> GetLineasClasificacion(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<LineaClasificacion> lineas;
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/gruposlineasinvestigacion/" + id + "/clasificaciones");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                lineas = JsonConvert.DeserializeObject<List<LineaClasificacion>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return lineas;
        }

        /// <summary>
        /// Obtiene las líneas de investigación.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        private static List<LineaInvestigacion> GetLineasInvestigacion(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<LineaInvestigacion> lineas;
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/grupos/" + id + "/lineasinvestigacion");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                lineas = JsonConvert.DeserializeObject<List<LineaInvestigacion>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return lineas;
        }

        /// <summary>
        /// Obtiene las palabras clave.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        private static List<GrupoPalabraClave> GetPalabrasClave(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<GrupoPalabraClave> palabras;
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/grupos/" + id + "/palabrasclave");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                palabras = JsonConvert.DeserializeObject<List<GrupoPalabraClave>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return palabras;
        }

        /// <summary>
        /// Obtiene los investigadores que más han participado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        private static List<string> GetInvestigadoresPrincipalesMax(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<string> investigadores;
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/grupos/" + id + "/investigadoresprincipalesmaxparticipacion");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                investigadores = JsonConvert.DeserializeObject<List<string>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return investigadores;
        }

        /// <summary>
        /// Obtiene los investigadores principales.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        private static List<string> GetInvestigadoresPrincipales(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<string> investigadores;
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/grupos/" + id + "/investigadoresprincipales");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                investigadores = JsonConvert.DeserializeObject<List<string>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return investigadores;
        }

        /// <summary>
        /// Obtiene los equipos de los grupos.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        private static List<GrupoEquipo> GetGrupoEquipo(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<GrupoEquipo> grupoEquipo;
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/grupos/" + id + "/miembrosequipo");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                grupoEquipo = JsonConvert.DeserializeObject<List<GrupoEquipo>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return grupoEquipo;
        }
    }
}
