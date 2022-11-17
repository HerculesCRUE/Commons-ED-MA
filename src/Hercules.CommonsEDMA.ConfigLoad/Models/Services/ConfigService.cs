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
        private string urlAPIDespliegues { get; set; }

        private string urlDominioServicios { get; set; }

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
    }
}
