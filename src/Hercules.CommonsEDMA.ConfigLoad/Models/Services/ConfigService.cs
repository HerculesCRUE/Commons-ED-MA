using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.IO;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.Services
{
    public class ConfigService
    {
        private IConfiguration _configuration { get; set; }

        private string loginAdmin { get; set; }
        private string passAdmin { get; set; }
        private string nombreCortoComunidad = "hercules";
        private string urlAPIDespliegues { get; set; }
        private string urlServiciosBase { get; set; }
        private string urlServiciosInstalacion { get; set; }

        private string urlContentBase { get; set; }
        private string urlContentInstalacion { get; set; }

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


        public string ObtenerLoginAdmin()
        {
            if (string.IsNullOrEmpty(loginAdmin))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("loginAdmin"))
                {
                    loginAdmin = environmentVariables["loginAdmin"] as string;
                }
                else
                {
                    loginAdmin = _configuration["loginAdmin"];
                }
            }
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
                    passAdmin = _configuration["passAdmin"];
                }
            }
            return passAdmin;
        }

        public string ObtenerUrlServiciosBase()
        {
            if (string.IsNullOrEmpty(urlServiciosBase))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("urlServiciosBase"))
                {
                    urlServiciosBase = environmentVariables["urlServiciosBase"] as string;
                }
                else
                {
                    urlServiciosBase = _configuration["urlServiciosBase"];
                }
            }
            return urlServiciosBase;
        }

        public string ObtenerUrlServiciosInstalacion()
        {
            if (string.IsNullOrEmpty(urlServiciosInstalacion))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("urlServiciosInstalacion"))
                {
                    urlServiciosInstalacion = environmentVariables["urlServiciosInstalacion"] as string;
                }
                else
                {
                    urlServiciosInstalacion = _configuration["urlServiciosInstalacion"];
                }
            }
            return urlServiciosInstalacion;
        }

        public string ObtenerUrlContentBase()
        {
            if (string.IsNullOrEmpty(urlContentBase))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("urlContentBase"))
                {
                    urlContentBase = environmentVariables["urlContentBase"] as string;
                }
                else
                {
                    urlContentBase = _configuration["urlContentBase"];
                }
            }
            return urlContentBase;
        }

        public string ObtenerUrlContentInstalacion()
        {
            if (string.IsNullOrEmpty(urlContentInstalacion))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("urlContentInstalacion"))
                {
                    urlContentInstalacion = environmentVariables["urlContentInstalacion"] as string;
                }
                else
                {
                    urlContentInstalacion = _configuration["urlContentInstalacion"];
                }
            }
            return urlContentInstalacion;
        }


        public string ObtenerNombreCortoComunidad()
        {            
            return nombreCortoComunidad;
        }                
    }
}
