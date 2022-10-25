using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.ProduccionCientifica;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using OAI_PMH.Models.SGI.PRC;

namespace OAI_PMH.Services
{
    public class PRC
    {
        public static Dictionary<string, DateTime> GetModifiedPRC(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgiprc/producciones-cientificas/estado?q=fechaEstado=ge=\"" + from + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request, true);
            if (!string.IsNullOrEmpty(response.Content))
            {
                List<ProduccionCientificaEstado> listaValidaciones = JsonConvert.DeserializeObject<List<ProduccionCientificaEstado>>(response.Content);
                foreach(ProduccionCientificaEstado item in listaValidaciones)
                {
                    idList.Add(item.idRef);
                }
                                
                foreach (string id in idList)
                {
                    idDictionary.Add("PRC_" + id, DateTime.UtcNow);
                }
            }
            return idDictionary;
        }

        public static List<ProduccionCientificaEstado> GetPRC(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgiprc/producciones-cientificas/estado?q=fechaEstado=ge=\"" + from + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            return JsonConvert.DeserializeObject<List<ProduccionCientificaEstado>>(response.Content);
        }
    }
}
