using Newtonsoft.Json.Linq;
using OAI_PMH.Controllers;
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
        private static DateTime lastUpdate;
        private static ConfigService _ConfigService;

        public static string CheckToken(ConfigService pConfig, bool pTokenGestor = true, bool pTokenPii = false)
        {
            _ConfigService = pConfig;
            string token = GetToken(pConfig, pTokenGestor, pTokenPii);
            return token;
        }

        public static IRestResponse httpCall(RestClient pRestClient, RestRequest pRestRequest, bool pPermitirRespuestaVacia = false)
        {
            IRestResponse response = null;
            while (true)
            {
                try
                {
                    response = pRestClient.Execute(pRestRequest);
                    if (!pPermitirRespuestaVacia && string.IsNullOrEmpty(response.Content))
                    {
                        throw new Exception("La respuesta no debería estar vacía");
                    }
                    break;
                }
                catch
                {
                    return null;
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
            Uri url = new Uri(pConfig.GetConfigSGI() + "/auth/realms/sgi/protocol/openid-connect/token");
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

        private static string RefreshToken(ConfigService pConfig)
        {
            Uri url = new Uri(pConfig.GetConfigSGI() + "/auth/realms/sgi/protocol/openid-connect/token");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "front"),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });

            string result = httpCall(url.ToString(), "POST", content).Result;

            var json = JObject.Parse(result);
            accessToken = json["access_token"].ToString();

            return accessToken;
        }
    }
}