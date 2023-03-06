using Newtonsoft.Json;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.Organization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAI_PMH.Services
{
    class Organization
    {
        public static Dictionary<string, DateTime> GetModifiedOrganizations(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList;
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgemp/empresas/modificadas-ids?q=fechaModificacion=ge=\"" + from + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            if (!String.IsNullOrEmpty(response.Content))
            {
                var sinComillas = response.Content[1..^1].Replace("\"", "");

                idList = sinComillas.Split(',').ToList();
                foreach (string id in idList)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        idDictionary.Add($"Organizacion_{id}", DateTime.UtcNow);
                    }
                }
            }
            return idDictionary;
        }

        public static Empresa GetEmpresa(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            string identifier = id.Split('_')[1];
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgemp/empresas/" + identifier);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            if (string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            Empresa empresa = JsonConvert.DeserializeObject<Empresa>(response.Content);
            empresa.DatosContacto = GetDatosContacto(identifier, pConfig);
            return empresa;
        }

        private static DatosContacto GetDatosContacto(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            DatosContacto datosContacto;
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgemp/datos-contacto/empresa/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            string datosLimpios = response.Content;
            try
            {
                datosContacto = JsonConvert.DeserializeObject<DatosContacto>(datosLimpios);
            }
            catch (Exception)
            {
                return null;
            }
            return datosContacto;
        }
    }
}
