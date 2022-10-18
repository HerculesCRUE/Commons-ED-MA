﻿using Newtonsoft.Json;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.Autorizacion;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAI_PMH.Services
{
    /// <summary>
    /// Autorizaciones de proyectos.
    /// </summary>
    public class Autorizaciones
    {
        /// <summary>
        /// Obtiene los IDs de las autorizaciones de proyectos modificadas en una determinada fecha
        /// </summary>
        /// <param name="from"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static Dictionary<string, DateTime> GetModifiedAutorizaciones(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/autorizaciones/modificadas-ids?q=fechaModificacion=ge=\"" + from + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (!string.IsNullOrEmpty(response.Content))
            {
                idList = response.Content[1..^1].Split(',').ToList();
                foreach (string id in idList)
                {
                    idDictionary.Add("AutorizacionProyecto_" + id, DateTime.UtcNow);
                }
            }
            return idDictionary;
        }

        /// <summary>
        /// Obtiene las autorizaciones.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public static Autorizacion GetAutorizacion(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            string identifier = id.Split("_")[1];
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/autorizaciones/" + identifier);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                return JsonConvert.DeserializeObject<Autorizacion>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}