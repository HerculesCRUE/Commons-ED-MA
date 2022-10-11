﻿using Newtonsoft.Json;
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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "invenciones/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "invenciones/" + identifier);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Invencion invencion = JsonConvert.DeserializeObject<Invencion>(response.Content);
            invencion.sectoresAplicacion = GetSectores(identifier, pConfig);
            invencion.invencionDocumentos = GetDocumentos(identifier, pConfig);
            invencion.gastos = GetGastos(identifier, pConfig);
            invencion.palabrasClave = GetPalabrasClaves(identifier, pConfig);
            invencion.areasConocimiento = GetAreasConocimiento(identifier, pConfig);
            invencion.inventores = GetInventores(identifier, pConfig);
            invencion.periodosTitularidad = GetPeriodosTitularidad(identifier, pConfig);
            if (invencion.periodosTitularidad != null && invencion.periodosTitularidad.Any())
            {
                invencion.titulares = new List<Titular>();
                foreach (PeriodoTitularidad periodo in invencion.periodosTitularidad)
                {
                    invencion.titulares.AddRange(GetTitular(periodo.id.ToString(), pConfig));
                }
            }
            invencion.solicitudes = GetSolicitudesProteccion(identifier, pConfig);
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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "invenciones/" + identifier + "/solicitudesproteccion");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "periodostitularidad/" + identifier + "/titulares");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "invenciones/" + identifier + "/periodostitularidad");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "invenciones/" + identifier + "/areasconocimiento");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "invenciones/" + identifier + "/sectoresaplicacion");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "invenciones/" + identifier + "/invenciondocumentos");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "invenciones/" + identifier + "/gastos");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "invenciones/" + identifier + "/palabrasclave");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseInvenciones() + "invenciones/" + identifier + "/invencion-inventores");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
