using Newtonsoft.Json.Linq;
using OAI_PMH.Controllers;
using OAI_PMH.Middlewares;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OAI_PMH.Services
{
    class Token
    {
        private static string accessToken;
        private static string refreshToken;
        private static ConfigService _ConfigService;

        // Logs.
        private static FileLogger _FileLogger;

        public static string CheckToken(ConfigService pConfig, bool pTokenGestor = true, bool pTokenPii = false)
        {
            _ConfigService = pConfig;
            _FileLogger = new FileLogger(_ConfigService);
            string token = GetToken(pConfig, pTokenGestor, pTokenPii);
            return token;
        }

        public static IRestResponse httpCall(RestClient pRestClient, RestRequest pRestRequest)
        {
            pRestClient.Timeout = 3600000;
            IRestResponse response;
            while (true)
            {
                DateTime inicio = DateTime.Now;

                try
                {
                    response = pRestClient.Execute(pRestRequest);
                    DateTime fin = DateTime.Now;                    
                    if (response.StatusCode !=System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.NoContent && response.StatusCode != System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("La respuesta ha sido: " + response.StatusCode);
                    }
                    _FileLogger.Log(inicio, fin, pRestClient.BaseUrl.ToString(), "DEBUG");
                    break;
                }
                catch(Exception ex)
                {
                    DateTime fin = DateTime.Now;
                    _FileLogger.Log(inicio, fin, pRestClient.BaseUrl.ToString(), "ERROR "+ex.Message);
                    throw;
                }
            }

            return response;
        }

        protected static async Task<string> httpCall(string pUrl, string pMethod, FormUrlEncodedContent pBody)
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(pMethod), pUrl))
                {
                    request.Content = pBody;

                    int intentos = 3;
                    while (true)
                    {
                        try
                        {
                            response = await httpClient.SendAsync(request);
                            break;
                        }
                        catch
                        {
                            //intentos--;
                            if (intentos == 0)
                            {
                                throw;
                            }
                            else
                            {
                                Thread.Sleep(60000);
                            }
                        }
                    }
                }
            }
            if (response.Content != null)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetToken(ConfigService pConfig, bool pTokenGestor, bool pTokenPii)
        {
            Uri url = new(pConfig.GetConfigSGI() + "/auth/realms/sgi/protocol/openid-connect/token");
            FormUrlEncodedContent content = null;
            if (pTokenGestor)
            {
                content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("client_id", "front"),
                new KeyValuePair<string, string>("username", _ConfigService.GetUsernameToken()),
                new KeyValuePair<string, string>("password", _ConfigService.GetPasswordToken()),
                new KeyValuePair<string, string>("grant_type", "password")
            });
            }
            else
            {
                content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("client_id", "front"),
                new KeyValuePair<string, string>("username", _ConfigService.GetUsernamePIIToken()),
                new KeyValuePair<string, string>("password", _ConfigService.GetPasswordPIIToken()),
                new KeyValuePair<string, string>("grant_type", "password")
            });
            }

            string result = httpCall(url.ToString(), "POST", content).Result;

            var json = JObject.Parse(result);
            accessToken = json["access_token"].ToString();
            refreshToken = json["refresh_token"].ToString();

            return accessToken;
        }
    }
}