using Newtonsoft.Json;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAI_PMH.Services
{
    /// <summary>
    /// Clase principal de Invention.
    /// </summary>
    public class Invention
    {
        /// <summary>
        /// Obtiene los IDs modificados de las invenciones.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static Dictionary<string, DateTime> GetModifiedInvenciones(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/invenciones/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request,true);

            if (!string.IsNullOrEmpty(response.Content))
            {
                idList = response.Content[1..^1].Split(',').ToList();
                foreach (string id in idList)
                {
                    string idMod = "Invencion_" + id.Replace("\"", "");
                    if (!idDictionary.ContainsKey(idMod))
                    {
                        idDictionary.Add(idMod, DateTime.UtcNow);
                    }
                }
            }
            return idDictionary;
        }

        /// <summary>
        /// Obtiene los datos de las invenciones mediante un ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static Invencion GetInvenciones(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            string identifier = id.Replace("\"", "").Split('_')[1];
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/invenciones/" + identifier);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);           
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            if (string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            Invencion invencion = JsonConvert.DeserializeObject<Invencion>(response.Content);
            List<SectorAplicacion> sectorAplicacion =  GetSectores(identifier, pConfig);
            List<InvencionDocumento> invencionDocumento = GetDocumentos(identifier, pConfig);
            List<InvencionGastos> gastos = GetGastos(identifier, pConfig);
            List<PalabraClave> palabrasClave = GetPalabrasClaves(identifier, pConfig);
            List<AreaConocimiento> areasConocimiento = GetAreasConocimiento(identifier, pConfig);
            List<PeriodoTitularidad> periodosTitularidad = GetPeriodosTitularidad(identifier, pConfig);
            List<Titular> titulares = new List<Titular>();
            if (periodosTitularidad != null && periodosTitularidad.Any())
            {
                foreach (PeriodoTitularidad periodo in periodosTitularidad)
                {
                    titulares.AddRange(GetTitular(periodo.id.ToString(), pConfig));
                }
            }
            List<SolicitudProteccion> solicitudes = GetSolicitudesProteccion(identifier, pConfig);
            invencion.sectoresAplicacion = sectorAplicacion;
            invencion.invencionDocumentos = invencionDocumento;
            invencion.gastos = gastos;
            invencion.palabrasClave = palabrasClave;
            invencion.areasConocimiento = areasConocimiento;
            invencion.periodosTitularidad = periodosTitularidad;
            invencion.titulares = titulares;
            invencion.solicitudes = solicitudes;
            return invencion;
        }

        /// <summary>
        /// Obtiene las solicitudes de protección.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static List<SolicitudProteccion> GetSolicitudesProteccion(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/invenciones/" + identifier + "/solicitudesproteccion");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<SolicitudProteccion>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene los periodos de titularidad.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static List<Titular> GetTitular(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/periodostitularidad/" + identifier + "/titulares");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<Titular>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene los periodos de titularidad.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static List<PeriodoTitularidad> GetPeriodosTitularidad(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/invenciones/" + identifier + "/periodostitularidad");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<PeriodoTitularidad>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene las áreas de conocimiento.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static List<AreaConocimiento> GetAreasConocimiento(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/invenciones/" + identifier + "/areasconocimiento");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<AreaConocimiento>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene los sectores.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static List<SectorAplicacion> GetSectores(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/invenciones/" + identifier + "/sectoresaplicacion");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<SectorAplicacion>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene los documentos.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static List<InvencionDocumento> GetDocumentos(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/invenciones/" + identifier + "/invenciondocumentos");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<InvencionDocumento>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene los gastos.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static List<InvencionGastos> GetGastos(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/invenciones/" + identifier + "/gastos");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<InvencionGastos>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene las palabras clave.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static List<PalabraClave> GetPalabrasClaves(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/invenciones/" + identifier + "/palabrasclave");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<PalabraClave>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene los inventores.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static List<Inventor> GetInventores(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig, pTokenGestor: false, pTokenPii: true);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgipii/invenciones/" + identifier + "/invencion-inventores");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<Inventor>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
