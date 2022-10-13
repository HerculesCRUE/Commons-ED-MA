using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            List<string> idList = new();
            RestClient client = new(pConfig.GetUrlBaseFormacionAcademica() + "formacion/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"" + ";tipoFormacion=\"020.010.010.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (!string.IsNullOrEmpty(response.Content))
            {
                idList = response.Content[1..^1].Split(',').ToList();
                foreach (string id in idList)
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
            RestClient client = new(pConfig.GetUrlBaseFormacionAcademica() + "formacion/" + identifier + "?tipoFormacion=\"020.010.010.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                return JsonConvert.DeserializeObject<List<Ciclos>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Dictionary<string, DateTime> GetModifiedDoctorados(string from, ConfigService pConfig, string accessToken)
        {
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetUrlBaseFormacionAcademica() + "formacion/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"" + ";tipoFormacion=\"020.010.020.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (!String.IsNullOrEmpty(response.Content))
            {
                idList = response.Content[1..^1].Split(',').ToList();
                foreach (string id in idList)
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
            RestClient client = new(pConfig.GetUrlBaseFormacionAcademica() + "formacion/" + identifier + "?tipoFormacion=\"020.010.020.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                return JsonConvert.DeserializeObject<List<Doctorados>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Dictionary<string, DateTime> GetModifiedPosgrado(string from, ConfigService pConfig, string accessToken)
        {
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetUrlBaseFormacionAcademica() + "formacion/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"" + ";tipoFormacion=\"020.010.030.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (!String.IsNullOrEmpty(response.Content))
            {
                idList = response.Content[1..^1].Split(',').ToList();
                foreach (string id in idList)
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
            RestClient client = new(pConfig.GetUrlBaseFormacionAcademica() + "formacion/" + identifier + "?tipoFormacion=\"020.010.030.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                return JsonConvert.DeserializeObject<List<Posgrado>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Dictionary<string, DateTime> GetModifiedEspecializada(string from, ConfigService pConfig, string accessToken)
        {
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetUrlBaseFormacionAcademica() + "formacion/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"" + ";tipoFormacion=\"020.020.000.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (!String.IsNullOrEmpty(response.Content))
            {
                idList = response.Content[1..^1].Split(',').ToList();
                foreach (string id in idList)
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
            RestClient client = new(pConfig.GetUrlBaseFormacionAcademica() + "formacion/" + identifier + "?tipoFormacion=\"020.020.000.000\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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