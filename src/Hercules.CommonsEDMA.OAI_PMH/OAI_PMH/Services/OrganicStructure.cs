using OAI_PMH.Controllers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Services
{
    public class OrganicStructure
    {
        public static Dictionary<string, DateTime> GetAreasConocimiento(string parentId, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetUrlBaseEstructuraOrganica() + "empresas/modificadas-ids?q=padreId=na=\"" + parentId + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            idList = response.Content[1..^1].Split(',').ToList();

            foreach (string id in idList)
            {
                idDictionary.Add("Organizacion_" + id, DateTime.UtcNow);
            }
            return idDictionary;
        }
    }
}
