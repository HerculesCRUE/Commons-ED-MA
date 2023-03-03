using Newtonsoft.Json;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.FormacionAcademica;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAI_PMH.Services
{
    public class AcademicFormation
    {
        public static Dictionary<string, DateTime> GetModifiedCiclos(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idListCiclos;
            RestClient clientCiclos = new(pConfig.GetConfigSGI() + "/api/sgp/formacion/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"" + ";tipoFormacion=\"020.010.010.000\"");            
            clientCiclos.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse responseCiclos = Token.httpCall(clientCiclos, request);

            if (!string.IsNullOrEmpty(responseCiclos.Content))
            {
                idListCiclos = responseCiclos.Content[1..^1].Split(',').ToList();
                foreach (string id in idListCiclos)
                {
                    string idMod = "FormacionAcademica-Ciclos_" + id.Replace("\"", "");
                    if (!idDictionary.ContainsKey(idMod))
                    {
                        idDictionary.Add(idMod, DateTime.UtcNow);
                    }
                }
            }
            return idDictionary;
        }
        public static List<Ciclos> GetFormacionAcademicaCiclos(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/formacion/" + identifier + "?tipoFormacion=\"020.010.010.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<Ciclos>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Dictionary<string, DateTime> GetModifiedDoctorados(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idListDoctorados;
            RestClient clientDoctorados = new(pConfig.GetConfigSGI() + "/api/sgp/formacion/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"" + ";tipoFormacion=\"020.010.020.000\"");
            clientDoctorados.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse responseDoctorados = Token.httpCall(clientDoctorados, request);

            if (!string.IsNullOrEmpty(responseDoctorados.Content))
            {
                idListDoctorados = responseDoctorados.Content[1..^1].Split(',').ToList();
                foreach (string id in idListDoctorados)
                {
                    string idCompleto = "FormacionAcademica-Doctorados_" + id.Replace("\"", "");
                    if (!idDictionary.ContainsKey(idCompleto))
                    {
                        idDictionary.Add(idCompleto, DateTime.UtcNow);
                    }
                }
            }
            return idDictionary;
        }
        public static List<Doctorados> GetFormacionAcademicaDoctorados(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/formacion/" + identifier + "?tipoFormacion=\"020.010.020.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<Doctorados>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Dictionary<string, DateTime> GetModifiedPosgrado(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idListPosgrado;
            RestClient clientPosgrado = new(pConfig.GetConfigSGI() + "/api/sgp/formacion/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"" + ";tipoFormacion=\"020.010.030.000\"");
            clientPosgrado.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse responsePosgrado = Token.httpCall(clientPosgrado, request);

            if (!String.IsNullOrEmpty(responsePosgrado.Content))
            {
                idListPosgrado = responsePosgrado.Content[1..^1].Split(',').ToList();
                foreach (string id in idListPosgrado)
                {
                    string idMod = "FormacionAcademica-Posgrado_" + id.Replace("\"", "");
                    if (!idDictionary.ContainsKey(idMod))
                    {
                        idDictionary.Add(idMod, DateTime.UtcNow);
                    }
                }
            }
            return idDictionary;
        }
        public static List<Posgrado> GetFormacionAcademicaPosgrado(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/formacion/" + identifier + "?tipoFormacion=\"020.010.030.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<Posgrado>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Dictionary<string, DateTime> GetModifiedEspecializada(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idListEspecializada;
            RestClient clientEspecializada = new(pConfig.GetConfigSGI() + "/api/sgp/formacion/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"" + ";tipoFormacion=\"020.020.000.000\"");
            clientEspecializada.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse responseEspecializada = Token.httpCall(clientEspecializada, request);

            if (!String.IsNullOrEmpty(responseEspecializada.Content))
            {
                idListEspecializada = responseEspecializada.Content[1..^1].Split(',').ToList();
                foreach (string id in idListEspecializada)
                {
                    string idMod = "FormacionAcademica-Especializada_" + id.Replace("\"", "");
                    if (!idDictionary.ContainsKey(idMod))
                    {
                        idDictionary.Add(idMod, DateTime.UtcNow);
                    }
                }
            }
            return idDictionary;
        }
        public static List<FormacionEspecializada> GetFormacionAcademicaEspecializada(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            string identifier = id.Replace("\"", "");
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/formacion/" + identifier + "?tipoFormacion=\"020.020.000.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                return JsonConvert.DeserializeObject<List<FormacionEspecializada>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}