﻿using OAI_PMH.Controllers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAI_PMH.Services
{
    public class OrganicStructure
    {
        public static Dictionary<string, DateTime> GetAreasConocimiento(string parentId, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList;
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgo/empresas/modificadas-ids?q=padreId=na=\"" + parentId + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            idList = response.Content[1..^1].Split(',').ToList();

            foreach (string id in idList)
            {
                idDictionary.Add("Organizacion_" + id, DateTime.UtcNow);
            }
            return idDictionary;
        }
    }
}
