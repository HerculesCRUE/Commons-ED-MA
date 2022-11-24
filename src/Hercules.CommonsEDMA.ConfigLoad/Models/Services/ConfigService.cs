using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.IO;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.Services
{
    public class ConfigService
    {
        private IConfiguration _configuration { get; set; }
        private string nombreCortoComunidad = "hercules";
        private string loginAdmin = "admin";
        private string passAdmin { get; set; }
        private string urlAPIDespliegues { get; set; }

        private string urlDominioServicios { get; set; }
        private string sqlOauthConnectionString { get; set; }
        private string sqlAcidConnectionString { get; set; }
        private string tipoBD { get; set; }
        private string APIConsumerKey { get; set; }
        private string APIConsumerSecret { get; set; }
        private string APITokenKey { get; set; }
        private string APITokenSecret { get; set; }
        private string urlPropia { get; set; }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigService()
        {
            _configuration = GetBuildConfiguration();
        }

        /// <summary>
        /// Contruye el objeto de lectura con la configuración del JSON.
        /// </summary>
        /// <returns></returns>
        public static IConfiguration GetBuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        public string ObtenerUrlAPIDespliegues()
        {
            if (string.IsNullOrEmpty(urlAPIDespliegues))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("urlAPIDespliegues"))
                {
                    urlAPIDespliegues = environmentVariables["urlAPIDespliegues"] as string;
                }
                else
                {
                    urlAPIDespliegues = _configuration["urlAPIDespliegues"];
                }
            }
            return urlAPIDespliegues;
        }

        public string ObtenerUrlDominioServicios()
        {
            if (string.IsNullOrEmpty(urlDominioServicios))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("urlDominioServicios"))
                {
                    urlDominioServicios = environmentVariables["urlDominioServicios"] as string;
                }
                else
                {
                    urlDominioServicios = _configuration["urlDominioServicios"];
                }
            }
            return urlDominioServicios;
        }

        public string ObtenerNombreCortoComunidad()
        {
            return nombreCortoComunidad;
        }

        public string ObtenerOauthConnectionString()
        {
            if (string.IsNullOrEmpty(sqlOauthConnectionString))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("oauth"))
                {
                    sqlOauthConnectionString = environmentVariables["oauth"] as string;
                }
                else
                {
                    sqlOauthConnectionString = _configuration.GetConnectionString("oauth");
                }
            }
            return sqlOauthConnectionString;
        }

        public string ObtenerAcidConnectionString()
        {
            if (string.IsNullOrEmpty(sqlAcidConnectionString))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("acid"))
                {
                    sqlAcidConnectionString = environmentVariables["acid"] as string;
                }
                else
                {
                    sqlAcidConnectionString = _configuration.GetConnectionString("acid");
                }
            }
            return sqlAcidConnectionString;
        }

        public string ObtenerTipoBD()
        {
            if (string.IsNullOrEmpty(tipoBD))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("connectionType"))
                {
                    tipoBD = environmentVariables["connectionType"] as string;
                }
                else
                {
                    tipoBD = _configuration.GetConnectionString("connectionType");
                }
            }
            return tipoBD;
        }

        public string ObtenerAPIConsumerKey()
        {
            if (string.IsNullOrEmpty(APIConsumerKey))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("APIConsumerKey"))
                {
                    APIConsumerKey = environmentVariables["APIConsumerKey"] as string;
                }
                else
                {
                    APIConsumerKey = _configuration["APIConsumerKey"];
                }
            }
            return APIConsumerKey;
        }

        public string ObtenerAPIConsumerSecret()
        {
            if (string.IsNullOrEmpty(APIConsumerSecret))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("APIConsumerSecret"))
                {
                    APIConsumerSecret = environmentVariables["APIConsumerSecret"] as string;
                }
                else
                {
                    APIConsumerSecret = _configuration["APIConsumerSecret"];
                }
            }
            return APIConsumerSecret;
        }

        public string ObtenerAPITokenKey()
        {
            if (string.IsNullOrEmpty(APITokenKey))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("APITokenKey"))
                {
                    APITokenKey = environmentVariables["APITokenKey"] as string;
                }
                else
                {
                    APITokenKey = _configuration["APITokenKey"];
                }
            }
            return APITokenKey;
        }

        public string ObtenerAPITokenSecret()
        {
            if (string.IsNullOrEmpty(APITokenSecret))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("APITokenSecret"))
                {
                    APITokenSecret = environmentVariables["APITokenSecret"] as string;
                }
                else
                {
                    APITokenSecret = _configuration["APITokenSecret"];
                }
            }
            return APITokenSecret;
        }


        public string ObtenerLoginAdmin()
        {
            return loginAdmin;
        }

        public string ObtenerPassAdmin()
        {
            if (string.IsNullOrEmpty(passAdmin))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("passAdmin"))
                {
                    passAdmin = environmentVariables["passAdmin"] as string;
                }
                else
                {
                    passAdmin = _configuration.GetConnectionString("passAdmin");
                }
            }
            return passAdmin;
        }

        public string ObtenerUrlPropia()
        {
            if (string.IsNullOrEmpty(urlPropia))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("urlPropia"))
                {
                    urlPropia = environmentVariables["urlPropia"] as string;
                }
                else
                {
                    urlPropia = _configuration["urlPropia"];
                }
            }
            return urlPropia;
        }
    }
}
